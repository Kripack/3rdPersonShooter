public abstract class MovementState : BaseState 
{
    protected PlayerController Controller;
    public MovementState(StateMachine stateMachine, PlayerController controller) : base(stateMachine)
    {
        this.Controller = controller;
    }
    
}