using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLyDoAnTotNghiep.Models.BoardMembers;
using QLyDoAnTotNghiep.Services.BoardMembers;

namespace QLyDoAnTotNghiep.Controllers.BoardMembers
{
    [Route("api/boardmembers")]
    [ApiController]
    public class BoardMembersController : ControllerBase
    {
        private readonly IBoardMembersService _boardMembersService;

        public BoardMembersController(IBoardMembersService boardMembersService)
        {
            _boardMembersService = boardMembersService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var members = await _boardMembersService.GetAllBoardMembersAsync();
            return Ok(members);
        }

        [HttpGet("board/{boardId}")]
        [Authorize]
        public async Task<IActionResult> GetByBoardId(int boardId)
        {
            var members = await _boardMembersService.GetMembersByBoardIdAsync(boardId);
            return Ok(members);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var member = await _boardMembersService.GetByIdAsync(id);
            if (member == null) return NotFound();
            return Ok(member);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] BoardMember boardMember)
        {
            try
            {
                var created = await _boardMembersService.CreateBoardMemberAsync(boardMember);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {   
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] BoardMember boardMember)
        {
            boardMember.Id = id;
            var success = await _boardMembersService.UpdateBoardMemberAsync(boardMember);
            if (!success) return NotFound();
            return Ok(new { message = "Cập nhật vai trò thành viên thành công" });
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _boardMembersService.DeleteBoardMemberAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Xóa thành viên hội đồng thành công" });
        }
    }
}
