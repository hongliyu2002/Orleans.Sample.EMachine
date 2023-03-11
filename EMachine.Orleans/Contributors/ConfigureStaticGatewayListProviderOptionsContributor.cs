﻿using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Contributors;

internal sealed class ConfigureStaticGatewayListProviderOptionsContributor : ConfigureOptionsContributorBase<StaticGatewayListProviderOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:StaticGatewayListProvider";
}
