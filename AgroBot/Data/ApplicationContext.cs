using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AgroBot.Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<PipelineContext> Pipelines { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Crop> Crops { get; set; }
        
        public DbSet<Journal> Journals { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PipelineContext>()
                .Property(p => p.Type)
                .HasConversion(v => v.ToString(), v => (PipelineType)Enum.Parse(typeof(PipelineType), v));

            builder.Entity<PipelineContext>()
                .Property(p => p.CurrentStep)
                .HasConversion(v => v.ToString(), v => (PipelineStepType)Enum.Parse(typeof(PipelineStepType), v));

            builder.Entity<Crop>()
                .Property(c => c.Status)
                .HasConversion(v => v.ToString(), v => (CropStatus)Enum.Parse(typeof(CropStatus), v));
        }
    }
}
