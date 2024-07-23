namespace GraphQLApi;

[GraphQLDescription("The book entity.")]
public class Book
{
    [GraphQLDescription("The title of the book.")]
    public required string Title { get; set; }

    [GraphQLDescription("The author of the book.")]
    public required Author Author { get; set; }
}

[GraphQLDescription("The author entity.")]
public class Author
{
    [GraphQLDescription("The name of the author.")]
    public required string Name { get; set; }
}