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
    public class DesignationController : ControllerBase
    {
        private readonly IRepository<Designation> _designationRepository;

        public DesignationController(IRepository<Designation> designationRepository)
        {
            _designationRepository = designationRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _designationRepository.GetAllListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _designationRepository.FirstOrDefaultAsync(a => a.Id == id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Designation designation)
        {
            _designationRepository.Add(designation);
            await _designationRepository.SaveChangesAsync();
            return Ok(designation);
        }

        [HttpPut]
        public async Task<IActionResult> Put(DesignationDtos input)
        {
            var data = await _designationRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (data == null)
            {
                return NotFound();
            }
            
            data.Name = input.Name;
            data.DepartmentId = input.DepartmentId;
            data.Description = input.Description;
            await _designationRepository.UpdateAsync(data);
            return Ok(data);
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _designationRepository.FirstOrDefaultAsync(id);

            if (data == null)
            {
                return NotFound();
            }
            await  _designationRepository.DeleteAsync(id);
            return Ok("Item Deleted");
        }
    }
}
