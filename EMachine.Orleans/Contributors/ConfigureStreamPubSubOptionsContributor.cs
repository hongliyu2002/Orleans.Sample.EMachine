﻿using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Contributors;

internal sealed class ConfigureStreamPubSubOptionsContributor : ConfigureOptionsContributorBase<StreamPubSubOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:StreamPubSub";
}