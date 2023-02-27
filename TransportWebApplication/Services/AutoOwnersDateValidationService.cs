using Microsoft.EntityFrameworkCore;
using TransportWebApplication.Models;

namespace TransportWebApplication.Services
{
    public static class AutoOwnersDateValidationService
    {
        public static async Task<bool> IsValidPlacement(AutoOwner autoOwner, TransportContext _context)
        {

            var autoOwners = await _context.AutoOwners.ToListAsync();

            autoOwners.Sort((x, y) => x.StartDate.CompareTo(y.StartDate));

            bool isValid = false;

            foreach(var checkedAOwner in autoOwners)
            {
                if (checkedAOwner.EndDate == autoOwner.StartDate)
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        public static bool IsValid(AutoOwner autoOwner)
        {
            if (autoOwner.EndDate < autoOwner.StartDate)
            {
                return false;
            }
            return true;
        }
    }
}
