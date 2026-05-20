using CodeFirst.DTOs;

namespace CodeFirst.Services;

public interface IPCService
{
    Task<IEnumerable<PCDto>> GetAllPCsAsync();
    Task<PCWithComponentsDto?> GetPCWithComponentsAsync(int id);
    Task<PCDto> CreatePCAsync(PCManipulateDto dto);
    Task<bool> UpdatePCAsync(int id, PCManipulateDto dto);
    Task<bool> DeletePCAsync(int id);
}