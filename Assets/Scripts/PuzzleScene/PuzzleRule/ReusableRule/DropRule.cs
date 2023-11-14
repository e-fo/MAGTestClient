using System.Threading.Tasks;
using UnityEngine;

public static partial class ReusableRule
{
    public async static Task DropRule(Puzzle puzzle, Vector2Int tapPos)
    {
        var dropMap = PuzzleLogic.CalculateTilesDrop(PuzzleLogic.GetIdGrid(puzzle.Grid));
        await PuzzlePresentation.TileDropVisual(puzzle, dropMap);
        PuzzleLogic.ApplyTilesDrop(ref puzzle.Grid, dropMap);
    }
}