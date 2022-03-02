using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers
{
    [Authorize(Roles = "Admin,Owner", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ApointmentsController : ControllerBase
    {
        private readonly IRepository<Apointment> _repository;

        public ApointmentsController(IRepository<Apointment> repository)
        {
            _repository = repository;
        }

        // GET: api/Apointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Apointment>>> GetAllApointment()
        {
            return await _repository.GetAll();
        }

        // GET: api/Apointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Apointment>> GetApointment(Guid id)
        {
            var Apointment = await _repository.GetById(id);

            if (Apointment == null)
            {
                return NotFound();
            }

            return Apointment;
        }

        //// PUT: api/Apointments/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> EditApointment(Guid id, Apointment apointment)
        {
            if (id != apointment.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }
            if (await _repository.Exists(id) == false)
            {
                return NotFound();
            }
            await _repository.Update(apointment);

            return NoContent();
        }

        //// POST: api/Apointments
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Apointment>> InsertApointment(Apointment apointment)
        {
            if (ModelState.IsValid)
            {
                await _repository.Add(apointment);
                return CreatedAtAction("GetApointment", new { id = apointment.Id }, apointment);
            }

            return BadRequest();
        }

        //// DELETE: api/Apointments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveApointment(Guid id)
        {
            var Apointment = await _repository.GetById(id);
            if (Apointment == null)
            {
                return NotFound();
            }

            await _repository.Remove(Apointment);

            return NoContent();
        }
    }
}
