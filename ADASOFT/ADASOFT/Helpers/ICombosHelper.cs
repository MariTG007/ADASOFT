using Microsoft.AspNetCore.Mvc.Rendering;

namespace ADASOFT.Helpers
{
    public interface ICombosHelper
    {
        //Task<IEnumerable<SelectListItem>> GetComboAttendantAsync();

        Task<IEnumerable<SelectListItem>> GetComboStatesAsync();

        Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId);

        Task<IEnumerable<SelectListItem>> GetComboCampusesAsync(int cityId);

        Task<IEnumerable<SelectListItem>> GetComboTeachersAsync();
    }
}
