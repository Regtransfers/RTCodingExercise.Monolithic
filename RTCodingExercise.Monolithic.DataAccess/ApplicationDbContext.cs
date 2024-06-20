using Microsoft.EntityFrameworkCore;
using RTCodingExercise.Monolithic.Models;

namespace RTCodingExercise.Monolithic.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Plate> Plates { get; set; }
    }
}