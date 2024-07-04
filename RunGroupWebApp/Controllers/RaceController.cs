using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repository;
using RunGroupWebApp.Services;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class RaceController : Controller
    {
        IRaceRepository _raceRepository;
        IPhotoService _photoService;
        public RaceController(IRaceRepository raceRepository, IPhotoService photoService)
        {
            _raceRepository = raceRepository;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races =  await _raceRepository.GetAll();
            return View(races);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _raceRepository.GetByIdAsync(id);
            return View(race);
        }

        public IActionResult Create()
        {
          return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceVM.Image);

                var race = new Race
                {

                    Image = result.Url.ToString(),
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Address = new Address
                    {
                        City = raceVM.Address.City,
                        State = raceVM.Address.State,
                        Street = raceVM.Address.Street
                    },
                    RaceCategory = raceVM.RaceCategory
                };
                _raceRepository.Add(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", " Error in creating club");
            }
            return View(raceVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _raceRepository.GetByIdAsync(id);
            if (result == null)
            {
                return View("Error");
            }
            var clubVM = new EditRaceViewModel
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                AddressId = result.AddressId,
                Address = result.Address,
                RaceCategory = result.RaceCategory,
                URL = result.Image
            };
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Unable to edit");
                return View("Edit", raceVM);
            }
            var result = await _raceRepository.GetByIdAsyncNoTracking(id);
            if ((result != null))
            {
                try
                {
                    await _photoService.DeletePhotoAsync(result.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Couldn't delete the image");
                    return View("Edit", raceVM);
                }
            }
            var upload = await _photoService.AddPhotoAsync(raceVM.Image);
            var race = new Race
            {
                Id = raceVM.Id,
                Title = raceVM.Title,
                Description = raceVM.Description,
                AddressId = raceVM.AddressId,
                Address = raceVM.Address,
                Image = upload.Url.ToString()
            };
            _raceRepository.Update(race);
            return RedirectToAction("Index");
        }
    }
}
