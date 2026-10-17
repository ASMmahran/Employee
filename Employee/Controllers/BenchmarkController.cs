using Employee.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Employee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BenchmarkController : ControllerBase
    {
        private readonly IDbContextFactory<AppDbContext> _factory;
        public BenchmarkController(IDbContextFactory<AppDbContext> factory) => _factory = factory;

        // عملية مستقلة: query + محاكاة latency (زي استدعاء خدمة خارجية)
        private async Task<int> CountEmployeesAsync(CancellationToken ct)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            var count = await db.Employees.CountAsync(ct);
            await Task.Delay(500, ct);
            return count;
        }

        private async Task<decimal> TotalSalariesAsync(CancellationToken ct)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            var total = await db.Employees.SumAsync(e => e.Salary, ct);
            await Task.Delay(500, ct);
            return total;
        }

        [HttpGet("compare")]
        public async Task<IActionResult> Compare(CancellationToken ct)
        {
            // warm-up عشان أول اتصال ما يبوّظ القياس
            await CountEmployeesAsync(ct);

            // 1) بالتتابع
            var sw = Stopwatch.StartNew();
            var c1 = await CountEmployeesAsync(ct);
            var t1 = await TotalSalariesAsync(ct);
            sw.Stop();
            var sequentialMs = sw.ElapsedMilliseconds;

            // 2) بالتوازي
            sw.Restart();
            var countTask = CountEmployeesAsync(ct);
            var totalTask = TotalSalariesAsync(ct);
            await Task.WhenAll(countTask, totalTask);
            sw.Stop();
            var parallelMs = sw.ElapsedMilliseconds;

            return Ok(new
            {
                sequentialMs,
                parallelMs,
                improvementPercent = Math.Round((1 - (double)parallelMs / sequentialMs) * 100, 1),
                count = countTask.Result,
                totalSalaries = totalTask.Result
            });
        }
    }
}
