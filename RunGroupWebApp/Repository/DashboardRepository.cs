﻿using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repository
{
    public class DashboardRepository :IDashboardRepository
    {
        private readonly ApplicationDBContext _dbcontext;
        private readonly IHttpContextAccessor _httpcontext;
        public DashboardRepository(ApplicationDBContext context, IHttpContextAccessor httpContext) { 
           _dbcontext = context;
           _httpcontext = httpContext;
        }

        public async Task<List<Club>> getAllUserClubs()
        {
            var userId = _httpcontext.HttpContext?.User.GetUserID();
            var userClubs = _dbcontext.Clubs.Where(r => r.AppUserId == userId);
            return userClubs.ToList();
        }

        public async Task<List<Race>> getAllUserRaces()
        {
            var userId = _httpcontext.HttpContext?.User.GetUserID();
            var userRaces = _dbcontext.Races.Where(r => r.AppUserId == userId);
            return userRaces.ToList();
        }
    }
}
