using System;

internal class QLKSContext
{
    public object Customers { get; internal set; }

    internal void SaveChanges()
    {
        throw new NotImplementedException();
    }
}