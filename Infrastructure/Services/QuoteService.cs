using Domain.Context;
using Domain.Dtos;
using Dapper;
namespace Infrastructure.Services;

public class QuoteService
{
    private readonly DapperContext _context;
    private readonly IFileService _fileService;
    public QuoteService(DapperContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<List<GetQuoteDto>> GetQuotes()
    {
        using (var conn = _context.CreateConnection())
        {
            var sql = "select id,author,quotetext,categoryid,filename from quotes order by id";
            return (await conn.QueryAsync<GetQuoteDto>(sql)).ToList();
        }
    }



    public async Task<GetQuoteDto> GetQuoteById(int id)
    {
        using (var conn = _context.CreateConnection())
        {
            var sql = "select id,author,quotetext,categoryid,filename from quotes where id = @id";
            return (await conn.QuerySingleOrDefaultAsync<GetQuoteDto>(sql, new { id }));
        }
    }

    public async Task<GetQuoteDto> AddQuote(AddQuoteDto quote)
    {
        using (var conn = _context.CreateConnection())
        {
            string fileName = null;
            int id = 0;
            //upload file
            if (quote.File != null)
            {
                fileName = await _fileService.CreateFile("images", quote.File);
                var sql = "insert into quotes (author,quotetext,categoryid,filename)values(@author,@quotetext,@categoryid,@filename)returning id";
                id = await conn.ExecuteScalarAsync<int>(sql, new
                {
                    quote.Author,
                    quote.QuoteText,
                    quote.CategoryId,
                    fileName

                });
            }

            else {

                var sql = "insert into quotes (author,quotetext,categoryid)values(@author,@quotetext,@categoryid) returning id";
                id = await conn.ExecuteScalarAsync<int>(sql,quote);
            }


            return new GetQuoteDto()
            {
                Author = quote.Author,
                QuoteText = quote.QuoteText,
                CategoryId = quote.CategoryId,
                FileName = fileName,
                Id = id
            };

        }
    }

    public async Task<GetQuoteDto> UpdateQuote(AddQuoteDto quote)
    {
        using (var conn = _context.CreateConnection())
        {
            var existing = await conn.QuerySingleOrDefaultAsync<GetQuoteDto>($"Select id as Id,author as Author,quotetext as QuoteText,categoryid as CategoryId,filename as FileName from quotes where id = @Id;", new { quote.Id });
            if (existing == null)
            {
                return new GetQuoteDto();
            }
            string filename = null;
            if (quote.File != null && existing.FileName != null)
            {
                _fileService.DeleteFile("images", existing.FileName);
                filename = await _fileService.CreateFile("images", quote.File);
            }
            else if (quote.File != null && existing.FileName == null)
            {
                filename = await _fileService.CreateFile("images", quote.File);
            }
            var sql = "update quotes set author = @Author,quotetext =@QuoteText,categoryid =@CategoryId,filename = @FileName where id = @Id";

            if (quote.File != null)
            {
                sql = "update quotes set author = @Author,quotetext =@QuoteText,categoryid =@CategoryId,filename = @FileName where id = @Id";
            }
            var result = conn.Execute(sql, new
            {
                quote.Author,
                quote.QuoteText,
                quote.CategoryId,
                filename,
                quote.Id
            });
            return new GetQuoteDto()
            {
                Author = quote.Author,
                QuoteText = quote.QuoteText,
                CategoryId = quote.CategoryId,
                FileName = filename,
                Id = result
            };
        }
    }

}
