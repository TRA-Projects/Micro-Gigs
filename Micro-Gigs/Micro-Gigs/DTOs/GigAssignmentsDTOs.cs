using System.ComponentModel.DataAnnotations;

namespace Micro_Gigs.DTOs
{
    // المدخلات: البيانات المطلوبة عند تكليف مستقل بمهمة
    // DTO خاص بإضافة تعيين (Assignment) جديد
    public class GigAssignmentsInputDTOs
    {
        // رقم الـ Gig مطلوب
        [Required(ErrorMessage = "GigId is required")]
        public int GigId { get; set; }

        // رقم المستقل (Freelancer) مطلوب
        [Required(ErrorMessage = "FreelancerId is required")]
        public int FreelancerId { get; set; }

        // السعر المتفق عليه مطلوب ويجب أن يكون أكبر من صفر
        [Required(ErrorMessage = "AgreedPrice is required")]
        [Range(0.01, 999999.99)]
        public decimal AgreedPrice { get; set; }
    }




    // المخرجات: البيانات التي تظهر عند استعراض تفاصيل التكليف
    // DTO خاص بإرجاع بيانات التعيين للمستخدم

    public class GigAssignmentsOutputDTOs
    {
        // رقم التعيين
        public int AssignmentId { get; set; }

        // رقم الخدمة (Gig)
        public int GigId { get; set; }

        // عنوان الخدمة
        public string GigTitle { get; set; } = string.Empty;

        // رقم المستقل
        public int FreelancerId { get; set; }

        // اسم المستقل
        public string FreelancerName { get; set; } = string.Empty;

        // السعر المتفق عليه
        public decimal AgreedPrice { get; set; }

        // تاريخ تعيين المستقل للمهمة
        public DateTime AssignedDate { get; set; }

        // تاريخ إكمال المهمة (قد يكون فارغًا إذا لم تكتمل)
        public DateTime? CompletionDate { get; set; }

        // حالة المهمة (Pending أو In Progress أو Completed)
        public string Status { get; set; } = string.Empty;

    }
}
