using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionsController(
        AuctionDbContext context,
        IMapper mapper
    ) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly AuctionDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string date)
        {
            var query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

            if (!string.IsNullOrEmpty(date))
            {
                query = query.Where(
                    x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0
                ); /// here we are trying to get updatedAt is greater then date which is comming from frontend
            }
            return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
        {
            var auction = await _context.Auctions
                .Include(a => a.Item)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (auction is null) return NotFound();

            return _mapper.Map<AuctionDto>(auction);
        }

        [HttpPost]
        public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
        {
            var auction = _mapper.Map<Auction>(auctionDto);

            auction.Seller = "TEST";

            _context.Auctions.Add(auction);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest("Could not save the changes to DB");

            return CreatedAtAction(
                nameof(GetAuctionById),
                new { auction.Id },
                 _mapper.Map<AuctionDto>(auction)
            );
        }

        [HttpPut("{id}")]

        public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
        {
            var auction = await _context.Auctions.Include(auc => auc.Item)
                .FirstOrDefaultAsync(auc => auc.Id == id);

            if (auction is null) return NotFound();

            auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
            auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest("Problem saving values");
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuction(Guid id)
        {
            var auction = await _context.Auctions.FindAsync(id);

            if (auction == null) return NotFound();

            _context.Auctions.Remove(auction);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest("Problem deleting values");
        }

    }
}
