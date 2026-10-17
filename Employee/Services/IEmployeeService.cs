using Employee.Models;
namespace Employee.Services
{
    public interface IEmployeeService
    {
        Task<List<Employeee>> GetAllAsync(CancellationToken ct);
        Task<Employeee?> GetByIdAsync(int id, CancellationToken ct);
        Task<Employeee> CreateAsync(Employeee employee, CancellationToken ct);
        Task<bool> UpdateAsync(int id, Employeee employee, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
