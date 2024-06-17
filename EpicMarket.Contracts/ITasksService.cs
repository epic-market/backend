using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    
    public interface ITasksService
    {
        Task<long> SaveTask(TasksDTO tasksDTO);

        Task<int> SaveComments(CommentDTO commentDTO);
        Task<GetDataResult<List<CommentDTO>>> GetAllComments( int taskId);
        Task<TasksDTO> GettaskDetails(int taskId);
    }
}
