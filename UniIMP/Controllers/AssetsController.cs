using Microsoft.AspNetCore.Mvc;
using UniIMP.DataAccess;
using UniIMP.DataAccess.Entities;
using UniIMP.DataAccess.Repositories;

namespace UniIMP.Controllers
{
    public class AssetsController : CrudApiController<Asset>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICrudRepository<Asset> _assetRepository;
        private readonly ICrudRepository<AssetTag> _tagRepository;

        public AssetsController(
            ApplicationDbContext dbContext,
            ICrudRepository<Asset> assetRepository, 
            ICrudRepository<AssetTag> tagRepository) 
            : base(assetRepository)
        {
            _dbContext = dbContext;
            _assetRepository = assetRepository;
            _tagRepository = tagRepository;
        }

        [HttpGet("{id?}/tags")]
        public async Task<IActionResult> GetTags(int id)
        {
            var asset = await _assetRepository.GetAsync(id);

            if (asset == null)
                return BadRequest();
            
            await _dbContext.Entry(asset).Collection(a => a.Tags).LoadAsync();

            return Ok(asset.Tags);
        }

        [HttpGet("{id?}/tags/add")]
        public async Task<IActionResult> AddTag(int id, [FromQuery] int tagId)
        {
            var asset = await _assetRepository.GetAsync(id);

            if (asset == null)
                return BadRequest();
            
            var tag = await _tagRepository.GetAsync(tagId);

            if (tag == null)
                return BadRequest();

            asset.Tags.Add(tag);
            await _assetRepository.SaveAsync();

            return Ok();
        }

    }

}