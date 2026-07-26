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

        // استرجاع تفاصيل الطلب الكاملة الخاصة بالمشرفين (تشمل الحقول الإدارية)
        AdminGigApplicationDto? GetAdminApplicationById(int id);

        // إنشاء طلب تقديم جديد باستخدام بيانات الـ DTO وإرجاع الـ ID الخاص به
        int CreateApplication(CreateGigApplicationDto dto);

        // تحديث حالة الطلب (مثل قبول أو رفض) وإرجاع قيمة نجاح العملية
        bool UpdateApplicationStatus(int id, string status);

        // تحديث الملاحظات الإدارية والتقييم للطلب (خاص بالمشرفين)
        bool UpdateAdminNotesAndRating(int id, string? internalNotes, int? adminRating);

        // حذف طلب تقديم (تعديل مؤشر الحذف الناعم IsDeleted إلى True) وإرجاع قيمة نجاح العملية
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

        // جلب طلب واحد بالمعرّف وتحويله لـ DTO العادي، أو إرجاع null إن لم يتم العثور عليه
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

        // جلب تفاصيل الطلب للمشرف (تتضمن الحقول الإدارية الجديدة: IsDeleted, InternalNotes, AdminRating)
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

        // إنشاء كائن بيانات جديد، تعيين التاريخ، الحالة الافتراضية، وقيم الحقول الإدارية الافتراضية
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
                IsDeleted = false // القيمة الافتراضية للطلب الجديد
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

        // تحديث الملاحظات الإدارية والتقييم (خاص بلوحة تحكم المشرفين)
        public bool UpdateAdminNotesAndRating(int id, string? internalNotes, int? adminRating)
        {
            var application = _repo.GetById(id);
            if (application == null) return false;

            application.InternalNotes = internalNotes;
            application.AdminRating = adminRating;
            _repo.Update(application);
            return true;
        }

        // تنفيذ الحذف الناعم (Soft Delete) بتغيير IsDeleted إلى True بدلاً من الحذف النهائي
        public bool DeleteApplication(int id)
        {
            var application = _repo.GetById(id);
            if (application == null) return false;

            application.IsDeleted = true;
            _repo.Update(application);
            return true;
        }
    }
}