using CleanArchitectureTemplate.Application.Interfaces;
using CleanArchitectureTemplate.Domain.Interfaces;

namespace CleanArchitectureTemplate.Infrastructure.DateTime;

public class DateTimeService : IDateTime
{
    public System.DateTime Now { get; set; } = System.DateTime.Now;
}