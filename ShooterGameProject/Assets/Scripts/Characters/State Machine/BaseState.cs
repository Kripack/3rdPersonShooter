public abstract class BaseState
{
    protected StateMachine StateMachine;

    public BaseState(StateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }


    public abstract void OnEnter();

    public abstract void Update();

    public virtual void OnExit()
    {

    }
}
