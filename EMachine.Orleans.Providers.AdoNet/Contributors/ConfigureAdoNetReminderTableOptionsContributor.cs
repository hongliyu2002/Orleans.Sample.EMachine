﻿using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Providers.AdoNet.Contributors;

internal sealed class ConfigureAdoNetReminderTableOptionsContributor : ConfigureOptionsContributorBase<AdoNetReminderTableOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:AdoNet:ReminderTable";
}
