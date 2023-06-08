using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services;

namespace MvcApp.Controllers;

public class QuoteController : Controller
{
    private readonly QuoteService _quoteservice;
    public QuoteController(QuoteService quoteService)
    {
        _quoteservice = quoteService;
    }

    public async Task<IActionResult> Index()
    {
        var quotes = await _quoteservice.GetQuotes();
        return View(quotes);
    }
    [HttpGet]
    public IActionResult Create()
    {
       return View(new AddQuoteDto());
    }

    [HttpPost]
    public  async Task<IActionResult> Create(AddQuoteDto quote)
    {
        if(ModelState.IsValid)
        {
          await   _quoteservice.AddQuote(quote);
            return RedirectToAction("Index");

        }       return View();
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var existing = await _quoteservice.GetQuoteById(id);
        var addquote = new AddQuoteDto()
        {
            Id = existing.Id,
            Author = existing.Author,
            CategoryId = existing.CategoryId,
            QuoteText = existing.QuoteText
        };
       return View(addquote);
    }

    [HttpPost]
    public async Task<IActionResult> Update(AddQuoteDto quote)
    {
        if(ModelState.IsValid)
        {
           await  _quoteservice.UpdateQuote(quote);
            return RedirectToAction("Index");

        }       return View();
    }
}
