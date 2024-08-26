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
    public class OtherComponentsController : ControllerBase
    {
        private readonly ComputerContext _context;

        public OtherComponentsController(ComputerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OtherComponentDTO>>> GetOtherComponents()
        {
            var components = await _context.Components
                .Select(oc => new OtherComponentDTO
                {
                    Id = oc.Id,
                    Type = oc.Type,
                    Model = oc.Model,
                    Manufacturer = oc.Manufacturer,
                    Price = oc.Price
                })
                .ToListAsync();

            return Ok(components);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OtherComponentDTO>> GetOtherComponent(int id)
        {
            var component = await _context.Components
                .Select(oc => new OtherComponentDTO
                {
                    Id = oc.Id,
                    Type = oc.Type,
                    Model = oc.Model,
                    Manufacturer = oc.Manufacturer,
                    Price = oc.Price
                })
                .FirstOrDefaultAsync(oc => oc.Id == id);

            if (component == null)
            {
                return NotFound();
            }

            return Ok(component);
        }

        [HttpPost]
        public async Task<ActionResult<OtherComponentDTO>> PostOtherComponent(OtherComponentDTO otherComponentDTO)
        {
            var component = new OtherComponent
            {
                Type = otherComponentDTO.Type,
                Model = otherComponentDTO.Model,
                Manufacturer = otherComponentDTO.Manufacturer,
                Price = otherComponentDTO.Price
            };

            _context.Components.Add(component);
            await _context.SaveChangesAsync();

            otherComponentDTO.Id = component.Id;

            return CreatedAtAction(nameof(GetOtherComponent), new { id = component.Id }, otherComponentDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOtherComponent(int id, OtherComponentDTO otherComponentDTO)
        {
            if (id != otherComponentDTO.Id)
            {
                return BadRequest();
            }

            var component = await _context.Components.FindAsync(id);

            if (component == null)
            {
                return NotFound();
            }

            component.Type = otherComponentDTO.Type;
            component.Model = otherComponentDTO.Model;
            component.Manufacturer = otherComponentDTO.Manufacturer;
            component.Price = otherComponentDTO.Price;

            _context.Entry(component).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Components.Any(e => e.Id == id))
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
        public async Task<IActionResult> DeleteOtherComponent(int id)
        {
            var component = await _context.Components.FindAsync(id);
            if (component == null)
            {
                return NotFound();
            }

            _context.Components.Remove(component);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
