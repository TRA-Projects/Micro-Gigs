using System;
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

        public GigApplicationsServices(GigApplicationsRepo repo)
        {
            _repo = repo;
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

        public int CreateApplication(CreateGigApplicationDto dto)
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