using IPD.Application.Interfaces.Common;

namespace IPD.Application.Interfaces.Services
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
    }
}
