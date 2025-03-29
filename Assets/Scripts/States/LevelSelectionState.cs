using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionState : IState
{
    LevelSelectionSceneData _sceneData;

    public async UniTask OnEnter()
    {
        if( SceneManager.GetActiveScene().buildIndex != 0 )
        {
            await UnityEngineUtil.LoadSceneWithIndex(0);
        }

        _sceneData = GameObject.FindObjectOfType<LevelSelectionSceneData>();

        int selectetLevel = await _sceneData.LevelSelectionPanel.GetSelectedLevel();

        GameManager.MainStateMachine.SwitchState(new PuzzleState(selectetLevel));
    }

    public void OnUpdate(){}

    public UniTask OnExit()
    {
        return UniTask.CompletedTask;
    }
}