public class StateMachine
{
    public BaseState CurrentState { get; private set; }

    public void SetState(BaseState newState)
    {
        CurrentState?.OnExit();
        CurrentState = newState;
        CurrentState?.OnEnter();
    }

    public void Update()
    {
        CurrentState.Update();
    }
}
