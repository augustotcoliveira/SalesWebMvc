using SalesWebMvc.Data;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class DepartmentService
    {
        public SalesWebMvcContext _context;
        public DepartmentService(SalesWebMvcContext context) 
        {
            _context = context;
        }

        public async Task<List<Department>> FindAllAsync()
        {
            //chamada assincrona
            return await _context.Department.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
