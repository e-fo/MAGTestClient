using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;

public class PuzzleState : IState
{
    public PuzzleState(int selectedLevel)
    {
        _selectedLevel = selectedLevel;
    }

    private readonly int _selectedLevel;
    private PuzzleSceneData _sceneData;
    private Puzzle _puzzleController;
    IRuleTileTap[] _tapRules = null;

    public async UniTask OnEnter()
    {
        await UnityEngineUtil.LoadSceneWithIndex(1);

        _sceneData = GameObject.FindObjectOfType<PuzzleSceneData>();
        _puzzleController = _sceneData.PuzzleController;

        //init rules
        {
            //finds all types which implmented IRuleTileTap
            Type[] types = null;
            {
                types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IRuleTileTap).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .ToArray();
            }

            //creates instance for all of found rule classes
            _tapRules = new IRuleTileTap[types.Length];
            for (int i = 0; i < types.Length; ++i)
            {
                IRuleTileTap r = (IRuleTileTap)Activator.CreateInstance(types[i]);
                _tapRules[i] = r;
            }
        }

        //init puzzle controller
        {
            var level = _sceneData.LevelList.List[_selectedLevel];
            _puzzleController.InitPuzzle(level);
        }

        //init UI
        {
            _sceneData.PuzzleUI.RestartButton.onClick.AddListener(
                () => GameManager.MainStateMachine.SwitchState(new PuzzleState(_selectedLevel))
            );
            _sceneData.PuzzleUI.LvlSelectionButton.onClick.AddListener(
                () => GameManager.MainStateMachine.SwitchState(new LevelSelectionState())
            );
        }

        RunGame().Forget();
    }

    public void OnUpdate() { }

    public UniTask OnExit()
    {
        return UniTask.CompletedTask;
    }

    private async UniTaskVoid RunGame()
    {
        while (true)
        {
            Vector2Int input = await _puzzleController.GetPlayerInput();

            var typeGrid = PuzzleLogic.GetTypeGrid(_puzzleController.Grid);
            var cnf = _puzzleController.TileConfigs.List.FirstOrDefault(c => c.GetInstanceID() == typeGrid[input.x, input.y]);
            if (null == cnf) return;

            for (int i = 0; i < _tapRules.Length; ++i)
            {
                if (_tapRules[i].AcceptedBaseTypes.Contains(cnf.BaseType))
                    await _tapRules[i].Execute(input, _puzzleController);
            }
        }
    }
}