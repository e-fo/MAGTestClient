using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

public struct PopIslandRule: IRuleTileTap
{
    public readonly TileBaseType[] AcceptedBaseTypes => new TileBaseType[]{TileBaseType.Simple};

    public async UniTask Execute(Vector2Int position, PuzzleSceneData sceneData)
    {
        var tile = sceneData.PuzzleController.Grid[position.x, position.y];
        var grid = sceneData.PuzzleController.Grid;
        int typeDefault = TileStateValue.Empty.SOEnumTypeInstanceId;

        var islandMap = ArrayUtil.GetIslandMap(PuzzleLogic.GetTypeGrid(grid), 
            position, 
            typeDefault);
        ArrayUtil.ReplaceNotDefaultElements(ref islandMap, 
            PuzzleLogic.GetIdGrid(grid), 
            typeDefault);
        int islandLength = ArrayUtil.CountNotEqual2D(islandMap, TileStateValue.Empty.GameObjectInstanceId);


        var cnf = sceneData.PuzzleController.TileConfigs.List.First(c=>c.GetInstanceID() == tile.SOEnumTypeInstanceId);
        var orderedGenReqs = cnf.GenerationReqs.OrderBy(x=>x.NumberOfRequiredItem).ToArray();
        
        if(islandLength == 1)
        {
            await PuzzlePresentation.TileShakeVisual(sceneData.PuzzleController, position);
        }
        else if(
            islandLength > 1 && 
            (orderedGenReqs.Length == 0 || islandLength < orderedGenReqs[0].NumberOfRequiredItem))
        {
            await ReusableRule.UpdateGoalRule(sceneData, islandMap);
            await PuzzlePresentation.BatchDestroyVisual(sceneData.PuzzleController, islandMap);
            PuzzleLogic.DestroyTileBatch(sceneData.PuzzleController, islandMap);


            await ReusableRule.DropRule(sceneData.PuzzleController, position);
            await ReusableRule.Refill(sceneData.PuzzleController, position);
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
            
            var tuple = sceneData.PuzzleController.InstantiateTile(shouldGenerateConf, position);

            await PuzzlePresentation.AbsorbToNewTile(sceneData.PuzzleController, islandMap, tuple.Item2);
            await ReusableRule.UpdateGoalRule(sceneData, islandMap);
            PuzzleLogic.DestroyTileBatch(sceneData.PuzzleController, islandMap);

            sceneData.PuzzleController.Grid[position.x, position.y] = tuple.Item1;
            sceneData.PuzzleController.TilesRefComponents.Add(tuple.Item1.GameObjectInstanceId, tuple.Item2);

            await ReusableRule.DropRule(sceneData.PuzzleController, position);
            await ReusableRule.Refill(sceneData.PuzzleController, position);
        }
    }
}