using Dell_AdminWeb_API.Models;
using Dell_AdminWeb_API.Models.PostBodies;
using Microsoft.AspNetCore.Mvc;
using Persistence.Data.Contracts;
using Persistence.Data.Models;

namespace Dell_AdminWeb_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeywordsController : ControllerBase
    {
        private readonly IKeywordRepository _keywordRepo;
        private readonly IKeywordDocumentMappingRepository _keywordDocumentMappingRepo;

        public KeywordsController(IKeywordRepository keywordRepo, IKeywordDocumentMappingRepository keywordDocumentMappingRepo)
        {
            _keywordRepo = keywordRepo;
            _keywordDocumentMappingRepo = keywordDocumentMappingRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IList<KeywordDTO>>> GetAll()
        {
            var keywords = await _keywordRepo.GetAllAsync();
            return Ok(keywords.Select(x => ConvertToDTO(x)).ToList());
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] KeywordPostBody newKeyword)
        {
            await _keywordRepo.AddAsync(new Keyword
            {
                Value = newKeyword.Value,
                LastModified = DateTime.Now
            });
            return Ok(true);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] KeywordDTO keyword)
        {
            await _keywordRepo. UpdateAsync(new Keyword
            {
                Id = keyword.Id,
                Value = keyword.Value,
                LastModified = DateTime.Now
            });
            return Ok(true);
        }

        [HttpDelete("{keywordId}")]
        public async Task<ActionResult> Delete(int keywordId)
        {
            await _keywordRepo.DeleteAsync(keywordId);
            return Ok(true);
        }

        [HttpGet("{keywordId}/mappings")]
        public async Task<ActionResult<IList<Document>>> GetMappings(int keywordId)
        {
            return Ok(await _keywordDocumentMappingRepo.GetDocumentsByKeywordIdAsync(keywordId));
        }

        [HttpPut("{keywordId}/mappings")]
        public async Task<ActionResult> UpdateMappings(int keywordId, [FromBody] MappingPostBody mappings)
        {
            var currentMappings = await _keywordDocumentMappingRepo.GetAllByKeywordIdAsync(keywordId);

            var toAdd = mappings.documentIds.Where(d => !currentMappings.Exists(m => m.DocumentId == d)).ToList();
            var toDelete = currentMappings.Where(m => !mappings.documentIds.Contains(m.DocumentId))
                            .Select(m => m.DocumentId).ToList();

            var tasks = new Task[]
            {
                _keywordDocumentMappingRepo.AddRangeAsync(keywordId, toAdd),
                _keywordDocumentMappingRepo.DeleteRangeAsync(keywordId, toDelete)
            };

            Task.WaitAll(tasks);
            return Ok(true);
        }

        private static KeywordDTO ConvertToDTO(Keyword keyword) =>
            new KeywordDTO
            {
                Id = keyword.Id,
                Value = keyword.Value
            };

    }
}