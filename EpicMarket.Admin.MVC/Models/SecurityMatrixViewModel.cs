namespace EpicMarket.Admin.MVC.Models
{
    public class SecurityMatrixViewModel
    {
        public List<string> RoleNames { get; set; }
        public List<SecurableViewModel> Securables { get; set; }
        public List<AccessTypeViewModel> AccessTypes { get; set; }
        public int DefaultAccessTypeId { get; set; }
    }

    public class SecurableViewModel
    {
        public string Name { get; set; }
        public Dictionary<string, int> RoleAccess { get; set; }
    }

    public class AccessTypeViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
