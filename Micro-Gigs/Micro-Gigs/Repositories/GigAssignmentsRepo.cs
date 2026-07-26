using Micro_Gigs.Models;
using Microsoft.EntityFrameworkCore;

namespace Micro_Gigs.Repositories
{
    // Repository for managing GigAssignments operations | مستودع لإدارة عمليات تعيين الخدمات

    public class GigAssignmentsRepo
    {
        // Database context reference | مرجع لسياق قاعدة البيانات
        private MicroGigsContext _context;

        // Constructor: Injects DB context |  يحقن اتصال قاعدة البيانات
        public GigAssignmentsRepo(MicroGigsContext context)
        {
            _context = context;
        }
        // Get all assignments with Gig & Freelancer data | جلب جميع التعيينات مع تفاصيل الخدمة والمستقل
        public List<GigAssignments> GetAll()
        {
            return _context.Assignments
                .Include(a => a.Gig)        // Include Gig | تضمين بيانات الخدمة
                .Include(a => a.Freelancer) // Include Freelancer | تضمين بيانات المستقل
                .ToList();                  // Execute & return list | تنفيذ الجلب وإرجاع قائمة
        }
        // Get assignment by ID | جلب التعيين عن طريق رقمه المرجعي
        public GigAssignments? GetById(int id)
        {
            return _context.Assignments
                .Include(a => a.Gig)        // Include Gig | تضمين بيانات الخدمة
                .Include(a => a.Freelancer) // Include Freelancer | تضمين بيانات المستقل
                .FirstOrDefault(a => a.AssignmentId == id); // Find match or null | إيجاد العنصر أو إرجاع فارغ



        }


        // Get assignment by Gig ID | جلب التعيين الخاص بخدمة معينة
        public GigAssignments? GetByGigId(int gigId)
        {
            return _context.Assignments
                .Include(a => a.Gig)        // Include Gig | تضمين بيانات الخدمة
                .Include(a => a.Freelancer) // Include Freelancer | تضمين بيانات المستقل
                .FirstOrDefault(a => a.GigId == gigId); // Find match or null | إيجاد العنصر أو إرجاع فارغ
        }
        // Get all assignments for a specific Freelancer | جلب كل التعيينات الخاصة بمستقل معين
        public List<GigAssignments> GetByFreelancerId(int freelancerId)
        {
            return _context.Assignments
                .Where(a => a.FreelancerId == freelancerId) // Filter by Freelancer | تصفية حسب رقم المستقل
                .Include(a => a.Gig)                       // Include Gig | تضمين بيانات الخدمة
                .ToList();                                 // Execute & return list | تنفيذ الجلب وإرجاع قائمة
        }
        // Add a new assignment | إضافة تعيين جديد
        public void Add(GigAssignments assignment)
        {
            _context.Assignments.Add(assignment); // Add to context | إضافة للكائن
            _context.SaveChanges();                // Save to DB | حفظ في قاعدة البيانات
        }

        // Update an existing assignment | تعديل تعيين موجود
        public void Update(GigAssignments assignment)
        {
            _context.Assignments.Update(assignment); // Update in context | تحديث الكائن
            _context.SaveChanges();                   // Save to DB | حفظ التغيرات
        }
        // Delete an assignment | حذف تعيين
        public void Delete(GigAssignments assignment)
        {
            _context.Assignments.Remove(assignment); // Remove from context | حذف الكائن
            _context.SaveChanges();                   // Save to DB | حفظ التغيرات
        }



    }
}
