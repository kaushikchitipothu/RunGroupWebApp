using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Repository;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.getAllUsers();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userViewModel = new UserViewModel()
                {
                    Id = user.Id,
                    Pace =  user.Pace,
                    Mileage = user.Mileage,
                    UserName = user.UserName,
                    ProfileImageUrl = user.ProfileImageUrl
                };
                result.Add(userViewModel);
            }
            return View(result);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var userDetail = await _userRepository.GetUserID(id);
            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = userDetail.Id,
                Pace = userDetail.Pace,
                Mileage = userDetail.Mileage,
                UserName = userDetail.UserName,

            };
            return View(userDetailViewModel);
        }
    }
}
