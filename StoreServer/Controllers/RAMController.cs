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
    public class RAMController : ControllerBase
    {
        private readonly ComputerContext _context;

        public RAMController(ComputerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RAMDTO>>> GetRAMs()
        {
            var rams = await _context.RAM
                .Select(r => new RAMDTO
                {
                    Id = r.Id,
                    Type = r.Type,
                    Size = r.Size,
                    Frequency = r.Frequency,
                    Model = r.Model,
                    Price = r.Price
                })
                .ToListAsync();

            return Ok(rams);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RAMDTO>> GetRAM(int id)
        {
            var ram = await _context.RAM
                .Select(r => new RAMDTO
                {
                    Id = r.Id,
                    Type = r.Type,
                    Size = r.Size,
                    Frequency = r.Frequency,
                    Model = r.Model,
                    Price = r.Price
                })
                .FirstOrDefaultAsync(r => r.Id == id);

            if (ram == null)
            {
                return NotFound();
            }

            return Ok(ram);
        }

        [HttpPost]
        public async Task<ActionResult<RAMDTO>> PostRAM(RAMDTO ramDTO)
        {
            var ram = new RAM
            {
                Type = ramDTO.Type,
                Size = ramDTO.Size,
                Frequency = ramDTO.Frequency,
                Model = ramDTO.Model,
                Price = ramDTO.Price
            };

            _context.RAM.Add(ram);
            await _context.SaveChangesAsync();

            ramDTO.Id = ram.Id;

            return CreatedAtAction(nameof(GetRAM), new { id = ram.Id }, ramDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRAM(int id, RAMDTO ramDTO)
        {
            if (id != ramDTO.Id)
            {
                return BadRequest();
            }

            var ram = await _context.RAM.FindAsync(id);

            if (ram == null)
            {
                return NotFound();
            }

            ram.Type = ramDTO.Type;
            ram.Size = ramDTO.Size;
            ram.Frequency = ramDTO.Frequency;
            ram.Model = ramDTO.Model;
            ram.Price = ramDTO.Price;

            _context.Entry(ram).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.RAM.Any(e => e.Id == id))
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
        public async Task<IActionResult> DeleteRAM(int id)
        {
            var ram = await _context.RAM.FindAsync(id);
            if (ram == null)
            {
                return NotFound();
            }

            _context.RAM.Remove(ram);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
