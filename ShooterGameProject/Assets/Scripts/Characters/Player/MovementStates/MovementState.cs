public abstract class MovementState : BaseState 
{
    protected PlayerController controller;
    public MovementState(StateMachine stateMachine, PlayerController controller) : base(stateMachine)
    {
        this.controller = controller;
    }
    
}