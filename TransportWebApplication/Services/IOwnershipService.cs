using TransportWebApplication.Models;

namespace TransportWebApplication.Services
{
    public interface IOwnershipService
    {
        public abstract void ValidateInput(AutoOwner ownership);
        public abstract Task<AutoOwner?> GetLastOwnership(long autoId, CancellationToken cancellationToken);
        public abstract Task<bool> CreateOwnership(AutoOwner ownership, CancellationToken cancellationToken);
    }
}
