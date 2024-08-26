using StoreServer.Data;
using StoreServer.DTOs;
using StoreServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessorsController : ControllerBase
    {
        private readonly ComputerContext _context;

        public ProcessorsController(ComputerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProcessorDTO>>> GetProcessors()
        {
            var processors = await _context.Processors
                .Select(p => new ProcessorDTO
                {
                    Id = p.Id,
                    Model = p.Model,
                    Socket = p.Socket,
                    Frequency = p.Frequency,
                    Cores = p.Cores,
                    Price = p.Price
                })
                .ToListAsync();

            return Ok(processors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProcessorDTO>> GetProcessor(int id)
        {
            var processor = await _context.Processors
                .Select(p => new ProcessorDTO
                {
                    Id = p.Id,
                    Model = p.Model,
                    Socket = p.Socket,
                    Frequency = p.Frequency,
                    Cores = p.Cores,
                    Price = p.Price
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            if (processor == null)
            {
                return NotFound();
            }

            return Ok(processor);
        }

        [HttpPost]
        public async Task<ActionResult<ProcessorDTO>> PostProcessor(ProcessorDTO processorDTO)
        {
            var processor = new Processor
            {
                Model = processorDTO.Model,
                Socket = processorDTO.Socket,
                Frequency = processorDTO.Frequency,
                Cores = processorDTO.Cores,
                Price = processorDTO.Price
            };

            _context.Processors.Add(processor);
            await _context.SaveChangesAsync();

            processorDTO.Id = processor.Id;

            return CreatedAtAction(nameof(GetProcessor), new { id = processor.Id }, processorDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProcessor(int id, ProcessorDTO processorDTO)
        {
            if (id != processorDTO.Id)
            {
                return BadRequest();
            }

            var processor = await _context.Processors.FindAsync(id);

            if (processor == null)
            {
                return NotFound();
            }

            processor.Model = processorDTO.Model;
            processor.Socket = processorDTO.Socket;
            processor.Frequency = processorDTO.Frequency;
            processor.Cores = processorDTO.Cores;
            processor.Price = processorDTO.Price;

            _context.Entry(processor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Processors.Any(e => e.Id == id))
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
        public async Task<IActionResult> DeleteProcessor(int id)
        {
            var processor = await _context.Processors.FindAsync(id);
            if (processor == null)
            {
                return NotFound();
            }

            _context.Processors.Remove(processor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
