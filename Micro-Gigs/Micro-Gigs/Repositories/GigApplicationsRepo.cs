using System.Collections.Generic;
using System.Linq;
using Micro_Gigs.Models;
using Microsoft.EntityFrameworkCore;

namespace Micro_Gigs.Repositories
{
    /// <summary>
    /// واجهة (Interface) تحدد العمليات الأساسية المتاحة للتعامل مع طلبات التقديم (CRUD Operations).
    /// </summary>
    public interface IGigApplicationsRepo
    {
        // استرجاع كافة طلبات التقديم مع البيانات المرتبطة بها
        IEnumerable<GigApplications> GetAll();

        // استرجاع طلب تقديم معين بواسطة معرفه (ID)، أو إرجاع null إن لم يوجد
        GigApplications? GetById(int id);

        // إضافة طلب تقديم جديد وحفظ التغييرات في قاعدة البيانات
        void Add(GigApplications application);

        // تحديث بيانات طلب تقديم موجود مسبقاً وحفظ التغييرات
        void Update(GigApplications application);

        // حذف طلب تقديم من قاعدة البيانات وحفظ التغييرات
        void Delete(GigApplications application);
    }

    /// <summary>
    /// التطبيق الفعلي (Implementation) للواجهة باستخدام تقنية Entity Framework Core.
    /// </summary>
    public class GigApplicationsRepo : IGigApplicationsRepo
    {
        private readonly MicroGigsContext _context;

        // حقن سياق قاعدة البيانات (Database Context) عبر المُنشئ (Constructor Injection)
        public GigApplicationsRepo(MicroGigsContext context)
        {
            _context = context;
        }

        // جلب جميع الطلبات مع جلب البيانات المرتبطة (الخدمة والمستقل) باستخدام Include
        public IEnumerable<GigApplications> GetAll()
        {
            return _context.Applications
                .Include(a => a.Gig)
                .Include(a => a.Freelancer)
                .ToList();
        }

        // جلب طلب واحد بالمعرف مع بيانات الخدمة والمستقل المرتبطة به
        public GigApplications? GetById(int id)
        {
            return _context.Applications
                .Include(a => a.Gig)
                .Include(a => a.Freelancer)
                .FirstOrDefault(a => a.ApplicationId == id);
        }

        // إضافة الطلب الجديد لقاعدة البيانات وتنفيذ الحفظ الفعلي
        public void Add(GigApplications application)
        {
            _context.Applications.Add(application);
            _context.SaveChanges();
        }

        // تحديث بيانات الطلب في قاعدة البيانات وتنفيذ الحفظ
        public void Update(GigApplications application)
        {
            _context.Applications.Update(application);
            _context.SaveChanges();
        }

        // إزالة الطلب من قاعدة البيانات وتنفيذ الحفظ
        public void Delete(GigApplications application)
        {
            _context.Applications.Remove(application);
            _context.SaveChanges();
        }
    }
}