using System;

public class CollectiblesInvoker
{
    public event Action ItemPickedUp;

    public void PickUpItem()
    {
        ItemPickedUp?.Invoke();
    }
}