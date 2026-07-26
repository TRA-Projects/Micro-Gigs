using System;
using System.Collections.Generic;
using System.Linq;
using Micro_Gigs.DTOs;
using Micro_Gigs.Models;
using Micro_Gigs.Repositories;

namespace Micro_Gigs.Services
{
    /// <summary>
    /// واجهة (Interface) تحدد خدمات ومنطق العمل (Business Logic) الخاص بطلبات التقديم على الخدمات.
    /// </summary>
    public interface IGigApplicationsService
    {
        // استرجاع كافة الطلبات وتحويلها إلى DTOs لعرضها
        IEnumerable<GigApplicationDto> GetAllApplications();

        // استرجاع طلب معين بواسطة معرّفه وتحويله إلى DTO
        GigApplicationDto? GetApplicationById(int id);

        // إنشاء طلب تقديم جديد باستخدام بيانات الـ DTO وإرجاع الـ ID الخاص به
        int CreateApplication(CreateGigApplicationDto dto);

        // تحديث حالة الطلب (مثل قبول أو رفض) وإرجاع قيمة نجاح العملية
        bool UpdateApplicationStatus(int id, string status);

        // حذف طلب تقديم بناءً على معرّفه وإرجاع قيمة نجاح العملية
        bool DeleteApplication(int id);
    }

    /// <summary>
    /// التطبيق الفعلي (Implementation) لخدمات طلبات التقديم والمسؤول عن ربط الـ Repository بالـ DTOs.
    /// </summary>
    public class GigApplicationsServices : IGigApplicationsService
    {
        private readonly IGigApplicationsRepo _repo;

        // حقن مستودع البيانات (Repository) عبر المُنشئ (Constructor Injection)
        public GigApplicationsServices(IGigApplicationsRepo repo)
        {
            _repo = repo;
        }

        // جلب جميع الطلبات وتحويل كل نموذج قاعدة بيانات (Entity) إلى DTO لعرضه في الواجهة
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

        // جلب طلب واحد بالمعرّف وتحويله لـ DTO، أو إرجاع null إن لم يتم العثور عليه
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

        // إنشاء كائن بيانات جديد، تعيين تاريخ اليوم والحالة الافتراضية "Pending"، ثم حفظه وإرجاع معرّفه
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

            // إرجاع المعرف (ID) الخاص بالطلب الجديد بعد توليده من قاعدة البيانات
            return application.ApplicationId;
        }

        // البحث عن الطلب وتحديث حالته فقط، ثم حفظ التغييرات وإرجاع True إذا تم بنجاح
        public bool UpdateApplicationStatus(int id, string status)
        {
            var application = _repo.GetById(id);
            if (application == null) return false;

            application.Status = status;
            _repo.Update(application);
            return true;
        }

        // البحث عن الطلب وحذفه من قاعدة البيانات وإرجاع True إذا تم بنجاح
        public bool DeleteApplication(int id)
        {
            var application = _repo.GetById(id);
            if (application == null) return false;

            _repo.Delete(application);
            return true;
        }
    }
}