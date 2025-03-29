using Cysharp.Threading.Tasks;
using System;

public interface IState
{
    public UniTask OnEnter();
    public void OnUpdate();
    public UniTask OnExit();
}

public interface IStateMachine
{
    public IState CurrentState { get; }
    public void SwitchState( IState to);
    public void OnStart();
    public void OnUpdate();
    public void OnFinished();
}

public class StateMachine : IStateMachine
{
    bool _initialized = false;

    public StateMachine(IState init)
    {
        CurrentState = init;
    }

    public IState CurrentState {get; private set;}

    public void OnUpdate()
    {
        if (_initialized) 
        {
            CurrentState.OnUpdate();
        }
    }

    public void SwitchState( IState to )
    {
        Func<UniTaskVoid> asyncOp = async () => {
            _initialized = false;
            await CurrentState.OnExit();
            CurrentState = to;
            await CurrentState.OnEnter();
            _initialized = true;
        };
        asyncOp().Forget();
    }

    public async void OnStart()
    {
        await CurrentState.OnEnter();
        _initialized = true;
    }

    public void OnFinished() {}
}