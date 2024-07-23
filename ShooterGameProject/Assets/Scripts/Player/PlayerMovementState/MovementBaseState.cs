public abstract class MovementBaseState 
{
    protected PlayerController Controller;

    public MovementBaseState(PlayerController controller)
    {
        this.Controller = controller;
    }
    public abstract void EnterState();

    public abstract void UpdateState();

    public virtual void ExitState()
    {

    }
}
