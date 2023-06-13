using IPD.Application.Interfaces.Services;

namespace IPD.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}
