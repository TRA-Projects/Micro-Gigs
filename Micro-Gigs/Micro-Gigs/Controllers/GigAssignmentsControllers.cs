using Micro_Gigs.DTOs;              // استيراد ملفات DTO
using Micro_Gigs.Services;          // استيراد طبقة الخدمات (Service)
using Microsoft.AspNetCore.Authorization; // لاستخدام Authorize
using Microsoft.AspNetCore.Mvc;     // لاستخدام Controller و IActionResult
using System.Security.Claims;       // للحصول على بيانات المستخدم بعد تسجيل الدخول

namespace Micro_Gigs.Controllers
{
    // تعريف هذا الكلاس بأنه API Controller
    [ApiController]

    // مسار الـ API
    // api/GigAssignments
    [Route("api/[controller]")]
    public class GigAssignmentsController : ControllerBase
    {
        // Service المسؤول عن تنفيذ منطق العمل
        private GigAssignmentsServices _assignmentsService;

        // Constructor لاستقبال الـ Service عن طريق Dependency Injection
        public GigAssignmentsController(GigAssignmentsServices assignmentsService)
        {
            _assignmentsService = assignmentsService;
        }

        // ==========================================
        // جلب جميع التعيينات
        // Get All Assignments
        // ==========================================
        [HttpGet("GetAll")]

        // يسمح فقط للمستخدم الذي سجل الدخول
        [Authorize]
        public IActionResult GetAll()
        {
            // استدعاء الخدمة لجلب جميع البيانات
            var assignments = _assignmentsService.GetAllAssignments();

            // إرجاع البيانات مع كود 200
            return Ok(assignments);
        }

        // ==========================================
        // جلب تعيين بواسطة رقمه
        // Get Assignment By Id
        // ==========================================
        [HttpGet("GetById")]

        [Authorize]
        public IActionResult GetById([FromQuery] int id)
        {
            // البحث عن التعيين
            var assignment = _assignmentsService.GetAssignmentById(id);

            // إذا لم يتم العثور عليه
            if (assignment == null)
                return NotFound();

            // إذا وجد يتم إرجاعه
            return Ok(assignment);
        }

        // ==========================================
        // جلب جميع مهام مستقل معين
        // Get Assignments By Freelancer
        // ==========================================
        [HttpGet("GetByFreelancer")]

        [Authorize]
        public IActionResult GetByFreelancer([FromQuery] int freelancerId)
        {
            // جلب جميع التعيينات الخاصة بالمستقل
            var assignments = _assignmentsService.GetAssignmentsByFreelancer(freelancerId);

            return Ok(assignments);
        }

        // ==========================================
        // إنشاء تعيين جديد
        // Create Assignment
        // ==========================================
        [HttpPost("Create")]

        // يسمح فقط للعميل بإنشاء Assignment
        [Authorize(Roles = "Client")]
        public IActionResult Create([FromBody] GigAssignmentsInputDTOs dto)
        {
            // إرسال البيانات إلى Service
            var assignment = _assignmentsService.CreateAssignment(dto);

            // إذا فشلت عملية الإنشاء
            if (assignment == null)
                return BadRequest(new { message = "Cannot create assignment" });

            // إذا نجحت
            return Ok(assignment);
        }

        // ==========================================
        // المستقل يقوم بتسليم المهمة
        // Submit Assignment
        // ==========================================
        [HttpPut("Submit")]

        // يسمح فقط للمستقل
        [Authorize(Roles = "Freelancer")]
        public IActionResult Submit([FromQuery] int assignmentId)
        {
            // استخراج رقم المستخدم من الـ Token
            int freelancerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            // إرسال الطلب إلى Service
            var success = _assignmentsService.SubmitAssignment(assignmentId, freelancerId);

            // إذا فشل
            if (!success)
                return BadRequest(new { message = "Cannot submit assignment" });

            // نجاح العملية
            return NoContent();
        }

        // ==========================================
        // العميل يوافق على المهمة
        // Approve Assignment
        // ==========================================
        [HttpPut("Approve")]

        // يسمح فقط للعميل
        [Authorize(Roles = "Client")]
        public IActionResult Approve([FromQuery] int assignmentId)
        {
            // استخراج رقم العميل من الـ Token
            int clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            // إرسال الطلب إلى Service
            var success = _assignmentsService.ApproveAssignment(assignmentId, clientId);

            // إذا فشلت العملية
            if (!success)
                return BadRequest(new { message = "Cannot approve assignment" });

            // نجاح العملية
            return NoContent();
        }
    }
}