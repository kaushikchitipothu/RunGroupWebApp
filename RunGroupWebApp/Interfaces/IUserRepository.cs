using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces
{
    public interface IUserRepository
    {
        public Task<AppUser> GetUserID(string id);
        public bool save();
        public bool delete(AppUser user);
        public bool update(AppUser user);
        public bool Add(AppUser user);
        public Task<IEnumerable<AppUser>> getAllUsers();

    }
}
