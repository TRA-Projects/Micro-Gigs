using Micro_Gigs.Models;
using Micro_Gigs.DTOs;
using Micro_Gigs.Repositories;

namespace Micro_Gigs.Services
{
    public class GigAttachmentsService
    {
        private readonly GigAttachmentsRepo _repo;

        public GigAttachmentsService(GigAttachmentsRepo repo)
        {
            _repo = repo;
        }

        public async Task CreateAttachment(GigAttachmentsInputDTOs input, Guid userId)
        {
            //var newFile = new GigAttachments
            //{
            //    AttachmentId = Guid.NewGuid(),
            //    GigId = input.GigId,
            //    FileUrl = input.FileUrl,
            //    UploadedBy = userId
            //};

            //await _repo.AddAsync(newFile);
        }
    }
}