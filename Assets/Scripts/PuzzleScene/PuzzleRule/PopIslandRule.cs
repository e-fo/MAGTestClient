using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

public struct PopIslandRule: IRuleTileTap
{
    public readonly TileBaseType[] AcceptedBaseTypes => new TileBaseType[]{TileBaseType.Simple};

    public async UniTask Execute(Vector2Int position, Puzzle puzzle)
    {
        var tile = puzzle.Grid[position.x, position.y];
        var grid = puzzle.Grid;
        int typeDefault = TileStateValue.Empty.SOEnumTypeInstanceId;

        var islandMap = ArrayUtil.GetIslandMap(PuzzleLogic.GetTypeGrid(grid), 
            position, 
            typeDefault);
        ArrayUtil.ReplaceNotDefaultElements(ref islandMap, 
            PuzzleLogic.GetIdGrid(grid), 
            typeDefault);
        int islandLength = ArrayUtil.CountNotEqual2D(islandMap, TileStateValue.Empty.GameObjectInstanceId);


        var cnf = puzzle.TileConfigs.List.First(c=>c.GetInstanceID() == tile.SOEnumTypeInstanceId);
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
            
            var tuple = puzzle.InstantiateTile(shouldGenerateConf, position);

            await PuzzlePresentation.AbsorbToNewTile(puzzle, islandMap, tuple.Item2);

            PuzzleLogic.DestroyTileBatch(puzzle, islandMap);

            puzzle.Grid[position.x, position.y] = tuple.Item1;
            puzzle.TilesRefComponents.Add(tuple.Item1.GameObjectInstanceId, tuple.Item2);

            await ReusableRule.DropRule(puzzle, position);
            await ReusableRule.Refill(puzzle, position);
        }
    }
}