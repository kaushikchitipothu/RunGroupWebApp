using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RunGroupWebApp.Helpers;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels;
using System.Diagnostics;
using System.Globalization;
using System.Net;

namespace RunGroupWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClubRepository _clubRepository;

        public HomeController(ILogger<HomeController> logger,IClubRepository clubRepository)
        {
            _logger = logger;
            _clubRepository = clubRepository;
        }

        public async Task<IActionResult> Index()
        {
            var ipInfo = new IPInfo();
            var homeViewModel = new HomeViewModel();
            try
            {
                string url = "https://ipinfo.io/52.124.45.17?token=bdbc46dcfd78ee";
                var info = new WebClient().DownloadString(url);
                ipInfo = JsonConvert.DeserializeObject<IPInfo>(info);
                RegionInfo regionInfo = new RegionInfo(ipInfo.Country);
                homeViewModel.country = regionInfo.EnglishName;
                homeViewModel.city = ipInfo.City;
                homeViewModel.state = ipInfo.Region;
                if (homeViewModel.city != null)
                {
                    homeViewModel.clubs = await _clubRepository.GetClubByCity(homeViewModel.city);
                }
                else
                {
                    homeViewModel.clubs = null;
                }
                return View(homeViewModel);
            }
            catch (Exception ex)
            {
                homeViewModel.clubs = null;
            }
            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
