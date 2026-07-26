using System.ComponentModel.DataAnnotations;       // يحتوي على [Key] و [Required] و [Range]
using System.ComponentModel.DataAnnotations.Schema; // يحتوي على [Table] و [DatabaseGenerated] و [ForeignKey]

namespace Micro_Gigs.Models
{
    // تحديد اسم الجدول في قاعدة البيانات
    // سيتم إنشاء جدول باسم GigReviews
    [Table("GigReviews")]
    public class GigReviews
    {
        // =========================================================
        // PRIMARY KEY
        // المفتاح الأساسي للـ Review
        // =========================================================

        [Key]
        // تحديد أن ReviewId هو Primary Key

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // قاعدة البيانات تقوم تلقائياً بتوليد قيمة ReviewId
        // عادةً تكون القيمة 1 ثم 2 ثم 3 ...

        public int ReviewId { get; set; }
        // رقم التقييم
        // System Generated - يتم توليده تلقائياً
        // Primary Key


        // =========================================================
        // ASSIGNMENT ID
        // ربط التقييم بالتكليف GigAssignment
        // =========================================================

        [Required]
        // هذا الحقل إجباري ولا يمكن أن يكون فارغاً

        public int AssignmentId { get; set; }
        // رقم الـ Assignment المرتبط بهذا التقييم
        // Foreign Key
        // System Generated / يأتي من الـ Assignment الموجود


        // =========================================================
        // REVIEWER ID
        // المستخدم الذي قام بكتابة التقييم
        // =========================================================

        [Required]
        // هذا الحقل إجباري

        public int ReviewerId { get; set; }
        // رقم المستخدم الذي كتب التقييم
        // Foreign Key
        // يتم أخذه من المستخدم المسجل دخوله


        // =========================================================
        // RATING
        // درجة التقييم
        // =========================================================

        [Required]
        // التقييم إجباري ولا يمكن أن يكون فارغاً

        [Range(1, 5)]
        // يسمح فقط بقيم من 1 إلى 5
        // مثال:
        // 1 = سيئ جداً
        // 2 = سيئ
        // 3 = متوسط
        // 4 = جيد
        // 5 = ممتاز

        public int Rating { get; set; }
        // قيمة التقييم التي يدخلها المستخدم
        // User Input
        // يجب أن تكون بين 1 و 5


        // =========================================================
        // COMMENT
        // تعليق المستخدم على الـ Gig
        // =========================================================

        public string? Comment { get; set; }
        // تعليق اختياري من المستخدم
        // علامة ? تعني أنه يمكن أن تكون القيمة null
        // User Input
        // يمكن تركه فارغاً


        // =========================================================
        // NAVIGATION PROPERTIES
        // العلاقات بين الجداول
        // =========================================================

        // ---------------------------------------------------------
        // العلاقة مع GigAssignments
        // ---------------------------------------------------------

        [ForeignKey("AssignmentId")]
        // تحديد أن AssignmentId هو الـ Foreign Key
        // الذي يربط GigReviews مع GigAssignments

        public virtual GigAssignments Assignment { get; set; }
        // Navigation Property
        // تسمح بالوصول إلى بيانات الـ Assignment المرتبط بالتقييم
        //
        // مثال:
        // review.Assignment


        // ---------------------------------------------------------
        // العلاقة مع Users
        // ---------------------------------------------------------

        [ForeignKey("ReviewerId")]
        // تحديد أن ReviewerId هو الـ Foreign Key
        // الذي يربط GigReviews مع Users

        public virtual Users Reviewer { get; set; }
        // Navigation Property
        // تسمح بالوصول إلى بيانات المستخدم الذي كتب التقييم
        //
        // مثال:
        // review.Reviewer
    }
}