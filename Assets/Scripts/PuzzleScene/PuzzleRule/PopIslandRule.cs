using System.Linq;
using UnityEngine;

public struct PopIslandRule: IRuleTileTap
{
    public TileBaseType[] AcceptedBaseTypes => new TileBaseType[]{TileBaseType.Simple};

    public async void Execute(Vector2Int position, Puzzle puzzle)
    {
        puzzle.InputAvailable = false;

        var tile = puzzle.Grid[position.x, position.y];
        var cnf = puzzle.TileConfigs.First(c=>c.GetInstanceID() == tile.SOEnumTypeInstanceId);

        var go = puzzle.TilesRefComponents[tile.GameObjectInstanceId];

        var islandMap = PuzzleLogic.GetIslandMap(puzzle.Grid, position);
        int islandLength = ArrayUtil.CountNotEqual2D(islandMap, TileStateValue.Empty.GameObjectInstanceId);

        var orderedGenReqs = cnf.GenerationReqs.OrderBy(x=>x.NumberOfRequiredItem).ToArray();
        
        if(islandLength == 1)
        {
            await PuzzlePresentation.TileShakeVisual(puzzle, position);
        }
        else if(
            islandLength > 1 && 
            (orderedGenReqs.Length == 0 || islandLength < orderedGenReqs[0].NumberOfRequiredItem))
        {
            await PuzzlePresentation.BatchDestroyVisual(puzzle, islandMap);
            PuzzleLogic.DestroyTileBatch(puzzle, islandMap);


            await ReusableRule.DropRule(puzzle, position);

            await ReusableRule.Refill(puzzle, position);
        }
        else if(islandLength >= orderedGenReqs[0].NumberOfRequiredItem)
        {
            TileConfig shouldGenerateConf = null;
            for(int x=0; x<orderedGenReqs.Length; ++x) 
            {
                if(islandLength >= orderedGenReqs[x].NumberOfRequiredItem)
                {
                    shouldGenerateConf = orderedGenReqs[x].GeneratedType;
                }
            }

            var tuple = PuzzleLogic.InstantiateTile(
                puzzle.Prefab, 
                shouldGenerateConf,
                puzzle.transform,
                position,
                puzzle.InputHandler);


            await PuzzlePresentation.AbsorbToNewTileVisual(puzzle, islandMap, tuple.Item2);

            PuzzleLogic.DestroyTileBatch(puzzle, islandMap);

            puzzle.Grid[position.x, position.y] = tuple.Item1;
            puzzle.TilesRefComponents.Add(tuple.Item1.GameObjectInstanceId, tuple.Item2);

            await ReusableRule.DropRule(puzzle, position);

            await ReusableRule.Refill(puzzle, position);
        }
        
        puzzle.InputAvailable = true;
    }
}