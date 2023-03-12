﻿using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Providers.AdoNet.Contributors;

internal sealed class ConfigureAdoNetGrainStorageOptionsContributor : ConfigureOptionsContributorBase<AdoNetGrainStorageOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:AdoNet:GrainStorage";
}