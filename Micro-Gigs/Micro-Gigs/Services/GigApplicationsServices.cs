using System;
using System.Collections.Generic;
using System.Linq;
using Micro_Gigs.DTOs;
using Micro_Gigs.Models;
using Micro_Gigs.Repositories;

namespace Micro_Gigs.Services
{
    public interface IGigApplicationsService
    {
        IEnumerable<GigApplicationDto> GetAllApplications();
        GigApplicationDto? GetApplicationById(int id);

        // تأكد أن نوع الإرجاع هنا هو int (أو GigApplicationDto حسب رغبتك، ولكن الأفضل int لإرجاع الـ ID عند الإنشاء كما هو معمول به في الـ Controllers المشابهة)
        int CreateApplication(CreateGigApplicationDto dto);

        bool UpdateApplicationStatus(int id, string status);
        bool DeleteApplication(int id);
    }

    public class GigApplicationsServices : IGigApplicationsService
    {
        private readonly IGigApplicationsRepo _repo;

        public GigApplicationsServices(IGigApplicationsRepo repo)
        {
            _repo = repo;
        }

        public IEnumerable<GigApplicationDto> GetAllApplications()
        {
            var apps = _repo.GetAll();
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

        public GigApplicationDto? GetApplicationById(int id)
        {
            var a = _repo.GetById(id);
            if (a == null) return null;

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

        // تم تعديل نوع الإرجاع هنا إلى int ليتطابق مع الـ Interface ويُرجع الـ ID مباشرة
        public int CreateApplication(CreateGigApplicationDto dto)
        {
            var application = new GigApplications
            {
                GigId = dto.GigId,
                FreelancerId = dto.FreelancerId,
                ProposalText = dto.ProposalText,
                ProposedPrice = dto.ProposedPrice,
                ApplicationDate = DateTime.Now,
                Status = "Pending"
            };

            _repo.Add(application);

            // إرجاع المعرف (ID) الخاص بالطلب الجديد
            return application.ApplicationId;
        }

        public bool UpdateApplicationStatus(int id, string status)
        {
            var application = _repo.GetById(id);
            if (application == null) return false;

            application.Status = status;
            _repo.Update(application);
            return true;
        }

        public bool DeleteApplication(int id)
        {
            var application = _repo.GetById(id);
            if (application == null) return false;

            _repo.Delete(application);
            return true;
        }
    }
}