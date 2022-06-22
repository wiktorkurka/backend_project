using Microsoft.AspNetCore.Mvc;
using UniIMP.DataAccess.Entities;
using UniIMP.DataAccess.Repositories;

namespace UniIMP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrudApiController<T> : ControllerBase where T : DatabaseEntity
    {
        private readonly ICrudRepository<T> _repository;

        public CrudApiController(ICrudRepository<T> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var asset = await _repository.GetAllAsync();

            return Ok(asset);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var asset = await _repository.GetAsync(id);

            return Ok(asset);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] T model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _repository.CreateAsync(model);
            await _repository.SaveAsync();

            return Ok();
        }

        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] IEnumerable<T> model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    await _repository.CreateAsync(model);
        //    await _repository.SaveAsync();

        //    return Ok();
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] T model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var asset = await _repository.GetAsync(id);

            if (asset == null)
                return BadRequest();

            model.Id = id;

            _repository.Update(model);
            await _repository.SaveAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>? Delete(int id)
        {
            var asset = await _repository.GetAsync(id);

            if (asset == null)
                return BadRequest();

            _repository.Remove(id);
            await _repository.SaveAsync();

            return Ok();
        }
    }
}