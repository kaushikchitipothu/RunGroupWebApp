using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces
{
    public interface IDashboardRepository
    {
        public Task<List<Race>> getAllUserRaces();
        public Task<List<Club>> getAllUserClubs();

        public Task<AppUser> getUserById(string id);

        public Task<AppUser> getUserByIdNoTracking(string id);

        public bool update(AppUser user);
        public bool save();
    }
}
