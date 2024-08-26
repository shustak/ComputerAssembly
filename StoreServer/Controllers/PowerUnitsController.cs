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
    public class PowerUnitsController : ControllerBase
    {
        private readonly ComputerContext _context;

        public PowerUnitsController(ComputerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PowerUnitDTO>>> GetPowerUnits()
        {
            var powerUnits = await _context.PowerUnits
                .Select(p => new PowerUnitDTO
                {
                    Id = p.Id,
                    Model = p.Model,
                    Wattage = p.Wattage,
                    FormFactor = p.FormFactor,
                    Price = p.Price
                })
                .ToListAsync();

            return Ok(powerUnits);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PowerUnitDTO>> GetPowerUnit(int id)
        {
            var powerUnit = await _context.PowerUnits
                .Select(p => new PowerUnitDTO
                {
                    Id = p.Id,
                    Model = p.Model,
                    Wattage = p.Wattage,
                    FormFactor = p.FormFactor,
                    Price = p.Price
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            if (powerUnit == null)
            {
                return NotFound();
            }

            return Ok(powerUnit);
        }

        [HttpPost]
        public async Task<ActionResult<PowerUnitDTO>> PostPowerUnit(PowerUnitDTO powerUnitDTO)
        {
            var powerUnit = new PowerUnit
            {
                Model = powerUnitDTO.Model,
                Wattage = powerUnitDTO.Wattage,
                FormFactor = powerUnitDTO.FormFactor,
                Price = powerUnitDTO.Price
            };

            _context.PowerUnits.Add(powerUnit);
            await _context.SaveChangesAsync();

            powerUnitDTO.Id = powerUnit.Id;

            return CreatedAtAction(nameof(GetPowerUnit), new { id = powerUnit.Id }, powerUnitDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPowerUnit(int id, PowerUnitDTO powerUnitDTO)
        {
            if (id != powerUnitDTO.Id)
            {
                return BadRequest();
            }

            var powerUnit = await _context.PowerUnits.FindAsync(id);

            if (powerUnit == null)
            {
                return NotFound();
            }

            powerUnit.Model = powerUnitDTO.Model;
            powerUnit.Wattage = powerUnitDTO.Wattage;
            powerUnit.FormFactor = powerUnitDTO.FormFactor;
            powerUnit.Price = powerUnitDTO.Price;

            _context.Entry(powerUnit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PowerUnits.Any(e => e.Id == id))
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
        public async Task<IActionResult> DeletePowerUnit(int id)
        {
            var powerUnit = await _context.PowerUnits.FindAsync(id);
            if (powerUnit == null)
            {
                return NotFound();
            }

            _context.PowerUnits.Remove(powerUnit);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
