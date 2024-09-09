public class PlayerStateMachine : StateMachine
{
    public IdleState IdleState { get; private set; }
    public WalkingState WalkingState { get; private set; }
    public RunningState RunningState { get; private set; }
    public CrouchingState CrouchingState { get; private set; }
    public InAirState InAirState { get; private set; }

    public void InitStates(PlayerController controller)
    {
        IdleState = new IdleState(this, controller);
        WalkingState = new WalkingState(this, controller);
        RunningState = new RunningState(this, controller);
        CrouchingState = new CrouchingState(this, controller);
        InAirState = new InAirState(this, controller);
    }
}