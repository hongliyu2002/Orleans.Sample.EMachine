// using EMachine.Orleans.Shared.Queries;
// using Fluxera.Guards;
//
// namespace EMachine.Sales.Orleans.Queries;
//
// [Immutable]
// [GenerateSerializer]
// public sealed record SnackReaderListQuery : DomainPagedListQuery
// {
//     public SnackReaderListQuery(int skipCount, int maxResultCount, Guid traceId, DateTimeOffset operatedAt, string operatedBy)
//         : base(traceId, operatedAt, operatedBy)
//     {
//         SkipCount = Guard.Against.Negative(skipCount, nameof(skipCount));
//         MaxResultCount = Guard.Against.NegativeOrZero(maxResultCount, nameof(maxResultCount));
//     }
//
//     [Id(0)]
//     public int SkipCount { get; }
//
//     [Id(1)]
//     public int MaxResultCount { get; }
// }
