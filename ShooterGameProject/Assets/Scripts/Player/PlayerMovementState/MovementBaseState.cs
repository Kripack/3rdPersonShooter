public abstract class MovementBaseState 
{
    protected PlayerController controller;

    public MovementBaseState(PlayerController controller)
    {
        this.controller = controller;
    }
    public abstract void EnterState();

    public abstract void UpdateState();

    public virtual void ExitState()
    {

    }
}
