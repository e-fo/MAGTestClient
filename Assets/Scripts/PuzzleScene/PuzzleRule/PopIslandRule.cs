using System.Linq;
using UnityEngine;

public struct PopIslandRule: IRuleTileTap
{
    public TileBaseType[] AcceptedBaseTypes => new TileBaseType[]{TileBaseType.Simple};

    public async void Execute(Vector2Int position, Puzzle puzzle)
    {
        var tile = puzzle.Table[position.x, position.y];
        var cnf = puzzle.TileConfigs.First(c=>c.GetInstanceID() == tile.SOEnumTypeInstanceId);

        var go = puzzle.TilesRefComponents[tile.GameObjectInstanceId];

        var islandMap = PuzzleLogic.GetIslandMap(puzzle.Table, position);
        int islandLength = ArrayUtil.CountNotEqual2D(islandMap, TileStateValue.Empty.GameObjectInstanceId);

        var orderedGenReqs = cnf.GenerationReqs.OrderBy(x=>x.NumberOfRequiredItem).ToArray();
        
        if(islandLength == 1)
        {
            await PuzzlePresentation.TileShakeVisual(puzzle, position);
        }
        else if(islandLength > 1 && islandLength < orderedGenReqs[0].NumberOfRequiredItem)
        {
            await PuzzlePresentation.BatchDestroyVisual(puzzle, islandMap);
            PuzzleLogic.DestroyTileBatch(puzzle, islandMap);

            var dropMap = PuzzleLogic.CalculateTilesDrop(PuzzleLogic.GetIdGrid(puzzle.Table));
            await PuzzlePresentation.TileDropVisual(puzzle, dropMap);
            PuzzleLogic.ApplyTilesDrop(ref puzzle.Table, dropMap);

            var refillTypeMap = PuzzleLogic.GenerateRefillMap(
                PuzzleLogic.GetIdGrid(puzzle.Table),
                puzzle.TileConfigs.Select(c => c.GetInstanceID()).ToArray());
            var instantiateMap = PuzzleLogic.InstantiateTileBatch(puzzle, refillTypeMap);
            await PuzzlePresentation.RefillDropVisual(puzzle, instantiateMap);
        }
        else if(islandLength > orderedGenReqs[0].NumberOfRequiredItem)
        {

        }
    }
}