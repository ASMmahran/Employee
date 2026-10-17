using Employee.Data;
using Employee.Models;
using Microsoft.EntityFrameworkCore;
namespace Employee.Services
{

    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _db;
        public EmployeeService(AppDbContext db) => _db = db;

        public Task<List<Employeee>> GetAllAsync(CancellationToken ct) =>
            _db.Employees.AsNoTracking().ToListAsync(ct);

        public Task<Employeee?> GetByIdAsync(int id, CancellationToken ct) =>
            _db.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, ct);

        public async Task<Employeee> CreateAsync(Employeee employee, CancellationToken ct)
        {
            _db.Employees.Add(employee);
            await _db.SaveChangesAsync(ct);
            return employee;
        }

        public async Task<bool> UpdateAsync(int id, Employeee employee, CancellationToken ct)
        {
            var existing = await _db.Employees.FirstOrDefaultAsync(e => e.Id == id, ct);
            if (existing is null) return false;

            existing.Name = employee.Name;
            existing.Position = employee.Position;
            existing.Salary = employee.Salary;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var existing = await _db.Employees.FirstOrDefaultAsync(e => e.Id == id, ct);
            if (existing is null) return false;

            _db.Employees.Remove(existing);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
