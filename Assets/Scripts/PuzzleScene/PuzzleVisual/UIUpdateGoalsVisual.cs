using System.Linq;
using System.Threading.Tasks;

public static partial class PuzzlePresentation
{
    public static async Task UIUpdateGoals(PuzzleSceneData sceneData)
    {
        var ui = sceneData.PuzzleUI;
        ui.SetRemainMoves(sceneData.RemainMoves);
        foreach(var goal in sceneData.GoalStateList)
        {
            ui.UpdateGoal(goal);
        }
        await Task.Yield();
    }
}