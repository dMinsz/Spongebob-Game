using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class TitleScene : BaseScene
{

    protected override void Init()
    {
        base.Init();
        SceneType = SceneDefine.Scene.TitleScene;
    }


    private void Update()
    {
        if (Input.anyKey)
        {
            GameManager.Scene.LoadScene(SceneDefine.Scene.GameScene);
        }
    }

    public override void Clear()
    {
    }
}
