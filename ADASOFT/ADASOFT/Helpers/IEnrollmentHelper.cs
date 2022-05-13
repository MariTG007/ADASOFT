using ADASOFT.Common;
using ADASOFT.Models;

namespace ADASOFT.Helpers
{
    public interface IEnrollmentHelper
    {
        Task<Response> ProcessOrderAsync(ShowCartViewModel model);

        Task<Response> CancelOrderAsync(int id);

    }
}
