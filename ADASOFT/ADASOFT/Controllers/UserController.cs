﻿using ADASOFT.Data;
using ADASOFT.Data.Entities;
using ADASOFT.Helpers;
using ADASOFT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ADASOFT.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        // private readonly IBlobHelper _blobHelper;
        // private readonly ICombosHelper _combosHelper;
        /*
         public UsersController(DataContext context, IUserHelper userHelper, IBlobHelper blobHelper, ICombosHelper combosHelper)
         {
             _context = context;
             _userHelper = userHelper;
             _blobHelper = blobHelper;
             _combosHelper = combosHelper;
         }

         public async Task<IActionResult> Index()
         {
             return View(await _context.Users
                 .Include(u => u.City)
                 .ThenInclude(c => c.State)
                 .ThenInclude(s => s.Country)
                 .ToListAsync());
         }

         public async Task<IActionResult> Create()
         {
             AddUserViewModel model = new()
             {
                 Id = Guid.Empty.ToString(),
                 Countries = await _combosHelper.GetComboCountriesAsync(),
                 States = await _combosHelper.GetComboStatesAsync(0),
                 Cities = await _combosHelper.GetComboCitiesAsync(0),
                 UserType = UserType.Admin,
             };

             return View(model);
         }

         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create(AddUserViewModel model)
         {
             if (ModelState.IsValid)
             {
                 Guid imageId = Guid.Empty;

                 if (model.ImageFile != null)
                 {
                     imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                 }

                 model.ImageId = imageId;
                 User user = await _userHelper.AddUserAsync(model);
                 if (user == null)
                 {
                     ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                     model.Countries = await _combosHelper.GetComboCountriesAsync();
                     model.States = await _combosHelper.GetComboStatesAsync(model.CountryId);
                     model.Cities = await _combosHelper.GetComboCitiesAsync(model.StateId);
                     return View(model);
                 }

                 return RedirectToAction("Index", "Home");
             }

             model.Countries = await _combosHelper.GetComboCountriesAsync();
             model.States = await _combosHelper.GetComboStatesAsync(model.CountryId);
             model.Cities = await _combosHelper.GetComboCitiesAsync(model.StateId);
             return View(model);
         }

         public JsonResult GetStates(int countryId)
         {
             Country country = _context.Countries
                 .Include(c => c.States)
                 .FirstOrDefault(c => c.Id == countryId);
             if (country == null)
             {
                 return null;
             }

             return Json(country.States.OrderBy(d => d.Name));
         }

         public JsonResult GetCities(int stateId)
         {
             State state = _context.States
                 .Include(s => s.Cities)
                 .FirstOrDefault(s => s.Id == stateId);
             if (state == null)
             {
                 return null;
             }

             return Json(state.Cities.OrderBy(c => c.Name));
         } */

        
       public async Task<IActionResult> AddAttendant(int? id)
       {
           if (id == null)
           {
               return NotFound();
           }

           User user = await _context.Users.FindAsync(id);

           if (user == null)
           {
               return NotFound();
           }

           AttendantViewModel model = new()
           {
               UserId = user.Id,
           };

           return View(model);
       }


       [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> AddAttendant(AttendantViewModel model)
       {
           if (ModelState.IsValid)
           {
               try
               {
                   Attendant attendant = new()
                   {


                       User = await _context.Users.FindAsync(model.UserId),
                       FirstName = model.FirstName,

                   };
                   _context.Add(attendant);
                   await _context.SaveChangesAsync();
                   return RedirectToAction(nameof(HomeController), new { Id = model.UserId });
               }
               //TODO: remmember that duplicate is with first name and lastname
               
               catch (DbUpdateException dbUpdateException)
               {
                   if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                   {
                       ModelState.AddModelError(string.Empty, "Ya existe una acudiente  con el mismo nombre para este usuario.");
                   }
                   else
                   {
                       ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                   }
               }
               catch (Exception exception)
               {
                   ModelState.AddModelError(string.Empty, exception.Message);
               }
           }
           return View(model);

       }

       public async Task<IActionResult> DetailsAttendant(int? id)
       {
           if (id == null)
           {
               return NotFound();
           }

           Attendant attendant = await _context.Attendantes
               .Include(a => a.User)
               .FirstOrDefaultAsync(a => a.Id == id);
           if (attendant == null)
           {
               return NotFound();
           }

           return View(attendant);
       }

       public async Task<IActionResult> EditAttendant(int? id)
       {
           if (id == null)
           {
               return NotFound();
           }

           Attendant attendant = await _context.Attendantes
               .Include(a => a.User)
               .FirstOrDefaultAsync(a=> a.Id == id);
           if (attendant == null)
           {
               return NotFound();
           }
           AttendantViewModel model = new()
           {
               UserId = attendant.User.Id,
               Id = attendant.Id,
               FirstName = attendant.FirstName,
           };

           return View(model);
       }




       [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> EditAttendant(int id, AttendantViewModel model)
       {
           if (id != model.Id)
           {
               return NotFound();
           }

           if (ModelState.IsValid)
           {
               try
               {
                   Attendant attendant = new()
                   {
                       Id = model.Id,
                       FirstName = model.FirstName,

                   };
                   _context.Update(attendant);
                   await _context.SaveChangesAsync();
                   return RedirectToAction(nameof(HomeController), new { Id = model.UserId });
               }
               catch (DbUpdateException dbUpdateException)
               {
                   if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                   {
                       ModelState.AddModelError(string.Empty, "Ya existe un ac acudiente " +
                                                               "con el mismo nombre para este usuario.");
                   }
                   else
                   {
                       ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                   }
               }
               catch (Exception exception)
               {
                   ModelState.AddModelError(string.Empty, exception.Message);
               }

           }
           return View(model);
       }

       public async Task<IActionResult> DeleteAttendant(int? id)
       {
           if (id == null)
           {
               return NotFound();
           }

           Campus campus = await _context.Campuses
               .Include(c => c.City)
               .FirstOrDefaultAsync(c => c.Id == id); //FirstOrDefault instead of FindAsync, allows to use Include
           if (campus == null)
           {
               return NotFound();
           }

           return View(campus);
       }


       [HttpPost, ActionName("DeleteAttendant")]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> DeleteAttendantConfirmed(int id)
       {
           Attendant attendant = await _context.Attendantes
              .Include(a => a.User)
              .FirstOrDefaultAsync(c => c.Id == id);
           _context.Attendantes.Remove(attendant);
           await _context.SaveChangesAsync();
           return RedirectToAction(nameof(HomeController));
       }

    }
}
