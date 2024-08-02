public abstract class BaseState
{
    protected StateMachine stateMachine;

    public BaseState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }


    public abstract void OnEnter();

    public abstract void Update();

    public virtual void OnExit()
    {

    }
}
