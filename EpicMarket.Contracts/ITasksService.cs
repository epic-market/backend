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
        long SaveTask(TasksDTO tasksDTO);

        int SaveComments(CommentDTO commentDTO);
        Task<GetDataResult<List<CommentDTO>>> GetAllComments( int taskId); 
        Task<GetDataResult<List<TasksDTO>>> GetSupportByPersonId( int personId);
        Task<TasksDTO> GettaskDetails(int taskId);
        long AddSupportTask(SupportDTO supportDTO, int AdminPersonID);
    }
}
