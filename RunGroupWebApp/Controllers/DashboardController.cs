using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Repository;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }
        public async Task<IActionResult> Index()
        {
            var _userRaces = await _dashboardRepository.getAllUserRaces();
            var _userClubs = await _dashboardRepository.getAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                userClubs = _userClubs,
                userRaces = _userRaces
            };

            return View(dashboardViewModel);
        }
    }
}
