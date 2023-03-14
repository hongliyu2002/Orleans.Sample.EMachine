using EMachine.Orleans.Shared.States;
using EMachine.Sales.Orleans.Events;

namespace EMachine.Sales.Orleans.States;

[GenerateSerializer]
public sealed class SnackRepo : Repo
{

    #region Apply

    public void Apply(SnackRepoCreatedEvent evt)
    {
        Ids.Add(evt.Id);
    }

    public void Apply(SnackRepoDeletedEvent evt)
    {
        Ids.Remove(evt.Id);
    }

    #endregion

}
