using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace DocumentsDBCaller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly ILogger<DocumentsController> _logger;
        private readonly IGetDocument _getDock;

        public DocumentsController(ILogger<DocumentsController> logger, IGetDocument getLot)
        {
            _logger = logger;
            _getDock = getLot ?? throw new ArgumentNullException(nameof(getLot));
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetDocuments()
        {
            var response = await _getDock.GetAllDocuments();

            string output = (string)response.Data;

            return output;
        }

        [HttpGet("{dockId}")]
        public async Task<ActionResult<string>> GetDocument(int dockId)
        {
            var response = await _getDock.GetDocument(dockId);

            string output = (string)response.Data;

            return output;
        }

        [HttpPost()]
        public async Task<ActionResult<HttpStatusCode>> AddDocument(string json)
        {
            Document newDocument = JsonConvert.DeserializeObject<Document>(json) ?? new Document();

            var jsons = newDocument;

            var response = await _getDock.PostDocument(newDocument);

            return response.Status;
        }

        [HttpDelete("{dockId}")]
        public async Task<ActionResult<HttpStatusCode>> DeleteDocument(int dockId)
        {
            var response = await _getDock.RemoveDocument(dockId);

            return response.Status;
        }
    }
}