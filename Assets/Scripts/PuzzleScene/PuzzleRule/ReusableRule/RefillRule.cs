using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public static partial class ReusableRule
{
    public async static Task Refill(Puzzle puzzle, Vector2Int tapPos)
    {
        var refillTypeMap = PuzzleLogic.GenerateRefillMap(
            PuzzleLogic.GetIdGrid(puzzle.Grid),
            puzzle.TileConfigs.List.Select(c => c.GetInstanceID()).ToArray());

        var instantiateMap = PuzzleLogic.InstantiateTileBatch(puzzle, refillTypeMap);
        await PuzzlePresentation.RefillDropVisual(puzzle, instantiateMap);
    }
}