using System;
using System.ComponentModel.DataAnnotations;

namespace Micro_Gigs.DTOs
{
    /// <summary>
    /// ممثل لبيانات طلب التقديم على خدمة (Gig Application) عند إرسالها للعميل أو عرضها.
    /// </summary>
    public class GigApplicationDto
    {
        // المعرف الفريد لطلب التقديم
        public int ApplicationId { get; set; }

        // معرف الخدمة المراد التقديم عليها
        public int GigId { get; set; }

        // عنوان الخدمة (قيمة افتراضية لمنع القيم الفارغة)
        public string GigTitle { get; set; } = string.Empty;

        // معرف المستقل (Freelancer) المتقدم للخدمة
        public int FreelancerId { get; set; }

        // اسم المستقل المتقدم
        public string FreelancerName { get; set; } = string.Empty;

        // نص العرض أو الرسالة الموجهة لصاحب الخدمة
        public string ProposalText { get; set; } = string.Empty;

        // السعر المقترح من قبل المستقل لإنجاز الخدمة
        public decimal ProposedPrice { get; set; }

        // تاريخ وقت تقديم الطلب
        public DateTime ApplicationDate { get; set; }

        // حالة الطلب (مثال: معلق، مقبول، مرفوض)
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// نموذج نقل البيانات الخاص بإنشاء طلب تقديم جديد (يحتوي على قواعد التحقق من الصحة Validation).
    /// </summary>
    public class CreateGigApplicationDto
    {
        [Required(ErrorMessage = "GigId is required")]
        public int GigId { get; set; }

        [Required(ErrorMessage = "FreelancerId is required")]
        public int FreelancerId { get; set; }

        [Required(ErrorMessage = "ProposalText is required")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Proposal text must be between 10 and 2000 characters.")]
        public string ProposalText { get; set; } = string.Empty;

        [Required(ErrorMessage = "ProposedPrice is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Please enter a valid proposed price between 0.01 and 999999.99")]
        public decimal ProposedPrice { get; set; }
    }
}