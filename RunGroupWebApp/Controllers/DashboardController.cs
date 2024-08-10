using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Repository;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
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

        public async Task<IActionResult> EditUserProfile()
        {
            var id = _httpContextAccessor.HttpContext.User.GetUserID();
            var user = await _dashboardRepository.getUserById(id);
            if (user == null)
            {
                return View("Error");
            }
            var editUserViewModel = new EditUserViewModel()
            {
                Id = id,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State,
            };
            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserViewModel EditVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile", EditVM);
            }
            var user = await _dashboardRepository.getUserByIdNoTracking(EditVM.Id);
            if (user.ProfileImageUrl == null || user.ProfileImageUrl == "")
            {
                var photoResult = await _photoService.AddPhotoAsync(EditVM.Image);
                user.Id = EditVM.Id;
                user.City = EditVM.City;
                user.State = EditVM.State;
                user.Pace = EditVM.Pace;
                user.Mileage = EditVM.Mileage;
                user.ProfileImageUrl = photoResult.Url.ToString();

                _dashboardRepository.update(user);
                return RedirectToAction("Index","User");

            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Coudn't delete profile image.");
                    return View(EditVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(EditVM.Image);
                user.Id = EditVM.Id;
                user.City = EditVM.City;
                user.State = EditVM.State;
                user.Pace = EditVM.Pace;
                user.Mileage = EditVM.Mileage;
                user.ProfileImageUrl = photoResult.Url.ToString();

                _dashboardRepository.update(user);
                return RedirectToAction("Index","User");
            }
        }
    }
}
