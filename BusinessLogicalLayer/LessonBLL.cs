using BusinessLogicalLayer.Interfaces;
using Common;
using DataAccessLayer;
using Metadata;
using Metadata.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogicalLayer
{
    public class LessonBLL : BaseValidator<Lesson>, ILessonService
    {
        private readonly ITeacherService _teacherService;
        private readonly IClassService _classService;
        private readonly ISubjectService _subjectService;

        public LessonBLL(ITeacherService teacherService, IClassService classService, ISubjectService subjectService)
        {
            this._teacherService = teacherService;
            this._classService = classService;
            this._subjectService = subjectService;
        }

        public override Response Validate(Lesson lesson)
        {
            return base.Validate(lesson);
        }

        public async Task<Response> Update(Lesson lesson)
        {

            Response validationResponse = Validate(lesson);
            if (!validationResponse.Success)
            {
                return validationResponse;
            }
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    dataBase.Entry(lesson).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await dataBase.SaveChangesAsync();
                }
                return ResponseMessage.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                return ResponseMessage.CreateErrorResponse(ex);
            }

        }

        public async Task<QueryResponse<Lesson>> GetAll()
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    List<Lesson> lessons = await dataBase.Lessons.ToListAsync();
                    return ResponseMessage.CreateQuerySuccessResponse<Lesson>(lessons);
                }
            }
            catch (Exception ex)
            {
                QueryResponse<Lesson> lesson = (QueryResponse<Lesson>)ResponseMessage.CreateErrorResponse(ex);
                return lesson;
            }

        }

        public async Task<SingleResponse<Lesson>> GetByID(int id)
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Lesson lesson = await dataBase.Lessons.FirstOrDefaultAsync(p => p.ID == id);
                    if (lesson == null)
                    {
                        return ResponseMessage.CreateNotFoundData<Lesson>();
                    }
                    return ResponseMessage.CreateSingleSuccessResponse<Lesson>(lesson);
                }
            }
            catch (Exception ex)
            {
                SingleResponse<Lesson> lesson = (SingleResponse<Lesson>)ResponseMessage.CreateErrorResponse(ex);
                return lesson;
            }
        }

        public async Task<QueryResponse<Lesson>> GenerateSchedule(DateTime schoolYearBegin)
        {
            var allTeachers = await _teacherService.GetAll();
            var allSubjects = await _subjectService.GetAll();
            var allClasses = await _classService.GetAll();

            List<Teacher> teachers = allTeachers.Data;
            List<Subject> subjects = allSubjects.Data;
            List<Class> classes = allClasses.Data;
            Random rdm = new Random();
            List<Lesson> lessons = new List<Lesson>();
            int lessonDuration = 45;

            List<Subject> subjecstWithFrequency = new List<Subject>();
            foreach (Subject subject in subjects)
            {
                for (int i = 0; i < subject.Frequency; i++)
                {
                    subjecstWithFrequency.Add(subject);
                }
            }

            List<Subject> classSubjects = subjecstWithFrequency.ToList();
            Dictionary<int, List<Subject>> dictionary = new Dictionary<int, List<Subject>>();

            foreach (Class item in classes)
            {
                dictionary.Add(item.ID, classSubjects.ToList());
            }

            //PRA CADA DIA DA SEMANA
            for (int day = 1; day < 6; day++)
            {
                //PRA CADA AULA
                for (int order = 0; order < 5; order++)
                {
                    List<Teacher> teachersAvailable = teachers.ToList();

                    //PRA CADA TURMA
                    foreach (Class @class in classes)
                    {
                    //dia = 1
                    //ordem = 0
                    //turma a
                    Begin:
                        //SEGUNDA 1ªaula
                        int indexSubjectDrawn = rdm.Next(0, dictionary[@class.ID].Count);
                        Subject subjectDrawn = null;

                        try
                        {
                            subjectDrawn = dictionary[@class.ID][indexSubjectDrawn];
                        }
                        catch (Exception ex)
                        {

                        }

                        DateTime date = new DateTime(schoolYearBegin.Year, schoolYearBegin.Month, schoolYearBegin.Day + (day - 1));
                        Shift shift = @class.ClassShift;
                        if (shift == Shift.Matutino)
                        {
                            date = date.AddMinutes(450);
                        }
                        else if (shift == Shift.Vespertino)
                        {
                            date = date.AddMinutes(810);
                        }
                        else
                        {
                            date = date.AddMinutes(1110);
                        }

                        date = date.AddMinutes(lessonDuration * order);

                        if (order >= 3)
                        {
                            date = date.AddMinutes(15);
                        }

                        int indexTeacherOfSubject =
                            rdm.Next(0, subjectDrawn.Teachers.Count);

                        if (subjectDrawn.Teachers.Count == 0)
                        {
                            return new QueryResponse<Lesson>()
                            {
                                Message = "A matéria " + subjectDrawn.SubjectName + " não possui professores disponíveis.",
                                Success = false
                            };
                        }

                        Teacher teacherDrawn =
                            subjectDrawn.Teachers.ToList()[indexTeacherOfSubject];

                        bool find = false;
                        //Remove o professor que estava disponível da lista
                        for (int i = 0; i < teachersAvailable.Count; i++)
                        {
                            if (teachersAvailable[i].ID == teacherDrawn.ID)
                            {
                                teachersAvailable.RemoveAt(i);
                                find = true;
                                break;
                            }
                        }
                        if (!find)
                        {
                            goto Begin;
                        }
                        //Remove a matéria sorteada
                        dictionary[@class.ID].RemoveAt(indexSubjectDrawn);


                        lessons.Add(new Lesson()
                        {
                            //Presences = new List<Presence>
                            ClassID = @class.ID,
                            date = date,
                            Order = (LessonOrder)order,
                            Shift = shift,
                            LessonDate = (DayOfWeek)day,
                            SubjectID = subjectDrawn.ID,
                            TeacherID = teacherDrawn.ID
                        });

                    }
                }
            }

            return ResponseMessage.CreateQuerySuccessResponse<Lesson>(lessons);


        }

        public async Task<Response> InsertAllGeneratedLessons(List<Lesson> lessons)
        {
            try
            {
                using (BiometricPresenceDB db = new BiometricPresenceDB())
                {
                    db.Lessons.AddRange(lessons);

                    foreach (Lesson item in lessons)
                    {
                        item.Presences = new List<Presence>();
                        Class turma = await db.Classes.Include(c => c.Students).FirstOrDefaultAsync(i => i.ID == item.ClassID);
                        foreach (Student student in turma.Students.ToList())
                        {
                            item.Presences.Add(new Presence()
                            {
                                Lesson = item,
                                StudentID = student.ID,
                                Attendance = false,
                            });
                        }
                    }

                    await db.SaveChangesAsync();
                    return ResponseMessage.CreateSuccessResponse();
                }
            }
            catch (Exception ex)
            {
                return ResponseMessage.CreateErrorResponse(ex);
            }
        }
    }
}
