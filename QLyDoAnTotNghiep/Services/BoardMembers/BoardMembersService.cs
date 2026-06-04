using QLyDoAnTotNghiep.Data;
using Microsoft.EntityFrameworkCore;
using QLyDoAnTotNghiep.Models.BoardMembers;

namespace QLyDoAnTotNghiep.Services.BoardMembers
{
    public class BoardMembersService : IBoardMembersService
    {
        private readonly AppDbContext _context;

        public BoardMembersService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BoardMember> CreateBoardMemberAsync(BoardMember boardMember)
        {
            // Kiểm tra unique (một user không thể có 2 vai trò trong cùng hội đồng)
            if (await _context.BoardMembers.AnyAsync(bm =>
                bm.BoardId == boardMember.BoardId && bm.UserId == boardMember.UserId))
            {
                throw new Exception("Thành viên này đã tồn tại trong hội đồng");
            }

            _context.BoardMembers.Add(boardMember);
            await _context.SaveChangesAsync();
            return boardMember.GetSafeBoardMember();
        }

        //Get
        public async Task<List<BoardMember>> GetAllBoardMembersAsync()
        {
            return await _context.BoardMembers
                .Include(bm => bm.EvaluationBoard)
                .Include(bm => bm.User)
                .Select(bm => new BoardMember
                {
                    Id = bm.Id,
                    BoardId = bm.BoardId,
                    UserId = bm.UserId,
                    Role = bm.Role
                })
                .ToListAsync();
        }

        public async Task<List<BoardMember>> GetMembersByBoardIdAsync(int boardId)
        {
            return await _context.BoardMembers
                .Include(bm => bm.User)
                .Where(bm => bm.BoardId == boardId)
                .ToListAsync();
        }

        public async Task<BoardMember?> GetByIdAsync(int id)
        {
            return await _context.BoardMembers.FindAsync(id);
        }

        public async Task<bool> UpdateBoardMemberAsync(BoardMember boardMember)
        {
            var existing = await _context.BoardMembers.FindAsync(boardMember.Id);
            if (existing == null) return false;

            existing.Role = boardMember.Role;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBoardMemberAsync(int id)
        {
            var member = await _context.BoardMembers.FindAsync(id);
            if (member == null) return false;

            _context.BoardMembers.Remove(member);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
