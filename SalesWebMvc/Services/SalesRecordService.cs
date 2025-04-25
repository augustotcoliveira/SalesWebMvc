using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(result => result.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(result=> result.Date <= maxDate.Value);
            }
            return await result
                .Include(x => x.Seller) // join com seller
                .Include(x => x.Seller.Department) // join com os depts de seller
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }
        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(result => result.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(result => result.Date <= maxDate.Value);
            }
            return await result
                .Include(x => x.Seller) // join com seller
                .Include(x => x.Seller.Department) // join com os depts de seller
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();
        }
    }
}
