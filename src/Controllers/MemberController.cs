using LoyaltySystem.Models;
using LoyaltySystem.src.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoyaltySystem.Controllers
{
    [Route("api/member/")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly Context _context;

        public MemberController(Context context)
        {
            _context = context;
        }

        // GET: api/member
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetAllMembers()
        {
            if (_context.Member == null)
            {
                return NotFound();
            }
            return await _context.Member.ToListAsync();
        }

        // GET: api/member/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            if (_context.Member == null)
            {
                return NotFound();
            }
            var member = await _context.Member.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return member;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(int id, Member updateMember)
        {
            if (id != updateMember.Id)
            {
                return BadRequest();
            }

            var existingMember = await _context.Member.FindAsync(id);
            if (existingMember == null)
            {
                return NotFound();
            }

            // Updating the member's data
            existingMember.Name = updateMember.Name;

            if (existingMember.PurchasesTotalAmount != 0)
            {
                existingMember.PurchasesTotalAmount = existingMember.PurchasesTotalAmount + updateMember.PurchasesTotalAmount;
            }

            if (existingMember.PurchasesTotalAmount == 0)
            {
                existingMember.PurchasesTotalAmount = updateMember.PurchasesTotalAmount;
            }

            // Updating points
            if (existingMember.PurchasesTotalAmount >= 10 && existingMember.PurchasesTotalAmount % 10 == 0)
            {
                int newPoints = existingMember.PurchasesTotalAmount / 10;
                existingMember.Points = newPoints;
            }

            // Updating discount
            if (existingMember.Points >= 10)
            {
                int newDiscount = existingMember.Points / 10;

                existingMember.Discount = newDiscount * 3;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/member
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(MemberPostDTO memberDto)
        {
            var member = new Member
            {
                Id = memberDto.Id,
                Name = memberDto.Name
            };

            _context.Member.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
        }

        private bool MemberExists(int id)
        {
            return _context.Member.Any(e => e.Id == id);
        }

        private MemberPostDTO CreateMemberPostDTO(Member member)
        {
            return new MemberPostDTO
            {
                Id =  member.Id,
                Name = member.Name
            };
        }

    }
}
