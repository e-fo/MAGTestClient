using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public static class UnityEngineUtil
{
    public async static UniTask LoadSceneWithIndex( int idx )
    {
        bool isLoaded = false;
        void setFlag(Scene scene, LoadSceneMode mode) => isLoaded = true;
        SceneManager.sceneLoaded += setFlag;
        SceneManager.LoadScene(idx);
        while( !isLoaded )
        {
            await UniTask.Yield();
        }
        SceneManager.sceneLoaded -= setFlag;
    }
}