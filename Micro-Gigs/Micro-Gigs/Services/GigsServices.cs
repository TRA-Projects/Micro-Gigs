using Micro_Gigs.DTOs;
using Micro_Gigs.Models;
using Micro_Gigs.Repositories;

namespace Micro_Gigs.Services
{
    public class GigsServices
    {
        private GigsRepo gigsRepo;
        private UsersRepo usersRepo;

        public GigsServices(GigsRepo _gigsRepo, UsersRepo _usersRepo) 
        { 
            gigsRepo = _gigsRepo;
            usersRepo = _usersRepo;
        }

        // Helper: تحويل Model إلى DTO
        private GigResponseDto MapToDto(Gigs gig)
        {
            return new GigResponseDto
            {
                GigId = gig.GigId,
                Title = gig.Title,
                Description = gig.Description,
                Budget = gig.Budget,
                DueDate = gig.DueDate,
                Status = gig.Status,
                PostedDate = gig.PostedDate,
                ClientName = gig.Client?.UserName??"Unknown",
                CategoryName = gig.GigCategory?.CategoryName??"Unknown"
            };
        }


        public List<GigResponseDto> GetAllGigs() {
            var gigs = gigsRepo.GetAll();
            return gigs.Select(MapToDto).ToList();
        }

        public List<GigResponseDto> GetOpenGigs()
        {
            var gigs = gigsRepo.GetOpenGigs();
            return gigs.Select(MapToDto).ToList();
        }

        public GigResponseDto? GetGigById(int id)
        {
            var gig = gigsRepo.GetById(id);
            if (gig == null) return null;
            return MapToDto(gig);
        }

        public List<GigResponseDto> GetGigsByClient(int clientId)
        {
            var gigs = gigsRepo.GetByClientId(clientId);
    
            return gigs.Select(MapToDto).ToList();
        }

        public GigResponseDto? CreateGig(CreateGigDto dto, int clientId)
        {
            var client = usersRepo.GetById(clientId);
            if (client == null || client.UserType != "Client")
                return null;
            var gig = new Gigs
            {
                Title = dto.Title,
                Description = dto.Description,
                Budget = dto.Budget,
                DueDate = dto.DueDate,
                Status = "Open",
                PostedDate = DateTime.Now,
                ClientId = clientId,
                GigCategoryId = dto.GigCategoryId
            };

            gigsRepo.Add(gig);
            return MapToDto(gig);
        }

        public bool UpdateGig(int id, CreateGigDto dto, int clientId)
        {
            var gig = gigsRepo.GetById(id);
            if (gig == null) return false;
            if (gig.ClientId != clientId) return false;

            gig.Title = dto.Title;
            gig.Description = dto.Description;
            gig.Budget = dto.Budget;
            gig.DueDate = dto.DueDate;
            gig.GigCategoryId = dto.GigCategoryId;

            gigsRepo.Update(gig);
            return true;
        }


        public bool DeleteGig(int id)
        {
            var gig = gigsRepo.GetById(id);
            if (gig == null) return false;

            gigsRepo.Delete(gig);
            return true;
        }
    }
}
