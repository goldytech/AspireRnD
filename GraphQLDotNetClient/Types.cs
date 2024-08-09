namespace GraphQLDotNetClient;

public record AlphaBroderCategory(
    string Description,
    string Id,
    DateTime LastModifiedAt,
    string Name
);

public record AlphaBroderProduct(
    AlphaBroderCategory AlphaBroderCategory,
    string Description,
    string Name,
    decimal Price
);

public record AlphaBroderProductsResponse(
    List<AlphaBroderProduct> Nodes
);

public record Data(
    AlphaBroderProductsResponse AlphaBroderProducts
);

public record GraphQLResponse(
    Data Data
);