using System.Collections.Generic;
using System.Linq;
using Micro_Gigs.DTOs;

namespace Micro_Gigs.Services
{
    public class GigApplicationsService : IGigApplicationsService
    {
        // قائمة وهمية للتجربة أو يتم ربطها بـ DbContext الخاص بك هنا
        private static readonly List<GigApplicationDto> _applications = new();

        public IEnumerable<GigApplicationDto> GetAllApplications()
        {
            return _applications;
        }

        public GigApplicationDto GetApplicationById(int id)
        {
            return _applications.FirstOrDefault(a => a.ApplicationId == id);
        }

        public int CreateApplication(CreateGigApplicationDto dto)
        {
            int newId = _applications.Count > 0 ? _applications.Max(a => a.ApplicationId) + 1 : 1;

            var newApp = new GigApplicationDto
            {
                ApplicationId = newId,
                GigId = dto.GigId,
                FreelancerId = dto.FreelancerId,
                ProposalText = dto.ProposalText,
                ProposedPrice = dto.ProposedPrice,
                ApplicationDate = System.DateTime.Now,
                Status = "Pending"
            };

            _applications.Add(newApp);
            return newId; // إرجاع الـ int بشكل صحيح لتجنب خطأ التحويل
        }

        public bool UpdateApplicationStatus(int applicationId, string status)
        {
            var app = _applications.FirstOrDefault(a => a.ApplicationId == applicationId);
            if (app == null)
            {
                return false;
            }

            app.Status = status;
            return true;
        }

        public bool DeleteApplication(int id)
        {
            var app = _applications.FirstOrDefault(a => a.ApplicationId == id);
            if (app == null)
            {
                return false;
            }

            _applications.Remove(app);
            return true;
        }
    }
}