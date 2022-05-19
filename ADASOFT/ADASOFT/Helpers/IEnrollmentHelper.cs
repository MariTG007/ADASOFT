using ADASOFT.Common;
using ADASOFT.Data.Entities;
using ADASOFT.Models;

namespace ADASOFT.Helpers
{
    public interface IEnrollmentHelper
    {
        Task<Response> ProcessOrderAsync(ShowCartViewModel model);

        Task<Response> ConfirmEnrollment(Enrollment enrollment);


        Task<Response> CancelOrderAsync(int id);

    }
}
