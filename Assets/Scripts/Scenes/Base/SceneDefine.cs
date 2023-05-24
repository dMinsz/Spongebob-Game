using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDefine 
{
    public enum Scene
    {
        Unknown, // 디폴트 씬
        LoadingScene, // 로딩씬
        TitleScene, // 타이틀 씬 
        GameScene, // 인게임 씬
        EndingScene, // 게임 엔딩 씬 
    }
}
