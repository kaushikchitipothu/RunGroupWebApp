using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces
{
    public interface IDashboardRepository
    {
        public Task<List<Race>> getAllUserRaces();
        public Task<List<Club>> getAllUserClubs();
    }
}
