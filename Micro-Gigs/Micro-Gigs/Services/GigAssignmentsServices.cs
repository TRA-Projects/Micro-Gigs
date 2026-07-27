using Micro_Gigs.DTOs;
using Micro_Gigs.Models;
using Micro_Gigs.Repositories;

namespace Micro_Gigs.Services
{
    // Service Class | كلاس الخدمة الذي يحتوي على منطق العمل (Business Logic)
    public class GigAssignmentsServices
    {
        // Repository الخاص بالتعيينات | Repository for Assignments
        private GigAssignmentsRepo _assignmentsRepo;

        // Repository الخاص بالخدمات | Repository for Gigs
        private GigsRepo _gigsRepo;

        // Constructor | استقبال الـ Repositories عن طريق Dependency Injection
        public GigAssignmentsServices(GigAssignmentsRepo assignmentsRepo, GigsRepo gigsRepo)
        {
            _assignmentsRepo = assignmentsRepo;
            _gigsRepo = gigsRepo;
        }
        // جلب جميع التعيينات | Get all assignments
        public List<GigAssignmentsOutputDTOs> GetAllAssignments()
        {
            var assignments = _assignmentsRepo.GetAll();

            // تحويل البيانات من Entity إلى DTO
            return assignments.Select(MapToDto).ToList();
        }
        // جلب تعيين بواسطة رقمه | Get assignment by ID
        public GigAssignmentsOutputDTOs? GetAssignmentById(int id)
        {
            var assignment = _assignmentsRepo.GetById(id);

            // إذا لم يتم العثور عليه يرجع null
            if (assignment == null)
                return null;

            return MapToDto(assignment);
        }

        // جلب التعيين باستخدام رقم الخدمة | Get assignment by Gig ID
        public GigAssignmentsOutputDTOs? GetAssignmentByGigId(int gigId)
        {
            var assignment = _assignmentsRepo.GetByGigId(gigId);

            if (assignment == null)
                return null;

            return MapToDto(assignment);
        }
        // جلب التعيين باستخدام رقم الخدمة | Get assignment by Gig ID
        public GigAssignmentsOutputDTOs? GigAssignmentsInputDTOs(int gigId)
        {
            var assignment = _assignmentsRepo.GetByGigId(gigId);

            if (assignment == null)
                return null;

            return MapToDto(assignment);
        }

        // جلب جميع تعيينات مستقل معين | Get assignments by Freelancer
        public List<GigAssignmentsOutputDTOs> GetAssignmentsByFreelancer(int freelancerId)
        {
            var assignments = _assignmentsRepo.GetByFreelancerId(freelancerId);

            return assignments.Select(MapToDto).ToList();
        }

        // إنشاء تعيين جديد | Create new Assignment
        public GigAssignmentsOutputDTOs? CreateAssignment(GigAssignmentsInputDTOs dto)
        {
            // البحث عن الخدمة المطلوبة
            var gig = _gigsRepo.GetById(dto.GigId);

            // التأكد أن الخدمة موجودة وحالتها Assigned
            if (gig == null || gig.Status != "Open")
                return null;

            // إنشاء كائن جديد
            var assignment = new GigAssignments
            {
                GigId = dto.GigId,
                FreelancerId = dto.FreelancerId,
                AgreedPrice = dto.AgreedPrice,
                AssignedDate = DateTime.UtcNow, // تاريخ التعيين الحالي
                Status = "InProgress"           // تبدأ المهمة بحالة InProgress
            };

            // حفظ التعيين في قاعدة البيانات
            _assignmentsRepo.Add(assignment);
            gig.Status = "Assigned";
            _gigsRepo.Update(gig);

            // إرجاع البيانات على شكل DTO
            return MapToDto(assignment);
        }

        // تسليم المهمة من قبل المستقل | Submit Assignment
        public bool SubmitAssignment(int assignmentId, int freelancerId)
        {
            // البحث عن التعيين
            var assignment = _assignmentsRepo.GetById(assignmentId);

            // التأكد أن التعيين موجود ويخص نفس المستقل
            if (assignment == null || assignment.FreelancerId != freelancerId)
                return false;

            // يجب أن تكون الحالة InProgress
            if (assignment.Status != "InProgress")
                return false;

            // تغيير الحالة إلى Submitted
            assignment.Status = "Submitted";

            // حفظ تاريخ التسليم
            assignment.CompletionDate = DateTime.UtcNow;

            // تحديث البيانات
            _assignmentsRepo.Update(assignment);

            return true;
        }

        // موافقة العميل على المهمة | Approve Assignment
        public bool ApproveAssignment(int assignmentId, int clientId)
        {
            // البحث عن التعيين
            var assignment = _assignmentsRepo.GetById(assignmentId);

            if (assignment == null)
                return false;

            // البحث عن الخدمة
            var gig = _gigsRepo.GetById(assignment.GigId);

            // التأكد أن العميل هو صاحب الخدمة
            if (gig == null || gig.ClientId != clientId)
                return false;

            // تغيير حالة التعيين إلى Approved
            assignment.Status = "Approved";

            _assignmentsRepo.Update(assignment);

            // تغيير حالة الخدمة إلى Completed
            gig.Status = "Completed";

            _gigsRepo.Update(gig);

            return true;
        }

        // تحويل Entity إلى DTO | Convert Entity to DTO
        private GigAssignmentsOutputDTOs MapToDto(GigAssignments a)
        {
            return new GigAssignmentsOutputDTOs
            {
                AssignmentId = a.AssignmentId,

                GigId = a.GigId,

                // إذا لم يوجد عنوان يرجع Unknown
                GigTitle = a.Gig?.Title ?? "Unknown",

                FreelancerId = a.FreelancerId,

                // إذا لم يوجد اسم يرجع Unknown
                FreelancerName = a.Freelancer?.UserName ?? "Unknown",

                AgreedPrice = a.AgreedPrice,

                AssignedDate = a.AssignedDate,

                CompletionDate = a.CompletionDate,

                Status = a.Status
            };
        }
    }



}

