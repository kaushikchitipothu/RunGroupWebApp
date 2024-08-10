using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        public UserRepository(ApplicationDBContext context) { 
             _context = context;
        }
        public bool Add(AppUser user)
        {
            throw new NotImplementedException();
        }

        public bool delete(AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppUser>> getAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserID(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public bool save()
        {
           var saved = _context.SaveChanges();
           return saved==0 ? true:false;
        }

        public bool update(AppUser user)
        {
            _context.Users.Update(user);
            return save();
        }
    }
}
