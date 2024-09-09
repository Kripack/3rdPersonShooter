public abstract class PlayerBaseState : BaseState 
{
    protected readonly PlayerController controller;
    protected readonly PlayerStateMachine stateMachine;
    public PlayerBaseState(PlayerStateMachine stateMachine, PlayerController controller)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
    }
    
}