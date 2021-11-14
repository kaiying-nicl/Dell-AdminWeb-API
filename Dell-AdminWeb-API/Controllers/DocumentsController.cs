using Microsoft.AspNetCore.Mvc;
using Persistence.Data.Contracts;
using Persistence.Data.Models;

namespace Dell_AdminWeb_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepo;

        public DocumentsController(IDocumentRepository documentRepo)
        {
            _documentRepo = documentRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Document>>> GetAll()
        {
            return Ok(await _documentRepo.GetAllAsync());
        }
    }
}
