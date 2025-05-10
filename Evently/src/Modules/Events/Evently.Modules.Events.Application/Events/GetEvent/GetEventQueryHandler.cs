using System.Data.Common;
using Dapper;
using Evently.Modules.Events.Application.Abstractions.Data;
using MediatR;

namespace Evently.Modules.Events.Application.Events.GetEvent;

internal sealed class GetEventQueryHandler(IDbConnectionFactory dbConnectionFactory) : IRequestHandler<GetEventQuery, EventResponse?>
{
    public async Task<EventResponse?> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
                SELECT
                    "Id" AS {nameof(EventResponse.Id)},
                    "Title" AS {nameof(EventResponse.Title)},
                    "Description" AS {nameof(EventResponse.Description)},
                    "Location" AS {nameof(EventResponse.Location)},
                    "StartsAtUtc" AS {nameof(EventResponse.StartsAtUtc)},
                    "EndsAtUtc" AS {nameof(EventResponse.EndsAtUtc)}
                FROM events."Events"
                WHERE "Id" = @EventId
            """;

        EventResponse? @event = await connection.QuerySingleOrDefaultAsync<EventResponse>
        (
            sql,
            request
        );

        return @event;
    }
}
