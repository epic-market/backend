using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EpicMarket.Entities.CustomModels;
using Microsoft.AspNetCore.Http;

namespace EpicMarket.Entities
{
    public class TasksDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? TaskTypeID { get; set; }
        public string TaskData { get; set; }
        public IFormFile[] UploadFiles { get; set; }
    }
       public class AttachmentTASKDTO
    {
        public int TaskID { get; set; }
        public IFormFile[] UploadFiles { get; set; }
    }

    public class TaskDeatilDTO
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? TaskTypeID { get; set; }
        public string? TaskType { get; set; }
        public string TaskData { get; set; }
        public string TaskStatus { get; set; }
        public DateTime? CreateDate { get; set; }
        public List<string> UploadFiles { get; set; }
    }

    public class TasksListDTO
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public int? TaskTypeID { get; set; }
        public string TaskType { get; set; }
        public string TaskData { get; set; }
        public string TaskStatus { get; set; }
        public DateTime? CreateDate { get; set; }
    }
    public class TasksListParams
    {
        public int PageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;

    }
    public class CommentDTO
    {
        public string CommentText { get; set; }
        public string Status { get; set; }
        public int? TaskId { get; set; }
        public IFormFile[] UploadFiles { get; set; }
    }
    public class CommentListDTO
    {
        public int? ID { get; set; }
        public string CommentText { get; set; }
        public string Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
    }

    public class SupportDTO
    {
        public string Email { get; set; }

        public string Phonenumber { get; set; }
        public string Fullname { get; set; }
        public int TypeofPersonid { get; set; }
 
        public int? QueryId { get; set; }
        public string Comment { get; set; }


    }

}
