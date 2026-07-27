using Micro_Gigs.DTOs;                      // استيراد ملفات DTO
using Micro_Gigs.Services;                  // استيراد طبقة الخدمات (Service)
using Microsoft.AspNetCore.Authorization;   // لاستخدام Authorize
using Microsoft.AspNetCore.Mvc;             // لاستخدام Controller و IActionResult
using System.Security.Claims;               // للحصول على بيانات المستخدم بعد تسجيل الدخول

namespace Micro_Gigs.Controllers
{
    [ApiController]

    // مسار الـ API
    [Route("api/[controller]")]
    public class GigAssignmentsController : ControllerBase
    {
        // Service المسؤول عن تنفيذ منطق العمل
        private readonly GigAssignmentsServices _assignmentsService;

        // Constructor
        public GigAssignmentsController(GigAssignmentsServices assignmentsService)
        {
            _assignmentsService = assignmentsService;
        }

        // ===================================================
        // جلب جميع التعيينات
        // ===================================================
        [HttpGet("GetAll")]
        [Authorize]
        public IActionResult GetAll()
        {
            var assignments = _assignmentsService.GetAllAssignments();
            return Ok(assignments);
        }

        // ===================================================
        // جلب تعيين بواسطة رقمه
        // Data Binding Attribute: FromQuery
        // ===================================================
        [HttpGet("GetById")]
        [Authorize]
        public IActionResult GetById([FromQuery] int id)
        {
            var assignment = _assignmentsService.GetAssignmentById(id);

            if (assignment == null)
                return NotFound();

            return Ok(assignment);
        }

        // ===================================================
        // جلب جميع تعيينات مستقل
        // Data Binding Attribute: FromQuery
        // ===================================================
        [HttpGet("GetByFreelancer")]
        [Authorize]
        public IActionResult GetByFreelancer([FromQuery] int freelancerId)
        {
            var assignments = _assignmentsService.GetAssignmentsByFreelancer(freelancerId);

            return Ok(assignments);
        }

        // ===================================================
        // إنشاء Assignment جديد
        // Data Binding Attribute: FromBody
        // ===================================================
        [HttpPost("Create")]
        [Authorize(Roles = "Client")]
        public IActionResult Create([FromBody] GigAssignmentsInputDTOs dto)
        {
            var assignment = _assignmentsService.CreateAssignment(dto);

            if (assignment == null)
                return BadRequest(new { message = "Cannot create assignment" });

            return Ok(assignment);
        }

        // ===================================================
        // تسليم المهمة
        // Data Binding Attribute: FromQuery
        // ===================================================
        [HttpPut("Submit")]
        [Authorize(Roles = "Freelancer")]
        public IActionResult Submit([FromQuery] int assignmentId)
        {
            int freelancerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var success = _assignmentsService.SubmitAssignment(assignmentId, freelancerId);

            if (!success)
                return BadRequest(new { message = "Cannot submit assignment" });

            return NoContent();
        }

        // ===================================================
        // موافقة العميل على المهمة
        // Data Binding Attribute: FromQuery
        // ===================================================
        [HttpPut("Approve")]
        [Authorize(Roles = "Client")]
        public IActionResult Approve([FromQuery] int assignmentId)
        {
            int clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var success = _assignmentsService.ApproveAssignment(assignmentId, clientId);

            if (!success)
                return BadRequest(new { message = "Cannot approve assignment" });

            return NoContent();
        }
    }
}