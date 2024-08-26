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
    public class MotherboardsController : ControllerBase
    {
        private readonly ComputerContext _context;

        public MotherboardsController(ComputerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MotherboardDTO>>> GetMotherboards()
        {
            var motherboards = await _context.Motherboards
                .Select(m => new MotherboardDTO
                {
                    Id = m.Id,
                    Model = m.Model,
                    Socket = m.Socket,
                    FormFactor = m.FormFactor,
                    RAMType = m.RAMType,
                    Price = m.Price
                })
                .ToListAsync();

            return Ok(motherboards);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MotherboardDTO>> GetMotherboard(int id)
        {
            var motherboard = await _context.Motherboards
                .Select(m => new MotherboardDTO
                {
                    Id = m.Id,
                    Model = m.Model,
                    Socket = m.Socket,
                    FormFactor = m.FormFactor,
                    RAMType = m.RAMType,
                    Price = m.Price
                })
                .FirstOrDefaultAsync(m => m.Id == id);

            if (motherboard == null)
            {
                return NotFound();
            }

            return Ok(motherboard);
        }

        [HttpPost]
        public async Task<ActionResult<MotherboardDTO>> PostMotherboard(MotherboardDTO motherboardDTO)
        {
            var motherboard = new Motherboard
            {
                Model = motherboardDTO.Model,
                Socket = motherboardDTO.Socket,
                FormFactor = motherboardDTO.FormFactor,
                RAMType = motherboardDTO.RAMType,
                Price = motherboardDTO.Price
            };

            _context.Motherboards.Add(motherboard);
            await _context.SaveChangesAsync();

            motherboardDTO.Id = motherboard.Id;

            return CreatedAtAction(nameof(GetMotherboard), new { id = motherboard.Id }, motherboardDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMotherboard(int id, MotherboardDTO motherboardDTO)
        {
            if (id != motherboardDTO.Id)
            {
                return BadRequest();
            }

            var motherboard = await _context.Motherboards.FindAsync(id);

            if (motherboard == null)
            {
                return NotFound();
            }

            motherboard.Model = motherboardDTO.Model;
            motherboard.Socket = motherboardDTO.Socket;
            motherboard.FormFactor = motherboardDTO.FormFactor;
            motherboard.RAMType = motherboardDTO.RAMType;
            motherboard.Price = motherboardDTO.Price;

            _context.Entry(motherboard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Motherboards.Any(e => e.Id == id))
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
        public async Task<IActionResult> DeleteMotherboard(int id)
        {
            var motherboard = await _context.Motherboards.FindAsync(id);
            if (motherboard == null)
            {
                return NotFound();
            }

            _context.Motherboards.Remove(motherboard);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
