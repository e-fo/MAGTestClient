using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class InitState : IState
{
    public UniTask OnEnter()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        
        switch(buildIndex)
        {
            case 0:
                GameManager.MainStateMachine.SwitchState(new LevelSelectionState());
                break;
            case 1:
                GameManager.MainStateMachine.SwitchState(new PuzzleState(0));
                break;
        }
        return UniTask.CompletedTask;
    }

    public void OnUpdate() {}

    public UniTask OnExit()
    {
        return UniTask.CompletedTask;
    }
}