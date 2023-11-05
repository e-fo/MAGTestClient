using System;
using System.Linq;
using UnityEngine;

public class PuzzleInputHandler : MonoBehaviour
{
    private Puzzle _puzzle;
    IRuleTileTap[] tapRules = null;

    private void OnEnable()
    {
        _puzzle = GetComponent<Puzzle>();

        //finds all types which implmented IRuleTileTap
        Type[] types = null;
        {
            types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IRuleTileTap).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .ToArray();
        }

        //creates instance for all of found rule classes
        tapRules = new IRuleTileTap[types.Length];
        for(int i=0; i<types.Length; ++i)
        {
            IRuleTileTap r = (IRuleTileTap)Activator.CreateInstance(types[i]);
            tapRules[i] = r;
        }
    }

    public void OnTapHandler(Vector2Int pos)
    {
        for(int i=0; i<tapRules.Length; ++i) 
        {
            tapRules[i].Execute(pos, _puzzle);
        }
    }
}