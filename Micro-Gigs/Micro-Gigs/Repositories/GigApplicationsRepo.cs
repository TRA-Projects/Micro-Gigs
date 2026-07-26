using System.Collections.Generic;
using System.Linq;
using Micro_Gigs.Models;
using Microsoft.EntityFrameworkCore;

namespace Micro_Gigs.Repositories
{
    public class GigApplicationsRepo
    {
        private readonly MicroGigsContext _context;

        public GigApplicationsRepo(MicroGigsContext context)
        {
            _context = context;
        }

        public IEnumerable<GigApplications> GetAll()
        {
            return _context.Applications
                .Include(a => a.Gig)
                .Include(a => a.Freelancer)
                .ToList();
        }

        public GigApplications? GetById(int id)
        {
            return _context.Applications
                .Include(a => a.Gig)
                .Include(a => a.Freelancer)
                .FirstOrDefault(a => a.ApplicationId == id);
        }

        public void Add(GigApplications application)
        {
            _context.Applications.Add(application);
            _context.SaveChanges();
        }

        public void Update(GigApplications application)
        {
            _context.Applications.Update(application);
            _context.SaveChanges();
        }

        public void Delete(GigApplications application)
        {
            _context.Applications.Remove(application);
            _context.SaveChanges();
        }
    }
}