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
    public class CostumersController : ControllerBase
    {
        private readonly IRepository<Costumer> _repository;

        public CostumersController(IRepository<Costumer> repository)
        {
            _repository = repository;
        }

        // GET: api/Costumers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Costumer>>> GetAllCostumer()
        {
            //return await _costumerRepository.GetAllCostumers();
            return await _repository.GetAll();
        }

        // GET: api/Costumers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Costumer>> GetCostumer(Guid id)
        {
            var costumer = await _repository.GetById(id);

            if (costumer == null)
            {
                return NotFound();
            }

            return costumer;
        }

        //// PUT: api/Costumers/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCostumer(Guid id, Costumer costumer)
        {
            if (id != costumer.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }
            if (await _repository.Exists(id) == false)
            {
                return NotFound();
            }
            await _repository.Update(costumer);

            return NoContent();
        }

        //// POST: api/Costumers
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Costumer>> InsertCostumer(Costumer costumer)
        {
            if (ModelState.IsValid)
            {
                await _repository.Add(costumer);
                return CreatedAtAction("GetCostumer", new { id = costumer.Id }, costumer);
            }

            return BadRequest(); 
        }

        //// DELETE: api/Costumers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCostumer(Guid id)
        {
            var costumer = await _repository.GetById(id);
            if (costumer == null)
            {
                return NotFound();
            }

            await _repository.Remove(costumer);

            return NoContent();
        }
    }
}
