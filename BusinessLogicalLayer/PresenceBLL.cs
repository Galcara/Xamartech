using BusinessLogicalLayer.Interfaces;
using Common;
using DataAccessLayer;
using Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BusinessLogicalLayer
{
    public class PresenceBLL : BaseValidator<Presence>, IPresenceService
    {
        private readonly IStudentService _studentService;
        public PresenceBLL(IStudentService studentService)
        {
            this._studentService = studentService;
        }

        public override Response Validate(Presence presence)
        {
            return base.Validate(presence);
        }

        public async Task<Response> Insert(Presence presence)
        {
            Response response = Validate(presence);
            if (response.Success)
            {
                try
                {
                    using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                    {

                        dataBase.Presences.Add(presence);
                        await dataBase.SaveChangesAsync();
                        return ResponseMessage.CreateSuccessResponse();
                    }
                }
                catch (Exception ex)
                {
                    return ResponseMessage.CreateSingleErrorResponse<int>(ex);
                }
            }
            return ResponseMessage.CreateSingleErrorResponse<int>();
        }

        public Task<Response> Update(Presence presence)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResponse<Presence>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<SingleResponse<Presence>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> ApplyFrequency(string register)
        {
            using (BiometricPresenceDB db = new BiometricPresenceDB())
            {
                Student student = await db.Students.Include(c => c.Class).FirstOrDefaultAsync(c => c.Register == register);
                //TROCAR A DATA PRA TESTAR OUTROS HORÁRIOS
                DateTime dt = DateTime.Now;

                List<Lesson> todayLessons =
                    await db.Lessons.Include(c => c.Presences).Where(c => c.ClassID == student.ClassID && c.date.Date == dt.Date).ToListAsync();

                List<DateTime> schedules = new List<DateTime>();
                todayLessons.ForEach(c => schedules.Add(c.date));

                //13:30 - 14:15 - 15:00
                //15:19
                int indexLesson = 0;
                for (int i = 0; i < schedules.Count; i++)
                {
                    if (i == schedules.Count - 1)
                    {
                        indexLesson = i;
                        break;
                    }
                    if (dt > schedules[i] && dt < schedules[i + 1])
                    {
                        indexLesson = i;
                        break;
                    }
                }

                Presence presence = await db.Presences.FirstOrDefaultAsync(c => c.LessonID == todayLessons[indexLesson].ID && c.StudentID == student.ID);
                presence.Attendance = true;
                db.Entry(presence).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await db.SaveChangesAsync();
                return ResponseMessage.CreateSuccessResponse();
            }
        }
    }
}