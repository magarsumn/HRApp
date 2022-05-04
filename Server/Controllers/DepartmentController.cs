using System.Threading.Tasks;
using HRApp.Server.Models;
using HRApp.Server.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAssembly.Shared.Models;
using WebAssembly.Shared.Models.Dto;

namespace HRApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<Designation> _designationRepository;

        public DepartmentController(
            IRepository<Department> departmentRepository,
            IRepository<Designation> designationRepository
            )
        {
            _departmentRepository = departmentRepository;
            _designationRepository = designationRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _departmentRepository.GetAllListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _departmentRepository.FirstOrDefaultAsync(a => a.Id == id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Department department)
        {
            _departmentRepository.Add(department);
            await _departmentRepository.SaveChangesAsync();
            return Ok(department);
        }

        [HttpPut]
        public async Task<IActionResult> Put(DepartmentDtos input)
        {
            var data = await _departmentRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (data == null)
            {
                return NotFound();
            }
            
            data.Name = input.Name;
            data.Description=input.Description;
            await _departmentRepository.UpdateAsync(data);
            return Ok(data);
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _departmentRepository.FirstOrDefaultAsync(id);
            var deg = await _designationRepository.FirstOrDefaultAsync(a => a.DepartmentId == id);
            if(deg != null)
            {
                return Ok("DepartmentId Refrence Exists.");
            }
            else
            {
                if (data == null)
                {
                    return NotFound();
                }
                await _departmentRepository.DeleteAsync(id);
                return Ok("Item Deleted");
            }
        }
    }
}
