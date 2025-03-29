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
            await UniTask.WaitForSeconds( 0.1f );
        }
        SceneManager.sceneLoaded -= setFlag;
    }
}