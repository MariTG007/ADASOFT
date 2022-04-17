using Microsoft.AspNetCore.Mvc.Rendering;

namespace ADASOFT.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        public Task<IEnumerable<SelectListItem>> GetComboAttendantAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SelectListItem>> GetComboCampusAsync(int cityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SelectListItem>> GetComboStatesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
