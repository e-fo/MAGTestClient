using System.Linq;

public static partial class PuzzleLogic
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    /// <param name="destroyMap">each elements contains instanceId of should be destroyed tile</param>
    /// <returns></returns>
    public static void UpdateGoals(PuzzleSceneData sceneData, int[,] destroyMap)
    {
        int rows = destroyMap.GetLength(0);
        int cols = destroyMap.GetLength(1);
        var confList = sceneData.PuzzleController.TileConfigs.List;
        var goals = sceneData.GoalStateList;

        sceneData.RemainMoves --;

        static void updateGoals(LevelGoalState[] goals, in TileConfig destroyedTile)
        {
            var c = destroyedTile;
            for(int x=0; x<goals.Length; ++x) 
            {
                var g = goals[x];

                if( g.Remain> 0)
                {
                         if(g.Config.GoalType == LevelGoal.Type.TypeLess)                                       g.Remain--;
                    else if(g.Config.GoalType == LevelGoal.Type.BaseType && g.Config.TileBaseType == c.BaseType)g.Remain--;
                    else if(g.Config.GoalType == LevelGoal.Type.SpecificType && g.Config.TileConfig == c)       g.Remain--;
                }
            }
        }

        for(int i=0; i<rows; ++i)
            for(int j=0; j<cols; ++j)
            {
                int instanceId = destroyMap[i, j];
                if(instanceId != TileStateValue.Empty.GameObjectInstanceId)
                {
                    int confInstanceId = sceneData.PuzzleController.Grid[i, j].SOEnumTypeInstanceId;
                    var conf = confList.First(c=>c.GetInstanceID() == confInstanceId);
                    
                    updateGoals(goals, conf);
                }
            }
    }
}