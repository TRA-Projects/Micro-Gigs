using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
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

        // جلب جميع الطلبات مع استبعاد المحذوفة منطقياً يدوياً
        public IEnumerable<GigApplicationDto> GetAllApplications()
        {
            var apps = _repo.GetAll().Where(a => !a.IsDeleted);
            return apps.Select(a => new GigApplicationDto
            {
                ApplicationId = a.ApplicationId,
                GigId = a.GigId,
                FreelancerId = a.FreelancerId,
                ProposalText = a.ProposalText,
                ProposedPrice = a.ProposedPrice,
                ApplicationDate = a.ApplicationDate,
                Status = a.Status
            });
        }

        // جلب طلب واحد مع التحقق أنه غير محذوف
        public GigApplicationDto? GetApplicationById(int id)
        {
            var a = _repo.GetById(id);
            if (a == null || a.IsDeleted) return null;

            return new GigApplicationDto
            {
                ApplicationId = a.ApplicationId,
                GigId = a.GigId,
                FreelancerId = a.FreelancerId,
                ProposalText = a.ProposalText,
                ProposedPrice = a.ProposedPrice,
                ApplicationDate = a.ApplicationDate,
                Status = a.Status
            };
        }

        // جلب تفاصيل المشرف (لرؤية الطلبات حتى المحذوفة للمراجعة)
        public AdminGigApplicationDto? GetAdminApplicationById(int id)
        {
            var a = _repo.GetById(id);
            if (a == null) return null;

            return new AdminGigApplicationDto
            {
                ApplicationId = a.ApplicationId,
                GigId = a.GigId,
                FreelancerId = a.FreelancerId,
                ProposalText = a.ProposalText,
                ProposedPrice = a.ProposedPrice,
                ApplicationDate = a.ApplicationDate,
                Status = a.Status,
                IsDeleted = a.IsDeleted,
                InternalNotes = a.InternalNotes,
                AdminRating = a.AdminRating
            };
        }

        public async Task<int> CreateApplication(CreateGigApplicationDto dto)
        {
            var application = new GigApplications
            {
                GigId = dto.GigId,
                FreelancerId = dto.FreelancerId,
                ProposalText = dto.ProposalText,
                ProposedPrice = dto.ProposedPrice,
                ApplicationDate = DateTime.Now,
                Status = "Pending",
                IsDeleted = false
            };

            _repo.Add(application);

            // After creating application, notify the gig's client by email with freelancer and gig details.
            try
            {
                var gig = _gigsRepo.GetById(dto.GigId);
                var freelancer = _usersRepo.GetById(dto.FreelancerId);

                if (gig != null && gig.Client != null && freelancer != null)
                {
                    var clientEmail = gig.Client.Email;
                    var subject = $"New application for your gig: {gig.Title}";
                    var body = $@"Hello {gig.Client.UserName},<br/><br/>
A new freelancer has applied to your gig '<b>{gig.Title}</b>'.<br/><br/>
Freelancer details:<br/>
Name: {freelancer.UserName}<br/>
Email: {freelancer.Email}<br/>
Proposal: {application.ProposalText}<br/>
Proposed Price: {application.ProposedPrice:C}<br/>
Application Date: {application.ApplicationDate}<br/><br/>
You can review the application in your dashboard.<br/><br/>
Regards,<br/>Micro-Gigs Team";

                    // send email (best-effort)
                    await _emailService.SendEmailAsync(clientEmail, subject, body);
                }
            }
            catch
            {
                // swallow exceptions from email sending to not block application creation
            }
            return application.ApplicationId;
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