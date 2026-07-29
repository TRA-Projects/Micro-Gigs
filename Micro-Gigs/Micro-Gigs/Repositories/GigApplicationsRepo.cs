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

        // Retrieve all applications submitted for a specific gig.
        public IEnumerable<GigApplications> GetByGigId(int gigId)
        {
            return _context.Applications
                .Include(a => a.Freelancer)
                .Where(a => a.GigId == gigId)
                .ToList();
        }

        // Retrieve all applications submitted by a specific freelancer.
        public IEnumerable<GigApplications> GetByFreelancerId(int freelancerId)
        {
            return _context.Applications
                .Include(a => a.Gig)
                .Where(a => a.FreelancerId == freelancerId)
                .ToList();
        }

        public void Add(GigApplications application)
        {
            // Check for duplicate applications before adding a new one.
            bool alreadyApplied = _context.Applications
                .Any(a => a.GigId == application.GigId && a.FreelancerId == application.FreelancerId);

            if (alreadyApplied)
                throw new InvalidOperationException("The freelancer has already applied for this gig.");

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