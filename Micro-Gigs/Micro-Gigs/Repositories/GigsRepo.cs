using Micro_Gigs.Models;
using Microsoft.EntityFrameworkCore;

namespace Micro_Gigs.Repositories
{
    public class GigsRepo
    {
        private MicroGigsContext context;

        public GigsRepo(MicroGigsContext _context)
        {
            context = _context;
        }

        public List<Gigs> GetAll()
        {
            return context.Gigs.Include(g => g.Client).Include(g => g.GigCategory).ToList();
        }

        public void Add(Gigs gig)
        {
            context.Gigs.Add(gig);
            context.SaveChanges();
        }

        public void Update(Gigs gig)
        {
            context.Gigs.Update(gig);
            context.SaveChanges();
        }

        public void Delete(Gigs gig)
        {
                context.Gigs.Remove(gig);
                context.SaveChanges();   
        }

        public List<Gigs> GetOpenGigs()
        {
            return context.Gigs.Where(g => g.Status == "Open").Include(g => g.Client).Include(g => g.GigCategory).ToList();
        }

        public List<Gigs> GetByClientId(int clientId)
        {
            return context.Gigs.Where(g => g.ClientId == clientId).Include(g => g.GigCategory).ToList();
        }

        public Gigs? GetById(int id)
        {
            return context.Gigs.Include(g => g.Client).Include(g => g.GigCategory).Include(g => g.GigApplications).Include(g => g.GigAttachments).FirstOrDefault(g => g.GigId == id);
        }

    }
}
