using JetBrains.Annotations;

namespace EMachine.Sales.Domain;

[PublicAPI]
public sealed class SnackBaseView
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    #region Create

    public static SnackBaseView Create(Guid id, string name)
    {
        return new SnackBaseView
               {
                   Id = id,
                   Name = name
               };
    }

    #endregion

}
