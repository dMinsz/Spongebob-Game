using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //gameManager
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    //SceneManager
    private static SceneManagerEX sceneManager;
    public static SceneManagerEX Scene { get { return sceneManager; } }

    private GameManager() { }
 


    private void Awake() // ����Ƽ������ �����ͻ󿡼� �߰��Ҽ� �ֱ⶧���� �̷������α���
    {
        if (instance != null)
        {
            Debug.LogWarning("GameInstance: valid instance already registered.");
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this); // ����Ƽ�� ���� ��ȯ�ϸ� �ڵ����� ������Ʈ���� �����ȴ�
                                 // �ش� �ڵ�� ���� ���ϰ� ����
        instance = this;

        InitManagers();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void InitManagers()
    {

        GameObject dobj = new GameObject() { name = "SceneManagerEX" };
        dobj.transform.SetParent(transform);
        sceneManager = dobj.AddComponent<SceneManagerEX>();

    }
}
