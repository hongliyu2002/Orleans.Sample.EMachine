﻿using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureLoadSheddingOptionsContributor : ConfigureOptionsContributorBase<LoadSheddingOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Server:LoadShedding";
}