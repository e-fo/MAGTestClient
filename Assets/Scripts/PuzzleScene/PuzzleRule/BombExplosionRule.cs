using Cysharp.Threading.Tasks;
using UnityEngine;

public struct BombExplosionRule : IRuleTileTap
{
    public readonly TileBaseType[] AcceptedBaseTypes => new TileBaseType[]{ TileBaseType.Bomb };

    public async UniTask Execute(Vector2Int position, PuzzleSceneData sceneData)
    {
        var grid = sceneData.PuzzleController.Grid;
        int typeDefault = TileStateValue.Empty.SOEnumTypeInstanceId;

        var islandMap = ArrayUtil.GetIslandMap(PuzzleLogic.GetTypeGrid(grid), 
            position, 
            typeDefault);
        ArrayUtil.ReplaceNotDefaultElements(ref islandMap, 
            PuzzleLogic.GetIdGrid(grid), 
            typeDefault);
        int islandLength = ArrayUtil.CountNotEqual2D(islandMap, TileStateValue.Empty.GameObjectInstanceId);

        if(islandLength > 1)
        {
            await PuzzlePresentation.AbsorbToBomb(sceneData.PuzzleController, islandMap, position);
        }

        int explosionLevel = (islandLength<3)?islandLength:Mathf.Max(grid.GetLength(0), grid.GetLength(1));

        var destroyMap = ArrayUtil.GetNeighboringMap(PuzzleLogic.GetIdGrid(grid),
            position,
            explosionLevel,
            TileStateValue.Empty.GameObjectInstanceId);

        await PuzzlePresentation.BombExplosion(sceneData.PuzzleController, position, explosionLevel, destroyMap);

        destroyMap[position.x, position.y] = grid[position.x, position.y].GameObjectInstanceId;
        await ReusableRule.UpdateGoalRule(sceneData, destroyMap);
        PuzzleLogic.DestroyTileBatch(sceneData.PuzzleController, destroyMap);

        await ReusableRule.DropRule(sceneData.PuzzleController, position);
        await ReusableRule.Refill(sceneData.PuzzleController, position);
    }
}