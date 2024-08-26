using StoreServer.Data;
using StoreServer.DTOs;
using StoreServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreServer.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerAssembly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HDD_SSDController : ControllerBase
    {
        private readonly ComputerContext _context;

        public HDD_SSDController(ComputerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HDD_SSDDTO>>> GetHDD_SSDs()
        {
            var hdd_ssds = await _context.HDD_SSD
                .Select(h => new HDD_SSDDTO
                {
                    Id = h.Id,
                    Model = h.Model,
                    Type = h.Type,
                    Capacity = h.Capacity,
                    Interface = h.Interface,
                    Price = h.Price
                })
                .ToListAsync();

            return Ok(hdd_ssds);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HDD_SSDDTO>> GetHDD_SSD(int id)
        {
            var hdd_ssd = await _context.HDD_SSD
                .Select(h => new HDD_SSDDTO
                {
                    Id = h.Id,
                    Model = h.Model,
                    Type = h.Type,
                    Capacity = h.Capacity,
                    Interface = h.Interface,
                    Price = h.Price
                })
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hdd_ssd == null)
            {
                return NotFound();
            }

            return Ok(hdd_ssd);
        }

        [HttpPost]
        public async Task<ActionResult<HDD_SSDDTO>> PostHDD_SSD(HDD_SSDDTO hdd_ssdDTO)
        {
            var hdd_ssd = new HDD_SSD
            {
                Model = hdd_ssdDTO.Model,
                Type = hdd_ssdDTO.Type,
                Capacity = hdd_ssdDTO.Capacity,
                Interface = hdd_ssdDTO.Interface,
                Price = hdd_ssdDTO.Price
            };

            _context.HDD_SSD.Add(hdd_ssd);
            await _context.SaveChangesAsync();

            hdd_ssdDTO.Id = hdd_ssd.Id;

            return CreatedAtAction(nameof(GetHDD_SSD), new { id = hdd_ssd.Id }, hdd_ssdDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutHDD_SSD(int id, HDD_SSDDTO hdd_ssdDTO)
        {
            if (id != hdd_ssdDTO.Id)
            {
                return BadRequest();
            }

            var hdd_ssd = await _context.HDD_SSD.FindAsync(id);

            if (hdd_ssd == null)
            {
                return NotFound();
            }

            hdd_ssd.Model = hdd_ssdDTO.Model;
            hdd_ssd.Type = hdd_ssdDTO.Type;
            hdd_ssd.Capacity = hdd_ssdDTO.Capacity;
            hdd_ssd.Interface = hdd_ssdDTO.Interface;
            hdd_ssd.Price = hdd_ssdDTO.Price;

            _context.Entry(hdd_ssd).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.HDD_SSD.Any(e => e.Id == id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHDD_SSD(int id)
        {
            var hdd_ssd = await _context.HDD_SSD.FindAsync(id);
            if (hdd_ssd == null)
            {
                return NotFound();
            }

            _context.HDD_SSD.Remove(hdd_ssd);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
