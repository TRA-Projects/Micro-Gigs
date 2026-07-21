using Micro_Gigs.Models;


namespace Micro_Gigs.Repositories
{
    public class GigAttachmentsRepo
    {
        //private readonly MicroGigsContext _db; // مكتبة البيانات

        //public GigAttachmentsRepo(MicroGigsContext db)
        //{
        //    _db = db;
        //}

        //// وظيفة: حفظ المرفق في المخزن
        //public async Task AddAsync(GigAttachments data)
        //{
        //    await _db.GigAttachments.AddAsync(data);
        //    await _db.SaveChangesAsync(); // تثبيت الحفظ
        //}

        //// وظيفة: جلب المرفقات الخاصة بمهمة معينة من المخزن
        //public async Task<List<GigAttachments>> GetByGigIdAsync(Guid gigId)
        //{
        //    return await _db.GigAttachments.Where(x => x.GigId == gigId).ToListAsync();
        //}
    }
}
