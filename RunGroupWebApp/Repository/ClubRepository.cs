using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repository
{
    public class ClubRepository : IClubRepository
    {
        ApplicationDBContext _dbContext;
        public ClubRepository(ApplicationDBContext  context) 
        {
            _dbContext = context; 
        }
        public bool Add(Club club)
        {
            _dbContext.Add(club);
            return  Save();
        }

        public bool Delete(Club club)
        {
            _dbContext.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAll()
        {
            return await _dbContext.Clubs.ToListAsync();
        }

        public async Task<Club> GetByIdAsync(int id)
        {
            return await _dbContext.Clubs.Include(i => i.Address).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            return await _dbContext.Clubs.Where(c => c.Address.City == city).ToListAsync();
        }

        public bool Save()
        {
            var save = _dbContext.SaveChanges();
            return  save == 0 ? true : false;
        }

        public bool Update(Club club)
        {
            _dbContext.Update(club);
            return Save();
        }
    }
}
