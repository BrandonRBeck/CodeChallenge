using CodeChallenge.Persistence.DBModel;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Persistence;

public class SchedulingContext : DbContext
{
    public SchedulingContext(DbContextOptions<SchedulingContext> options) : base(options) { }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Appointment> Users { get; set; }
}
