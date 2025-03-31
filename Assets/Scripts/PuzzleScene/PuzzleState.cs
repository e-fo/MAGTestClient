using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;

public class PuzzleState : IState
{
    private enum GameResult { Win, Lose, Continue }
    private PuzzleSceneData _sceneData;
    private Puzzle _puzzleController;
    IRuleTileTap[] _tapRules = null;
    private int _selectedLevel;

    public async UniTask OnEnter(object arg)
    {
        _selectedLevel = (int)arg;

        await UnityEngineUtil.LoadSceneWithIndex(1);

        _sceneData = GameObject.FindObjectOfType<PuzzleSceneData>();
        _puzzleController = _sceneData.PuzzleController;
        var level = _sceneData.LevelList.List[_selectedLevel];

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
            _puzzleController.InitPuzzle(level);
        }

        //setup goals
        {
            _sceneData.RemainMoves = level.TotalMove;
            var goals = level.Goals;
            _sceneData.GoalStateList = new LevelGoalState[goals.Count];

            for(int x=0; x<goals.Count; ++x) 
            {
                _sceneData.GoalStateList[x] = new LevelGoalState()
                {
                    Config = goals[x],
                    Remain = goals[x].Amount
                };
            }
        }

        //init UI
        {
            _sceneData.PuzzleUI.RestartButton.onClick.AddListener(
                () => GameManager.MainStateMachine.SwitchState(StateEnum.PuzzleState, _selectedLevel)
            );
            _sceneData.PuzzleUI.LvlSelectionButton.onClick.AddListener(
                () => GameManager.MainStateMachine.SwitchState(StateEnum.LevelSelectionState)
            );
            var goals = level.Goals;
            for (int x = 0; x < goals.Count; ++x)
            {
                _sceneData.PuzzleUI.AddGoalToIndicatorPanel(goals[x], _sceneData);
            }
            _sceneData.PuzzleUI.SetRemainMoves(level.TotalMove);
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
        GameResult result = GameResult.Continue;
        while (result == GameResult.Continue)
        {
            Vector2Int input = await _puzzleController.GetPlayerInput();

            var typeGrid = PuzzleLogic.GetTypeGrid(_puzzleController.Grid);
            var cnf = _puzzleController.TileConfigs.List.FirstOrDefault(c => c.GetInstanceID() == typeGrid[input.x, input.y]);
            if (null == cnf) return;

            for (int i = 0; i < _tapRules.Length; ++i)
            {
                if (_tapRules[i].AcceptedBaseTypes.Contains(cnf.BaseType))
                    await _tapRules[i].Execute(input, _sceneData);
            }

            bool areGoalsSatisfied = _sceneData.GoalStateList.All(g=>g.Remain <=0);

            if  ( areGoalsSatisfied && _sceneData.RemainMoves >=0) result = GameResult.Win;
            else if (!areGoalsSatisfied && _sceneData.RemainMoves <=0) result = GameResult.Lose;
        }

        string message = null;
        if(result == GameResult.Win)
        {
            message = "You Win :))\nDo you want to continue?";
        }
        else if (result == GameResult.Lose)
        {
            message = "You Lose :((\nDo you want to continue?";
        }
        
        if (await _sceneData.PuzzleUI.AlertUI.Show(message))
        {
            int level = (result == GameResult.Win) ? _selectedLevel+1: _selectedLevel;
            GameManager.MainStateMachine.SwitchState(StateEnum.PuzzleState, level);
        } else
        {
            GameManager.MainStateMachine.SwitchState(StateEnum.LevelSelectionState);
        }
    }
}