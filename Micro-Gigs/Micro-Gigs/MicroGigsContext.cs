using Micro_Gigs.Models;
using Microsoft.EntityFrameworkCore;

namespace Micro_Gigs
{
    public class MicroGigsContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Gigs> Gigs { get; set; }
        public DbSet<GigCategories> Categories { get; set; }
        public DbSet<GigApplications> Applications { get; set; }
        public DbSet<GigAssignments> Assignments { get; set; }
        public DbSet<GigReviews> Reviews { get; set; }
        public DbSet<GigAttachments> Attachments { get; set; }

        public MicroGigsContext(DbContextOptions<MicroGigsContext> options) : base(options)
        {
        }

    }
}
