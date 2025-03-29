public interface IStateMachine
{
    public IState CurrentState { get; }
    public void SwitchState(StateEnum to, object arg = null);
    public void OnStart();
    public void OnUpdate();
    public void OnFinished();
}