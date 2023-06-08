using Microsoft.AspNetCore.Http;

namespace Domain.Dtos;

public class QuoteDto
{
    public int Id { get; set; }
    // [Required (ErrorMessage = "Please fill the Author")]
    public string Author { get; set; }
    // [Required (ErrorMessage = "Please fill the Quote Text")]
    public string QuoteText { get; set; }
    // [Required (ErrorMessage = "Please fill the Category Id")]
    public int CategoryId { get; set; }

}


