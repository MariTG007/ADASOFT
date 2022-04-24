using ADASOFT.Common;
using ADASOFT.Data;
using ADASOFT.Data.Entities;
using ADASOFT.Enums;
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
        private readonly IBlobHelper _blobHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IMailHelper _mailHelper;


        public UsersController(DataContext context, IUserHelper userHelper, IBlobHelper blobHelper,
             ICombosHelper combosHelper, IMailHelper mailHelper)
         {
             _context = context;
             _userHelper = userHelper;
             _blobHelper = blobHelper;
             _combosHelper = combosHelper;
            _mailHelper = mailHelper;
         }

         public async Task<IActionResult> Index()
         {
             return View(await _context.Users
                 .Include(u => u.Campus)
                 .ThenInclude(c => c.City)
                 .ThenInclude(c => c.State)
                 .ToListAsync());
         }

         public async Task<IActionResult> Create()
         {
             AddUserViewModel model = new()
             {
                 Id = Guid.Empty.ToString(),
                 States = await _combosHelper.GetComboStatesAsync(),
                 Cities = await _combosHelper.GetComboCitiesAsync(0),
                 Campuses = await _combosHelper.GetComboCampusesAsync(0),
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
                     model.States = await _combosHelper.GetComboStatesAsync();
                     model.Cities = await _combosHelper.GetComboCitiesAsync(model.StateId);
                     model.Campuses = await _combosHelper.GetComboCampusesAsync(model.CityId);
                    return View(model);
                 }

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendMail(
                    $"{model.FirstName} {model.LastName}",
                    model.Username,
                    "ADASOFT - Confirmación de Email",
                    $"<h1>ADASOFT - Confirmación de Email</h1>" +
                        $"Para habilitar el usuario por favor hacer clicK en el siguiente link:, " +
                        $"<p><a href = \"{tokenLink}\">Confirmar Email</a></p>");
                if (response.IsSuccess)
                {
                    ViewBag.Message = "Las instrucciones para habilitar el usuario han sido enviadas al correo.";
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

             model.States = await _combosHelper.GetComboStatesAsync();
             model.Cities = await _combosHelper.GetComboCitiesAsync(model.StateId);
             model.Campuses = await _combosHelper.GetComboCampusesAsync(model.CityId);
            return View(model);
         }

         public JsonResult GetCities(int StateId)
         {
             State state = _context.States
                 .Include(s => s.Cities)
                 .FirstOrDefault(s => s.Id == StateId);
             if (state == null)
             {
                 return null;
             }

             return Json(state.Cities.OrderBy(c => c.Name));
         }

         public JsonResult GetCampuses(int CityId)
         {
             City city = _context.Cities
                 .Include(c => c.Campuses)
                 .FirstOrDefault(c => c.Id == CityId);
             if (city == null)
             {
                 return null;
             }

             return Json(city.Campuses.OrderBy(c => c.Name));
         } 

        
       public async Task<IActionResult> AddAttendant(string? id)
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
                   return RedirectToAction(nameof(Index));
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
