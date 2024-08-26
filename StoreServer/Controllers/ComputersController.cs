using StoreServer.Data;
using StoreServer.DTOs;
using StoreServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerAssembly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputersController : ControllerBase
    {
        private readonly ComputerContext _context;

        public ComputersController(ComputerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComputerDTO>>> GetComputers()
        {
            var computers = await _context.Computers
                .Include(c => c.Processor)
                .Include(c => c.RAM)
                .Include(c => c.Motherboard)
                .Include(c => c.Frame)
                .Include(c => c.PowerUnit)
                .Include(c => c.HDD_SSD)
                .Include(c => c.OtherComponents)
                .Select(c => new ComputerDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Processor = new ProcessorDTO
                    {
                        Id = c.Processor.Id,
                        Model = c.Processor.Model,
                        Socket = c.Processor.Socket,
                        Frequency = c.Processor.Frequency,
                        Cores = c.Processor.Cores,
                        Price = c.Processor.Price
                    },
                    RAM = new RAMDTO
                    {
                        Id = c.RAM.Id,
                        Type = c.RAM.Type,
                        Size = c.RAM.Size,
                        Frequency = c.RAM.Frequency,
                        Model = c.RAM.Model,
                        Price = c.RAM.Price
                    },
                    Motherboard = new MotherboardDTO
                    {
                        Id = c.Motherboard.Id,
                        Model = c.Motherboard.Model,
                        Socket = c.Motherboard.Socket,
                        FormFactor = c.Motherboard.FormFactor,
                        RAMType = c.Motherboard.RAMType,
                        Price = c.Motherboard.Price
                    },
                    Frame = new FrameDTO
                    {
                        Id = c.Frame.Id,
                        Model = c.Frame.Model,
                        FormFactor = c.Frame.FormFactor,
                        Price = c.Frame.Price
                    },
                    PowerUnit = new PowerUnitDTO
                    {
                        Id = c.PowerUnit.Id,
                        Model = c.PowerUnit.Model,
                        Wattage = c.PowerUnit.Wattage,
                        FormFactor = c.PowerUnit.FormFactor,
                        Price = c.PowerUnit.Price
                    },
                    HDD_SSD = new HDD_SSDDTO
                    {
                        Id = c.HDD_SSD.Id,
                        Model = c.HDD_SSD.Model,
                        Type = c.HDD_SSD.Type,
                        Capacity = c.HDD_SSD.Capacity,
                        Interface = c.HDD_SSD.Interface,
                        Price = c.HDD_SSD.Price
                    },
                    OtherComponents = c.OtherComponents.Select(oc => new OtherComponentDTO
                    {
                        Id = oc.Id,
                        Type = oc.Type,
                        Model = oc.Model,
                        Manufacturer = oc.Manufacturer,
                        Price = oc.Price
                    }).ToList(),
                    TotalPrice = c.TotalPrice
                })
                .ToListAsync();

            return Ok(computers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComputerDTO>> GetComputer(int id)
        {
            var computer = await _context.Computers
                .Include(c => c.Processor)
                .Include(c => c.RAM)
                .Include(c => c.Motherboard)
                .Include(c => c.Frame)
                .Include(c => c.PowerUnit)
                .Include(c => c.HDD_SSD)
                .Include(c => c.OtherComponents)
                .Select(c => new ComputerDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Processor = new ProcessorDTO
                    {
                        Id = c.Processor.Id,
                        Model = c.Processor.Model,
                        Socket = c.Processor.Socket,
                        Frequency = c.Processor.Frequency,
                        Cores = c.Processor.Cores,
                        Price = c.Processor.Price
                    },
                    RAM = new RAMDTO
                    {
                        Id = c.RAM.Id,
                        Type = c.RAM.Type,
                        Size = c.RAM.Size,
                        Frequency = c.RAM.Frequency,
                        Model = c.RAM.Model,
                        Price = c.RAM.Price
                    },
                    Motherboard = new MotherboardDTO
                    {
                        Id = c.Motherboard.Id,
                        Model = c.Motherboard.Model,
                        Socket = c.Motherboard.Socket,
                        FormFactor = c.Motherboard.FormFactor,
                        RAMType = c.Motherboard.RAMType,
                        Price = c.Motherboard.Price
                    },
                    Frame = new FrameDTO
                    {
                        Id = c.Frame.Id,
                        Model = c.Frame.Model,
                        FormFactor = c.Frame.FormFactor,
                        Price = c.Frame.Price
                    },
                    PowerUnit = new PowerUnitDTO
                    {
                        Id = c.PowerUnit.Id,
                        Model = c.PowerUnit.Model,
                        Wattage = c.PowerUnit.Wattage,
                        FormFactor = c.PowerUnit.FormFactor,
                        Price = c.PowerUnit.Price
                    },
                    HDD_SSD = new HDD_SSDDTO
                    {
                        Id = c.HDD_SSD.Id,
                        Model = c.HDD_SSD.Model,
                        Type = c.HDD_SSD.Type,
                        Capacity = c.HDD_SSD.Capacity,
                        Interface = c.HDD_SSD.Interface,
                        Price = c.HDD_SSD.Price
                    },
                    OtherComponents = c.OtherComponents.Select(oc => new OtherComponentDTO
                    {
                        Id = oc.Id,
                        Type = oc.Type,
                        Model = oc.Model,
                        Manufacturer = oc.Manufacturer,
                        Price = oc.Price
                    }).ToList(),
                    TotalPrice = c.TotalPrice
                })
                .FirstOrDefaultAsync(c => c.Id == id);

            if (computer == null)
            {
                return NotFound();
            }

            return Ok(computer);
        }

        [HttpPost]
        public async Task<ActionResult<ComputerDTO>> PostComputer(ComputerDTO computerDTO)
        {
            var computer = new Computer
            {
                Name = computerDTO.Name,
                ProcessorId = computerDTO.Processor.Id,
                RAMId = computerDTO.RAM.Id,
                MotherboardId = computerDTO.Motherboard.Id,
                FrameId = computerDTO.Frame.Id,
                PowerUnitId = computerDTO.PowerUnit.Id,
                StorageId = computerDTO.HDD_SSD.Id,
                Price = computerDTO.TotalPrice,
                OtherComponents = computerDTO.OtherComponents.Select(oc => new OtherComponent
                {
                    Type = oc.Type,
                    Model = oc.Model,
                    Manufacturer = oc.Manufacturer,
                    Price = oc.Price
                }).ToList()
            };

            _context.Computers.Add(computer);
            await _context.SaveChangesAsync();

            computerDTO.Id = computer.Id;

            return CreatedAtAction(nameof(GetComputer), new { id = computer.Id }, computerDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComputer(int id, ComputerDTO computerDTO)
        {
            if (id != computerDTO.Id)
            {
                return BadRequest();
            }

            var computer = await _context.Computers.Include(c => c.OtherComponents).FirstOrDefaultAsync(c => c.Id == id);

            if (computer == null)
            {
                return NotFound();
            }

            computer.Name = computerDTO.Name;
            computer.ProcessorId = computerDTO.Processor.Id;
            computer.RAMId = computerDTO.RAM.Id;
            computer.MotherboardId = computerDTO.Motherboard.Id;
            computer.FrameId = computerDTO.Frame.Id;
            computer.PowerUnitId = computerDTO.PowerUnit.Id;
            computer.StorageId = computerDTO.HDD_SSD.Id;
            computer.Price = computerDTO.TotalPrice;

            // Удаление старых компонентов
            //_context.OtherComponents.RemoveRange(computer.OtherComponents);

            // Добавление новых компонентов
            computer.OtherComponents = computerDTO.OtherComponents.Select(oc => new OtherComponent
            {
                Type = oc.Type,
                Model = oc.Model,
                Manufacturer = oc.Manufacturer,
                Price = oc.Price
            }).ToList();

            _context.Entry(computer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Computers.Any(e => e.Id == id))
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
        public async Task<IActionResult> DeleteComputer(int id)
        {
            var computer = await _context.Computers.FindAsync(id);
            if (computer == null)
            {
                return NotFound();
            }

            _context.Computers.Remove(computer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
