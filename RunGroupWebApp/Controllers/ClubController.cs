using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repository;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClubController(IClubRepository clubRepository, IPhotoService photoService,IHttpContextAccessor httpContextAccessor)
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Club club = await _clubRepository.GetByIdAsync(id);
            return View(club);
        }

        public IActionResult Create()
        {
            var user = _httpContextAccessor.HttpContext?.User.GetUserID();
            var createClubViewModel = new CreateClubViewModel() { AppUserId = user };
            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {

                    Image = result.Url.ToString(),
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    AppUserId = clubVM.AppUserId,
                    Address = new Address
                    {
                        City = clubVM.Address.City,
                        State = clubVM.Address.State,
                        Street = clubVM.Address.Street
                    },
                    ClubCategory = clubVM.ClubCategory
                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", " Error in creating club");
            }
            return View(clubVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _clubRepository.GetByIdAsync(id);
            if (result == null)
            {
                return View("Error");
            }
            var clubVM = new EditClubViewModel
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                AddressId = result.AddressId,
                Address = result.Address,
                ClubCategory = result.ClubCategory,
                URL = result.Image
            };
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Unable to edit");
                return View("Edit", clubVM);
            }
            var result = await _clubRepository.GetByIdAsyncNoTracking(id);
            if ((result != null))
            {
                try
                {
                    await _photoService.DeletePhotoAsync(result.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Couldn't delete the image");
                    return View("Edit", clubVM);
                }
            }
            var upload = await _photoService.AddPhotoAsync(clubVM.Image);
            var club = new Club
            {
                Id = clubVM.Id,
                Title = clubVM.Title,
                Description = clubVM.Description,
                AddressId = clubVM.AddressId,
                Address = clubVM.Address,
                Image = upload.Url.ToString()
            };
            _clubRepository.Update(club);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var res = await _clubRepository.GetByIdAsync(id);
            if (res == null)
            {
                return View("Error");
            }
            return View(res);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteClub(int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                var res = await _clubRepository.GetByIdAsync(id);
                if(res == null)
                {
                    return View("error");
                }
                _clubRepository.Delete(res);
                return RedirectToAction("Index");
            }
        }
    }
}
