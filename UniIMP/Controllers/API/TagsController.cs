﻿using Microsoft.AspNetCore.Mvc;
using UniIMP.DataAccess.Entities;
using UniIMP.DataAccess.Repositories;

namespace UniIMP.Controllers.API
{
    public class TagsController : CrudController<AssetTag>
    {
        private readonly ICrudRepository<Asset> _assetRepository;
        private readonly ICrudRepository<AssetTag> _tagRepository;

        public TagsController(
            ICrudRepository<AssetTag> tagRepository,
            ICrudRepository<Asset> assetRepository)
            : base(tagRepository)
        {
            _tagRepository = tagRepository;
            _assetRepository = assetRepository;
        }

        [HttpGet("{id?}/assets")]
        public async Task<IActionResult> GetTags(int id)
        {
            var tag = await _tagRepository.GetAsync(id);

            if (tag == null)
                return BadRequest();

            await _tagRepository.LoadRelatedAsync(tag);

            return Ok(tag.Assets);
        }

        [HttpGet("{id?}/assets/add")]
        public async Task<IActionResult> AddTag(int id, [FromQuery] int tagId)
        {
            var tag = await _tagRepository.GetAsync(tagId);

            if (tag == null)
                return BadRequest();

            var asset = await _assetRepository.GetAsync(id);

            if (asset == null)
                return BadRequest();

            tag.Assets.Add(asset);
            await _tagRepository.SaveAsync();

            return Ok();
        }
    }
}