using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = SceneDefine.Scene.GameScene;
    }



    public override void Clear()
    {
    }
}
