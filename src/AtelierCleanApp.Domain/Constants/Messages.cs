namespace AtelierCleanApp.Domain.Constants;

public static class Messages
{
    public static class ErrorMessages
    {
        public const string UnexpectedDatabaseErrorPlayerID = "Database update error occurred while trying to find player with ID: {PlayerId}.";
        public const string UnexpectedErrorOccurredPlayerID = "Unexpected error occurred while retrieving player with ID: {PlayerId}.";
        public const string UnexpectedErrorOccurredRank = "Uexpected error occurred while retrieving all players sorted by rank";
        public const string UnexpectedErrorOccurredGetAllPlayers = "An unexpected error occurred while retrieving all players.";
        public const string UnexpectedErrorOccurredGetPlayersStatistics = "An unexpected error occurred while retrieving players statistics.";
        public const string UnhandledExceptionGetPlayersStatistics = "An unhandled exception occurred while processing GetPlayerStatistics.";
        public const string UnhandledExceptionGetPlayerID = "Unhandled exception occurred while processing GetPlayerById for ID: {PlayerId}";
        public const string UnhandledExceptionGetAllPlayers = "Unhandled exception occurred while processing GetAllPlayersSortedByRank.";
        public const string UnexpectedError = "Unexpected error occurred. Please try again later.";
        public const string StatisticsComputeError = "An error occurred while computing player statistics.";
        public const string StatisticsApplicationException = "Failed to calculate player statistics";
    }

    public static class WarningMessages
    {
        public const string InvalidPlayerIDRepository = "GetPlayerByIdAsync called with invalid ID: {PlayerId} in repository.";
        public const string InvalidPlayerIDApplication = "Attempted to get player with invalid ID: {PlayerId}";
        public const string InvalidPlayerIDService = "Attempted to get player with invalid ID: {PlayerId} in service.";
        public const string InvalidPlayerIDController = "GetPlayerById called with invalid ID: {PlayerId}. Returning BadRequest.";
        public const string NoPlayerData = "Statistics could not be calculated, possibly no player data. Returning NotFound.";
    }

    public static class InformationMessages
    {
        public const string GetPlayerSuccess = "Player with ID: {PlayerId} successfully retrieved from repository.";
        public const string PlayerNotFound = "Player with ID: {PlayerId} not found in repository.";
        public const string PlayersNotFoundController = "Players not found by service. Returning NotFound.";
        public const string PlayerNotFoundController = "Player with ID: {PlayerId} not found by service. Returning NotFound.";
        public const string GetAllPlayersSortedByRankEndpoint = "API endpoint GetAllPlayersSortedByRank called.";
    }

    public static class BadRequestMessages
    {
        public const string PlayerIDBadRequest = "Player ID must be a positive integer.";
    }

    public static class NotFoundMessages
    {
        public const string PlayerNotFound = "Player not found.";
        public const string PlayersStatisticsNotFound = "Players statistics not found.";
    }

}
