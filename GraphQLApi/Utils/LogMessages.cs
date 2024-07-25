namespace GraphQLApi.Utils;

public static partial class LogMessages
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = "Command: {commandName} - Json: {commandJson}")]
    public static partial void LogMongoDbCommand(this ILogger logger, string commandName, string commandJson);
    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "Database seeding successfully Completed")]
    public static partial void LogDatabaseSeedingSuccess(this ILogger logger);
    [LoggerMessage(EventId = 3, Level = LogLevel.Error, Message = "An error occurred while {operation} the database: {exception}")]
    public static partial void LogDatabaseSeedingError(this ILogger logger, string operation, Exception exception);
}

public class MongoDbLoggerCategory
{
}