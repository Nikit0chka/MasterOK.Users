using FastEndpoints;

namespace API.Endpoints.Get;

public sealed record GetRequest([property: RouteParam] int Id);