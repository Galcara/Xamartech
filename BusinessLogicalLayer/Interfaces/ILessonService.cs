using Common;
using Metadata;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogicalLayer.Interfaces
{
    public interface ILessonService
    {
        Task<Response> Update(Lesson item);
        Task<SingleResponse<Lesson>> GetByID(int id);
        Task<QueryResponse<Lesson>> GetAll();
        Task<QueryResponse<Lesson>> GenerateSchedule(DateTime dataInicio);
        Task<Response> InsertAllGeneratedLessons(List<Lesson> lessons);
    }
}
