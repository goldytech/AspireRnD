namespace GraphQLApi;

public class Query
{
    [GraphQLDescription("Retrieves a single book.")]
    public Book GetBook()
    {
        return new Book
        {
            Title = "The Hitchhiker's Guide to the Galaxy",
            Author = new Author
            {
                Name = "Douglas Adams"
            }
        };
    }
}