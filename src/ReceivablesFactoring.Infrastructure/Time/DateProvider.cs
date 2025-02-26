using ReceivablesFactoring.Application.Abstractions;

namespace ReceivablesFactoring.Infrastructure.Time;

public sealed class DateProvider : IDateProvider
{
    private const int TimeZoneDiff = -3;

    public DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow.AddHours(TimeZoneDiff));
}
