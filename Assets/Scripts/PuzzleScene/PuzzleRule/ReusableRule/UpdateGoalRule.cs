using System.Threading.Tasks;

public static partial class ReusableRule
{
    public async static Task UpdateGoalRule(PuzzleSceneData sceneData, int[,] destroyMap)
    {
        PuzzleLogic.UpdateGoals(sceneData, destroyMap);
        await PuzzlePresentation.UIUpdateGoals(sceneData);
    }
}