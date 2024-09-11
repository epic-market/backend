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
        Task<long> SaveTask(TasksDTO tasksDTO, int AdminPersonID, string LoggedInUserName);

		Task<int> SaveComments(CommentDTO commentDTO , string LoggedInUserName);
        Task<GetDataResult<List<CommentListDTO>>> GetAllComments(CommentListParams commentDTO); 
        Task<GetDataResult<List<TasksListDTO>>> GetSupportByPersonId( int personId, TasksListParams tasksListParams);
        Task<TaskDeatilDTO> GettaskDetails(int taskId);
		Task<long> AddSupportTask(SupportDTO supportDTO, int AdminPersonID);
    }
}
