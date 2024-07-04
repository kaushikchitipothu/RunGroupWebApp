using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repository
{
    public class RaceRepository : IRaceRepository
    {
        ApplicationDBContext _dbContext;
        public RaceRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }
        public bool Add(Race race)
        {
            _dbContext.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _dbContext.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
            return await _dbContext.Races.ToListAsync();
        }

        public async Task<Race> GetByIdAsync(int id)
        {
            return await _dbContext.Races.Include(i => i.Address).FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Race> GetByIdAsyncNoTracking(int id)
        {
            return await _dbContext.Races.Include(i => i.Address).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Race>> GetRaceByCity(string city)
        {
            return await _dbContext.Races.Where(c=>c.Address.City == city).ToListAsync();
        }

        public bool Save()
        {
           int save  = _dbContext.SaveChanges();
            return save == 0;
        }

        public bool Update(Race race)
        {
            _dbContext.Update(race);
            return Save();
        }
    }
}
