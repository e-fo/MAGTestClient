using UnityEngine;

public class BombExplosionRule : IRuleTileTap
{
    public TileBaseType[] AcceptedBaseTypes => new TileBaseType[]{ TileBaseType.Bomb };

    public async void Execute(Vector2Int position, Puzzle puzzle)
    {
        puzzle.InputAvailable = false;

        var grid = puzzle.Grid;
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
            await PuzzlePresentation.AbsorbToBomb(puzzle, islandMap, position);
        }

        int explosionLevel = (islandLength<3)?islandLength:Mathf.Max(grid.GetLength(0), grid.GetLength(1));

        var destroyMap = ArrayUtil.GetNeighboringMap(PuzzleLogic.GetIdGrid(grid),
            position,
            explosionLevel,
            TileStateValue.Empty.GameObjectInstanceId);

        await PuzzlePresentation.BombExplosion(puzzle, position, explosionLevel, destroyMap);

        destroyMap[position.x, position.y] = grid[position.x, position.y].GameObjectInstanceId;
        PuzzleLogic.DestroyTileBatch(puzzle, destroyMap);

        await ReusableRule.DropRule(puzzle, position);
        await ReusableRule.Refill(puzzle, position);

        puzzle.InputAvailable = true;
    }
}