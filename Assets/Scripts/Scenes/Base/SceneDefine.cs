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
        RobbyScene, // 로비씬
        FlyDutchManScene, // 보스 1
        EndingScene, // 게임 엔딩 씬 
    }
}
