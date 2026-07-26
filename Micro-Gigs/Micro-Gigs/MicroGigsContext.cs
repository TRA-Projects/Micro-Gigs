using Micro_Gigs.Models;
using Microsoft.EntityFrameworkCore;

namespace Micro_Gigs
{
    public class MicroGigsContext : DbContext
    {

        // constructor يستقبل الاعدادات من ملف البروجرام 
        // Configure the database connection string in the appsettings.json file and use it here
        public MicroGigsContext(DbContextOptions<MicroGigsContext> options) : base(options)
        {
        }


        public DbSet<Users> Users { get; set; }
        public DbSet<Gigs> Gigs { get; set; }
        public DbSet<GigCategories> Categories { get; set; }
        public DbSet<GigApplications> Applications { get; set; }
        public DbSet<GigAssignments> Assignments { get; set; }
        public DbSet<GigReviews> Reviews { get; set; }
        public DbSet<GigAttachments> Attachments { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=localhost;Database=Micro_GigsDB;Trusted_Connection=True;TrustServerCertificate=True;"
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ─── One-to-One: Gigs ↔ GigAssignments ───
            modelBuilder.Entity<GigAssignments>()
                .HasOne(a => a.Gig)
                .WithOne(g => g.GigAssignment)
                .HasForeignKey<GigAssignments>(a => a.GigId)
                .OnDelete(DeleteBehavior.Cascade);

            // ─── NoAction للعلاقات المتعددة (تمنع cycles) ───

            // GigApplications → Users (Freelancer)
            modelBuilder.Entity<GigApplications>()
                .HasOne(a => a.Freelancer)
                .WithMany(u => u.GigApplications)
                .HasForeignKey(a => a.FreelancerId)
                .OnDelete(DeleteBehavior.NoAction);

            // GigAssignments → Users (Freelancer)
            modelBuilder.Entity<GigAssignments>()
                .HasOne(a => a.Freelancer)
                .WithMany(u => u.FreelancerAssignments)
                .HasForeignKey(a => a.FreelancerId)
                .OnDelete(DeleteBehavior.NoAction);

            // GigReviews → Users (Reviewer)
            modelBuilder.Entity<GigReviews>()
                .HasOne(r => r.Client)
                .WithMany(u => u.ReviewsGiven)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            // GigAttachments → Users (UploadedBy)
            modelBuilder.Entity<GigAttachments>()
                .HasOne(a => a.UploadedByUser)
                .WithMany(u => u.Uploads)
                .HasForeignKey(a => a.UploadedBy)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
    




