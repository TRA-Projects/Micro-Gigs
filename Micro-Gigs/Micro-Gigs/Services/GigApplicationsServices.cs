using Micro_Gigs.DTOs;
using Micro_Gigs.Models;
using Micro_Gigs.Repositories;

namespace Micro_Gigs.Services
{
    public class GigApplicationsServices
    {
        private readonly GigApplicationsRepo _repo;
        private readonly GigsRepo _gigsRepo;
        private readonly UsersRepo _usersRepo;
        private readonly EmailService _emailService;

        public GigApplicationsServices(GigApplicationsRepo repo, GigsRepo gigsRepo, UsersRepo usersRepo, EmailService emailService)
        {
            _repo = repo;
            _gigsRepo = gigsRepo;
            _usersRepo = usersRepo;
            _emailService = emailService;
        }

        public IEnumerable<GigApplicationDto> GetAllApplications()
        {
            var apps = _repo.GetAll().Where(a => !a.IsDeleted);
            return apps.Select(a => new GigApplicationDto
            {
                ApplicationId = a.ApplicationId,
                GigId = a.GigId,
                GigTitle = a.Gig?.Title ?? string.Empty,        // Fixed issue #3
                FreelancerId = a.FreelancerId,
                FreelancerName = a.Freelancer?.UserName ?? string.Empty, // Fixed issue #3
                ProposalText = a.ProposalText,
                ProposedPrice = a.ProposedPrice,
                ApplicationDate = a.ApplicationDate,
                Status = a.Status
            });
        }

        public GigApplicationDto? GetApplicationById(int id)
        {
            var a = _repo.GetById(id);
            if (a == null || a.IsDeleted) return null;

            return new GigApplicationDto
            {
                ApplicationId = a.ApplicationId,
                GigId = a.GigId,
                GigTitle = a.Gig?.Title ?? string.Empty,        // Fixed issue #3
                FreelancerId = a.FreelancerId,
                FreelancerName = a.Freelancer?.UserName ?? string.Empty, // Fixed issue #3
                ProposalText = a.ProposalText,
                ProposedPrice = a.ProposedPrice,
                ApplicationDate = a.ApplicationDate,
                Status = a.Status
            };
        }

        public AdminGigApplicationDto? GetAdminApplicationById(int id)
        {
            var a = _repo.GetById(id);
            if (a == null) return null;

            return new AdminGigApplicationDto
            {
                ApplicationId = a.ApplicationId,
                GigId = a.GigId,
                GigTitle = a.Gig?.Title ?? string.Empty,
                FreelancerId = a.FreelancerId,
                FreelancerName = a.Freelancer?.UserName ?? string.Empty,
                ProposalText = a.ProposalText,
                ProposedPrice = a.ProposedPrice,
                ApplicationDate = a.ApplicationDate,
                Status = a.Status,
                IsDeleted = a.IsDeleted,
                InternalNotes = a.InternalNotes,
                AdminRating = a.AdminRating
            };
        }

        // Retrieve the freelancer ID from the controller (JWT token), not from the DTO.
        public async Task<(bool Success, string Error, int ApplicationId)> CreateApplication(CreateGigApplicationDto dto, int freelancerId)
        {
            // Validate the gig before saving the application.
            var gig = _gigsRepo.GetById(dto.GigId);
            if (gig == null)
                return (false, $"Gig with ID {dto.GigId} was not found.", 0);

            // Ensure the gig is currently open for applications.
            if (gig.Status != "Open")
                return (false, "Applications cannot be submitted because this gig is not open.", 0);

            var freelancer = _usersRepo.GetById(freelancerId);
            if (freelancer == null)
                return (false, $"User with ID {freelancerId} was not found.", 0);

            // Prevent duplicate applications for the same gig.
            bool alreadyApplied = _repo.GetAll()
                .Any(a => a.GigId == dto.GigId && a.FreelancerId == freelancerId && !a.IsDeleted);
            if (alreadyApplied)
                return (false, "You have already applied for this gig.", 0);

            var application = new GigApplications
            {
                GigId = dto.GigId,
                FreelancerId = freelancerId,   // Retrieved from the JWT token instead of the DTO.
                ProposalText = dto.ProposalText,
                ProposedPrice = dto.ProposedPrice,
                ApplicationDate = DateTime.UtcNow,
                Status = "Pending",
                IsDeleted = false
            };

            _repo.Add(application);

            // Send an email notification to the client (best effort).
            try
            {
                if (gig.Client != null)
                {
                    var subject = $"New application for your gig: {gig.Title}";
                    var body = $@"Hello {gig.Client.UserName},<br/><br/>
Freelancer <b>{freelancer.UserName}</b> applied to your gig '<b>{gig.Title}</b>'.<br/>
Proposal: {application.ProposalText}<br/>
Proposed Price: {application.ProposedPrice:C}<br/><br/>
Regards,<br/>Micro-Gigs Team";

                    await _emailService.SendEmailAsync(gig.Client.Email, subject, body);
                }
            }
            catch
            {
                // Do not interrupt the application process if sending the email fails.
            }

            return (true, string.Empty, application.ApplicationId);
        }

        public bool UpdateApplicationStatus(int id, string status)
        {
            var application = _repo.GetById(id);
            if (application == null || application.IsDeleted) return false;

            application.Status = status;
            _repo.Update(application);
            return true;
        }

        public bool UpdateAdminNotesAndRating(int id, string? internalNotes, int? adminRating)
        {
            var application = _repo.GetById(id);
            if (application == null) return false;

            application.InternalNotes = internalNotes;
            application.AdminRating = adminRating;
            _repo.Update(application);
            return true;
        }

        public bool DeleteApplication(int id)
        {
            var application = _repo.GetById(id);
            if (application == null || application.IsDeleted) return false;

            application.IsDeleted = true;
            _repo.Update(application);
            return true;
        }
    }
}