using Domain.Entities;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Infrastructure.AppDbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<UserRefreshToken> RefreshTokens { get; set; }
        public DbSet<InvalidToken> InvalidTokens { get; set; }
        public DbSet<AttempExam> AttempExams { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<UserFavourite> UserFavourites { get; set; }
        public DbSet<Collection> Collections { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Exam>()
               .HasMany(e => e.Tags)
               .WithMany(t => t.Exams)
               .UsingEntity(j => j.ToTable("ExamTags"));

            builder.Entity<Collection>()
                .HasMany(c => c.Exams)
                .WithMany()
                .UsingEntity(j => j.ToTable("ExamCollections"))
                .HasMany(c => c.Tags)
                .WithMany()
                .UsingEntity(j => j.ToTable("TagCollections"));

        }
    }
}
