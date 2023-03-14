using EMachine.Orleans.Shared.States;
using EMachine.Sales.Orleans.Events;

namespace EMachine.Sales.Orleans.States;

[GenerateSerializer]
public sealed class SnackMachineRepo : Repo
{

    #region Apply

    public void Apply(SnackMachineRepoCreatedEvent evt)
    {
        // Ids.Add(evt.Id);
    }

    public void Apply(SnackMachineRepoDeletedEvent evt)
    {
        // Ids.Remove(evt.Id);
    }

    #endregion

}
