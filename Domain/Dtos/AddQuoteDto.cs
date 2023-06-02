using Domain.Dtos;
using Microsoft.AspNetCore.Http;

public class AddQuoteDto : QuoteDto
{
    public IFormFile? File { get; set; }

}