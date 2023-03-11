﻿using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Contributors;

internal sealed class ClusterConfigureOptionsContributor : ConfigureOptionsContributorBase<ClusterOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Cluster";
}