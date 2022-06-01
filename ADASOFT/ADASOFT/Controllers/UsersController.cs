using ADASOFT.Common;
using ADASOFT.Data;
using ADASOFT.Data.Entities;
using ADASOFT.Enums;
using ADASOFT.Helpers;
using ADASOFT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;
using static ADASOFT.Helpers.ModalHelper;

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
        private readonly IFlashMessage _flashMessage;

        public UsersController(DataContext context, IUserHelper userHelper, IBlobHelper blobHelper,
             ICombosHelper combosHelper, IMailHelper mailHelper, IFlashMessage flashMessage)
         {
            _context = context;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _combosHelper = combosHelper;
            _mailHelper = mailHelper;
            _flashMessage = flashMessage;
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
                    _flashMessage.Danger("Este correo ya está siendo usado.");
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
                    _flashMessage.Info("Las instrucciones para habilitar el usuario han sido enviadas al correo.");
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

        [NoDirectAccess]
        public async Task<IActionResult> AddAttendant(string id)
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
          // if (ModelState.IsValid)
           //{
               try
               {
                   Attendant attendant = new()
                   {
                       User = await _context.Users.FindAsync(model.UserId),
                       Document = model.Document,
                       FirstName = model.FirstName,
                       LastName = model.LastName,
                       Address = model.Address,
                       Phone = model.Phone,
                       Cellphone = model.Cellphone,
                       Email = model.Email
                   };
                   _context.Add(attendant);
                   await _context.SaveChangesAsync();
                _flashMessage.Confirmation("Registro creado.");
                return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAttendant", attendant) });
            }
               //TODO: remmember that duplicate is with first name and lastname
               
               catch (DbUpdateException dbUpdateException)
               {
                   if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                   {
                        _flashMessage.Danger("Ya existe una acudiente  con el mismo nombre para este usuario.");
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
          // } else {
                /*Attendant attendant = new()
                {
                     User = await _context.Users.FindAsync(model.UserId),
                     Document = model.Document,
                     FirstName = model.FirstName,
                     LastName = model.LastName,
                     Address= model.Address,
                     Phone = model.Phone,
                     Cellphone = model.Cellphone,
                     Email =model.Email
                };
                _context.Add(attendant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
               // ModelState.AddModelError(string.Empty, " no entra al hpta if no es valido el modelo.");
            }*/
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

        [NoDirectAccess]
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
               Document = attendant.Document,
               FirstName = attendant.FirstName,
               LastName = attendant.LastName,
               Address = attendant.Address,
               Phone = attendant.Phone,
               Cellphone = attendant.Cellphone,
               Email = attendant.Email
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
                       Document =model.Document,
                       FirstName = model.FirstName,
                       LastName = model.LastName,
                       Address = model.Address,
                       Phone = model.Phone,
                       Cellphone = model.Cellphone,
                       Email = model.Email
                   };

                   _context.Update(attendant);
                   await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Registro actualizado.");
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(this, "_ViewAttendant", attendant)
                    });
                }
               catch (DbUpdateException dbUpdateException)
               {
                   if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                   {
                        _flashMessage.Danger("Ya existe un ac acudiente " +
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
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "EditAttendant", model) });
        }

       public async Task<IActionResult> DeleteAttendant(int? id)
       {
            Attendant attendant = await _context.Attendantes
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(c => c.Id == id); //FirstOrDefault instead of FindAsync, allows to use Include
            if (attendant == null)
            {
                return NotFound();
            }

            _context.Attendantes.Remove(attendant);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Details), new { Id = attendant.User.Id });
        }

        public async Task<IActionResult> IndexAttendant(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Attendant attendant = await _context.Attendantes
                .Include(a => a.User)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (attendant == null)
            {
                return NotFound();
            }

            return View(attendant);
        }

        public async Task<IActionResult> Details(String? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _context.Users
             .Include(u => u.Attendantes)
            .Include(u=>u.Campus)
            .ThenInclude(c=>c.City)
            .ThenInclude(c=>c.State)
            .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}
