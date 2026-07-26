using Micro_Gigs.Models;

namespace Micro_Gigs.Repositories.Interfaces
{
    public interface IGigReviewsRepository
    {
        Task<IEnumerable<GigReviews>> GetAllAsync();

        Task<GigReviews?> GetByIdAsync(int reviewId);

        Task<IEnumerable<GigReviews>> GetByAssignmentIdAsync(int assignmentId);

        Task<IEnumerable<GigReviews>> GetByReviewerIdAsync(int reviewerId);

        Task<GigReviews> AddAsync(GigReviews review);

        Task<GigReviews> UpdateAsync(GigReviews review);

        Task<bool> DeleteAsync(int reviewId);
    }
}