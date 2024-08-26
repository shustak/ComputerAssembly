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
    public class FramesController : ControllerBase
    {
        private readonly ComputerContext _context;

        public FramesController(ComputerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FrameDTO>>> GetFrames()
        {
            var frames = await _context.Frames
                .Select(f => new FrameDTO
                {
                    Id = f.Id,
                    Model = f.Model,
                    FormFactor = f.FormFactor,
                    Price = f.Price
                })
                .ToListAsync();

            return Ok(frames);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FrameDTO>> GetFrame(int id)
        {
            var frame = await _context.Frames
                .Select(f => new FrameDTO
                {
                    Id = f.Id,
                    Model = f.Model,
                    FormFactor = f.FormFactor,
                    Price = f.Price
                })
                .FirstOrDefaultAsync(f => f.Id == id);

            if (frame == null)
            {
                return NotFound();
            }

            return Ok(frame);
        }

        [HttpPost]
        public async Task<ActionResult<FrameDTO>> PostFrame(FrameDTO frameDTO)
        {
            var frame = new Frame
            {
                Model = frameDTO.Model,
                FormFactor = frameDTO.FormFactor,
                Price = frameDTO.Price
            };

            _context.Frames.Add(frame);
            await _context.SaveChangesAsync();

            frameDTO.Id = frame.Id;

            return CreatedAtAction(nameof(GetFrame), new { id = frame.Id }, frameDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFrame(int id, FrameDTO frameDTO)
        {
            if (id != frameDTO.Id)
            {
                return BadRequest();
            }

            var frame = await _context.Frames.FindAsync(id);

            if (frame == null)
            {
                return NotFound();
            }

            frame.Model = frameDTO.Model;
            frame.FormFactor = frameDTO.FormFactor;
            frame.Price = frameDTO.Price;

            _context.Entry(frame).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Frames.Any(e => e.Id == id))
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
        public async Task<IActionResult> DeleteFrame(int id)
        {
            var frame = await _context.Frames.FindAsync(id);
            if (frame == null)
            {
                return NotFound();
            }

            _context.Frames.Remove(frame);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
