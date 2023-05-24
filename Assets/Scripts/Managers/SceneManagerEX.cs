using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEX : MonoBehaviour
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public SceneDefine.Scene nextScene;

    string GetSceneName(SceneDefine.Scene type)
    {
        string name = System.Enum.GetName(typeof(SceneDefine.Scene), type); // C#¿« Reflection. Scene enum¿« 
        return name;
    }

    public void LoadScene(SceneDefine.Scene type)
    {
        nextScene = type;
        SceneManager.LoadScene(GetSceneName(SceneDefine.Scene.LoadingScene));
    }

    public AsyncOperation LoadSceneAsync(SceneDefine.Scene nextScene) 
    {
        return SceneManager.LoadSceneAsync(GetSceneName(nextScene));
    }


    public void Clear()
    {
        CurrentScene.Clear();
    }
}
