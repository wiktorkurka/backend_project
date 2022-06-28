using Microsoft.AspNetCore.Mvc;
using UniIMP.DataAccess.Entities;
using UniIMP.DataAccess.Repositories;

namespace UniIMP.Controllers.API
{
    public class AssetsController : CrudController<Asset>
    {
        private readonly ICrudRepository<Asset> _assetRepository;
        private readonly ICrudRepository<AssetTag> _tagRepository;

        public AssetsController(
            ICrudRepository<Asset> assetRepository,
            ICrudRepository<AssetTag> tagRepository)
            : base(assetRepository)
        {
            _assetRepository = assetRepository;
            _tagRepository = tagRepository;
        }

        [HttpGet("{id?}/tags")]
        public async Task<IActionResult> GetTags(int id)
        {
            var asset = await _assetRepository.GetAsync(id);

            if (asset == null)
                return BadRequest();

            await _assetRepository.LoadRelatedAsync(asset);

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