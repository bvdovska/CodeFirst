using Microsoft.EntityFrameworkCore;
using CodeFirst.Data;
using CodeFirst.DTOs;
using CodeFirst.Models;

namespace CodeFirst.Services;

public class PCService : IPCService
{
    private readonly AppDbContext _context;

    public PCService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PCDto>> GetAllPCsAsync()
    {
        return await _context.PCs.Select(p => new PCDto
        {
            Id = p.Id,
            Name = p.Name,
            Weight = p.Weight,
            Warranty = p.Warranty,
            CreatedAt = p.CreatedAt,
            Stock = p.Stock
        }).ToListAsync();
    }

    public async Task<PCWithComponentsDto?> GetPCWithComponentsAsync(int id)
    {
        var pc = await _context.PCs
            .Include(p => p.PCComponents)
            .ThenInclude(pc => pc.Component)
            .ThenInclude(c => c.Manufacturer)
            .Include(p => p.PCComponents)
            .ThenInclude(pc => pc.Component)
            .ThenInclude(c => c.Type)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pc == null) return null;

        return new PCWithComponentsDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock,
            Components = pc.PCComponents.Select(c => new PCComponentDetailDto
            {
                Amount = c.Amount,
                Component = new ComponentDetailDto
                {
                    Code = c.Component.Code,
                    Name = c.Component.Name,
                    Description = c.Component.Description,
                    Manufacturer = new ManufacturerDto
                    {
                        Id = c.Component.Manufacturer.Id,
                        Abbreviation = c.Component.Manufacturer.Abbreviation,
                        FullName = c.Component.Manufacturer.FullName,
                        FoundationDate = c.Component.Manufacturer.FoundationDate.ToString("yyyy-MM-dd")
                    },
                    Type = new TypeDto
                    {
                        Id = c.Component.Type.Id,
                        Abbreviation = c.Component.Type.Abbreviation,
                        Name = c.Component.Type.Name
                    }
                }
            }).ToList()
        };
    }

    public async Task<PCDto> CreatePCAsync(PCManipulateDto dto)
    {
        var pc = new PC
        {
            Name = dto.Name,
            Weight = dto.Weight,
            Warranty = dto.Warranty,
            CreatedAt = dto.CreatedAt,
            Stock = dto.Stock
        };

        _context.PCs.Add(pc);
        await _context.SaveChangesAsync();

        return new PCDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };
    }

    public async Task<bool> UpdatePCAsync(int id, PCManipulateDto dto)
    {
        var pc = await _context.PCs.FindAsync(id);
        if (pc == null) return false;

        pc.Name = dto.Name;
        pc.Weight = dto.Weight;
        pc.Warranty = dto.Warranty;
        pc.CreatedAt = dto.CreatedAt;
        pc.Stock = dto.Stock;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePCAsync(int id)
    {
        var pc = await _context.PCs
            .Include(p => p.PCComponents)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pc == null) return false;

        _context.PCComponents.RemoveRange(pc.PCComponents);
        _context.PCs.Remove(pc);
        await _context.SaveChangesAsync();
        return true;
    }
}