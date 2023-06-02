using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("[controller]")]

public class FileUploadController : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly QuoteService _quoteService;

   public FileUploadController(IWebHostEnvironment webHostEnvironment, QuoteService quoteService) 
   {
    _webHostEnvironment = webHostEnvironment;
    _quoteService = quoteService;
   }
   [HttpGet("GetList")]
   public List<string> GetListOfFiles()
   {
    var list = new List<string>();
    var path = Path.Combine(_webHostEnvironment.WebRootPath, "images");
    var files = Directory.GetFiles(path);
    list.AddRange(files);
    var directories = Directory.GetDirectories(path);
    list.AddRange(directories);
    return list.ToList();
   }

[HttpPost("UploadFile")]
   public string UploadFile([FromForm] AddQuoteDto quote)
   {
    return _quotesService.AddQuote(quote);
   }
}