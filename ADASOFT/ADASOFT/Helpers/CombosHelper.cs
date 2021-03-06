using ADASOFT.Data;
using ADASOFT.Data.Entities;
using ADASOFT.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ADASOFT.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCampusesAsync(int cityId)
        {
            List<SelectListItem> list = await _context.Campuses
                           .Where(s => s.City.Id == cityId)
                           .Select(c => new SelectListItem
                           {
                               Text = c.Name,
                               Value = c.Id.ToString()
                           })
                           .OrderBy(c => c.Text)
                           .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Seleccione un campus...", Value = "0" });
            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId)
        {
            List<SelectListItem> list = await _context.Cities
                            .Where(s => s.State.Id == stateId)
                            .Select(c => new SelectListItem
                            {
                                Text = c.Name,
                                Value = c.Id.ToString()
                            })
                            .OrderBy(c => c.Text)
                            .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Seleccione una ciudad...", Value = "0" });
            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboStatesAsync()
        {
            List<SelectListItem> list = await _context.States.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            })
                 .OrderBy(s => s.Text)
                 .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Seleccione un departamento...]", Value = "0" });
            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboTeachersAsync()
        {
            List<SelectListItem> list = await _context.Users
                .Where(u => u.UserType == UserType.Admin)
                .Select(u => new SelectListItem
            {
                Text = u.FirstName,
                Value = u.Id.ToString()
            })
                .OrderBy(u => u.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Seleccione un profesor...]", Value = "0" });
            return list;
        }
    }
}
