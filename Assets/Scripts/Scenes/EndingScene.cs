using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class EndingScene : BaseScene
{

    protected override void Init()
    {
        base.Init();
        SceneType = SceneDefine.Scene.EndingScene;
    }


    private void Update()
    {
        if (Input.anyKey)
        {
            GameManager.Scene.LoadScene(SceneDefine.Scene.RobbyScene);
        }
    }

    public override void Clear()
    {
    }
}
