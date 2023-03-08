// using EMachine.Orleans.Shared.Queries;
// using Fluxera.Guards;
//
// namespace EMachine.Sales.Orleans.Queries;
//
// [Immutable]
// [GenerateSerializer]
// public sealed record SnackMachineReaderListQuery : DomainPagedListQuery
// {
//     public SnackMachineReaderListQuery(int maxResultCount, int skipCount, Guid traceId, DateTimeOffset operatedAt, string operatedBy)
//         : base(traceId, operatedAt, operatedBy)
//     {
//         MaxResultCount = Guard.Against.NegativeOrZero(maxResultCount, nameof(maxResultCount));
//         SkipCount = Guard.Against.Negative(skipCount, nameof(skipCount));
//     }
//
//     [Id(0)]
//     public int MaxResultCount { get; }
//
//     [Id(1)]
//     public int SkipCount { get; }
// }
