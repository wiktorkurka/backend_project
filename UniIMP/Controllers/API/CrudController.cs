using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniIMP.DataAccess.Repositories;

namespace UniIMP.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrudController<T> : ControllerBase where T : class
    {
        private readonly ICrudRepository<T> _repository;

        public CrudController(ICrudRepository<T> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25,
            [FromQuery] bool loadRelated = false)
        {
            IQueryable<T> queryable = _repository.GetQueryable();
            List<T> entities = queryable.Skip(page * pageSize).Take(pageSize).ToList();

            if (loadRelated)
                foreach (var entity in entities)
                    await _repository.LoadRelatedAsync(entity);

            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery] bool loadRelated = false)
        {
            var entity = await _repository.GetAsync(id);

            if (loadRelated && entity != null)
                await _repository.LoadRelatedAsync(entity);

            return Ok(entity);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] T model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var entity = await _repository.GetAsync(id);

            if (entity == null)
                return BadRequest();

            try
            {
                Type type = model.GetType();
                PropertyInfo? idProp = type.GetProperty("Id");
                if (idProp != null)
                {
                    idProp.SetValue(model, id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            _repository.Update(model);
            await _repository.SaveAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>? Delete(int id)
        {
            var entity = await _repository.GetAsync(id);

            if (entity == null)
                return BadRequest();

            _repository.Remove(id);
            await _repository.SaveAsync();

            return Ok();
        }
    }
}