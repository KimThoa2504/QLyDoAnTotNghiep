using QLyDoAnTotNghiep.Models.BoardMembers;

namespace QLyDoAnTotNghiep.Services.BoardMembers
{
    public interface IBoardMembersService
    {
        Task<BoardMember> CreateBoardMemberAsync(BoardMember boardMember);
        Task<List<BoardMember>> GetAllBoardMembersAsync();
        Task<List<BoardMember>> GetMembersByBoardIdAsync(int boardId);
        Task<BoardMember?> GetByIdAsync(int id);
        Task<bool> UpdateBoardMemberAsync(BoardMember boardMember);
        Task<bool> DeleteBoardMemberAsync(int id);
    }
}
