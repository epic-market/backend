namespace EpicMarket.Admin.MVC.Models
{
	public class AttachmentModel
	{
		public string Name { get; set; }
		public string Comment { get; set; }
        public int RecordID { get; set; }
        public string Entity { get; set; }
		public string AttachmentType { get; set; }
		public int BusinessID { get; set; }
		public string FolderPathConstant { get; set; }
        public IFormFile[] Files { get; set; }

	}


    public class GetAttachmentLink
    {
        public string AttachmentType { get; set; }
        public int RecordID { get; set; }
        public string Entity { get; set; }
    }
}
