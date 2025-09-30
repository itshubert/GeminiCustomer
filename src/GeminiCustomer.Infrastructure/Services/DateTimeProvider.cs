using GeminiCustomer.Application.Common.Interfaces.Services;

namespace GeminiCustomer.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}