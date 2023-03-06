using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading;
using TransportWebApplication.Exceptions;
using TransportWebApplication.Models;

namespace TransportWebApplication.Services
{
    public class OwnershipService: IOwnershipService
    {
        private readonly TransportContext Context;

        public OwnershipService(TransportContext context)
        {
            Context = context;
        }

        public void ValidateInput(AutoOwner ownership)
        {
            ArgumentNullException.ThrowIfNull(ownership, nameof(ownership));

            if (ownership.EndDate is not null)
            {
                if (!ownership.IsFinite)
                {
                    throw new OwnershipException("End date only for finite ownership");
                }

                if (ownership.StartDate > ownership.EndDate)
                {
                    throw new OwnershipException("End date can't be less than a start date");
                }
            }
        }
        public async Task<AutoOwner?> GetLastOwnership(long autoId, CancellationToken cancellationToken)
        {
            var lastAutoOwner = await Context.AutoOwners
                .Where(ao => ao.AutoId == autoId)
                .OrderByDescending(ao => ao.StartDate)
                .FirstOrDefaultAsync(cancellationToken);

            return lastAutoOwner;
        }
        public async Task<bool> CreateOwnership(AutoOwner ownership, CancellationToken cancellationToken)
        {
            ValidateInput(ownership);

            var lastOwnerShip = await GetLastOwnership(ownership.AutoId, cancellationToken);

            if(lastOwnerShip is not null) 
            {
                if (lastOwnerShip.IsFinite)
                {
                    throw new OwnershipException("Last ownership is finite, can't add more");
                }

                if (lastOwnerShip.StartDate > ownership.StartDate)
                {
                    throw new OwnershipException("Last ownership start date is less than new one");
                }

            }

            await Context.AddAsync(ownership, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
