using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public SceneDefine.Scene SceneType { get; protected set; } = SceneDefine.Scene.Unknown; // 디폴트로 Unknow 이라고 초기화

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
    }

    public abstract void Clear();
}
