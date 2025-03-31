using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class InitState : IState
{
    public UniTask OnEnter(object arg)
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;

        switch (buildIndex)
        {
            case 0:
            GameManager.MainStateMachine.SwitchState(StateEnum.LevelSelectionState);
            break;
            case 1:
            GameManager.MainStateMachine.SwitchState(StateEnum.PuzzleState, 0);
            break;
        }
        return UniTask.CompletedTask;
    }

    public void OnUpdate() { }

    public UniTask OnExit()
    {
        return UniTask.CompletedTask;
    }
}

public class StateMachine : IStateMachine
{
    bool _initialized = false;
    private Dictionary<StateEnum, Type> _stateMap = new Dictionary<StateEnum, Type>();

    public StateMachine()
    {
        CurrentState = new InitState();
    }

    public IState CurrentState { get; private set; }

    public void OnUpdate()
    {
        if (_initialized)
        {
            CurrentState.OnUpdate();
        }
    }

    public void SwitchState(StateEnum to, object arg)
    {
        Func<UniTaskVoid> asyncOp = async () =>
        {
            _initialized = false;
            await CurrentState.OnExit();

            Type stateType = _stateMap[to];
            IState nextState = (IState)Activator.CreateInstance(stateType);
            CurrentState = nextState;

            await CurrentState.OnEnter(arg);
            _initialized = true;
        };
        asyncOp().Forget();
    }

    public async void OnStart()
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(assembly => assembly.GetTypes())
        .Where(type => typeof(IState).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        foreach (StateEnum stateName in Enum.GetValues(typeof(StateEnum)))
        {
            Type t = types.First(t=> t.Name == stateName.ToString());
            _stateMap.Add(stateName, t);
        }

        await CurrentState.OnEnter(null);
        _initialized = true;
    }

    public void OnFinished() { }
}