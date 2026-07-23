using Micro_Gigs.Models;

namespace Micro_Gigs.Repositories.Interfaces
{
    public interface IGigAttachmentsRepository
    {
        Task<IEnumerable<GigAttachments>> GetAllAsync();

        Task<GigAttachments?> GetByIdAsync(int attachmentId);

        Task<IEnumerable<GigAttachments>> GetByGigIdAsync(int gigId);

        Task<IEnumerable<GigAttachments>> GetByUserIdAsync(int userId);

        Task<GigAttachments> AddAsync(GigAttachments attachment);

        Task<GigAttachments> UpdateAsync(GigAttachments attachment);

        Task<bool> DeleteAsync(int attachmentId);
    }
}