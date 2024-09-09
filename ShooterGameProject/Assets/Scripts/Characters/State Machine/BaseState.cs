public abstract class BaseState
{
    public abstract void OnEnter();

    public abstract void Update();

    public virtual void OnExit()
    {

    }
}
