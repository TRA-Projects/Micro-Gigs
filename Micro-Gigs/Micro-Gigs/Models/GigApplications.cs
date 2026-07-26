using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_Gigs.Models
{
    /// <summary>
    /// نموذج (Model) يمثل جدول طلبات التقديم على الخدمات (Gig Applications) في قاعدة البيانات.
    /// </summary>
    public class GigApplications
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // المعرف الفريد لطلب التقديم (يتم توليده تلقائياً كـ Primary Key)
        public int ApplicationId { get; set; }

        [Required]
        // معرف الخدمة المراد التقديم عليها (حقل إلزامي ومرتبط بجدول الخدمات)
        public int GigId { get; set; }

        [Required]
        // معرف المستقل المتقدم للخدمة (حقل إلزامي ومرتبط بجدول المستخدمين)
        public int FreelancerId { get; set; }

        [MaxLength(2000)]
        // نص العرض أو المقترح المقدم من المستقل (بحد أقصى 2000 حرف)
        public string ProposalText { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        // السعر المقترح لإنجاز الخدمة
        public decimal ProposedPrice { get; set; }

        // تاريخ وقت إنشاء طلب التقديم (قيمة افتراضية تلقائية بوقت النظام الحالي)
        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        // حالة الطلب الحالية (مثل Pending، Accepted، Rejected)
        public string Status { get; set; } = "Pending";

        // ==========================================
        // الحقول الإدارية والخاصة بالتحكم الداخلي للنظام
        // ==========================================

        // مؤشر لمعرفة ما إذا كان الطلب محذوفاً أم لا (Soft Delete)، وقيمته الافتراضية false
        public bool IsDeleted { get; set; } = false;

        [MaxLength(1000)]
        // ملاحظات داخلية يكتبها المشرفون أو الإدارة حول الطلب (لا تظهر للمستقل)
        public string? InternalNotes { get; set; }

        // تقييم الإدارة أو صاحب العمل للطلب أو للمستقل بناءً على هذا التقديم
        public int? AdminRating { get; set; }

        // ==========================================
        // خصائص التنقل (Navigation Properties)
        // ==========================================

        [ForeignKey("GigId")]
        // كائن الخدمة المرتبط بهذا الطلب
        public virtual Gigs Gig { get; set; } = null;

        [ForeignKey("FreelancerId")]
        // كائن المستخدم (المستقل) المتقدم للطلب
        public virtual Users Freelancer { get; set; } = null;
    }
}