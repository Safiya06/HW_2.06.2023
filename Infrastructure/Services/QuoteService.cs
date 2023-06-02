using Domain.Context;
using Domain.Dtos;

namespace Infrastructure.Services;

public class QuoteService
{
    private readonly DapperContext _context;
    private readonly IFileService _fileService;
    public QuoteService(DapperContext context,IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public List<GetQuoteDto> GetQuotes()
    {
        using(var conn = _context.CreateConnection())
        {
            var sql = "select id,author,quotetext,categorytext,categoryid,filename from quotes";
            return conn.Query<GetQuoteDto>(sql).ToList();
        }
    }

    public GetQuoteDto AddQuote(AddQuoteDto quotes)
    {
         using(var conn = _context.CreateConnection())
        {
            //upload file
            var filename = _fileService.CreateFile("images",quote.File);
            var sql = "insert into quotes (author,quotetext,categorytext,categoryid,filename)values(@author,@quotetext,@categorytext,@categoryid,@filename)returning id";
            var result= conn.ExecuteScalar<int>(sql,new
            {
                quote.Author,
                quote.QuoteText,
                quote.CategoryId,
                filename

            });

            return new GetQuoteDto()
            {
                Author= quote.Author,
               QuoteText= quote.QuoteText,
               CategoryId= quote.CategoryId,
               FileName= filename 
            };
          
        }
    }
}
