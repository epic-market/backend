namespace EpicMarket.Admin.MVC.Services
{
    public interface IUserRepository
    {
        bool HasPermission(string username, string securable);
    }
} 