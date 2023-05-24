# ���� �⺻ ����

## Assets Folder

Assets ���� ���� ����

- Resources
    
    �̹���, ��������Ʈ �� ���� �����͵�

    �߰��� ���µ� ������

    Ȥ�ó� �߰��� ū ������ �ִٸ� , **.Ignore�� ���� ��������Ѵ�**

- GameSettings
    
    - GameSettings.cs

    ���� �� ���۵Ǹ� ������ ����Ǿ� �ϴ� ���ӸŴ��� ���� �����Ҽ� �ִ� ��ũ��Ʈ

    - InputSystem.InputActions

    ���� ��ǲ���� �⺻���� ��ǲ�׼ǵ� 


- Scenes

    ���� ���Ӿ����� �� ��

- Animations
    
    ���� �ִϸ��̼� ��Ʈ�ѷ� �� �ִϸ��̼� ���� ���� �����Ұ�
    
    **(�� ������Ʈ���� ������ �սô�.)**

- Scripts

  ��ũ��Ʈ ���ϵ� �� ��

    - Managers
    
        �Ŵ��� �� �����Դϴ� ���� ���� �Ŵ����� ���Ŵ��� �����ξ����ϴ�.

    - Player
    
        �÷��̾� ���� ��ũ��Ʈ

    - Monsters
    
        ���� ���͵� ���� �������ؼ� �־�ӽô�.

    - Scenes 
    
        �� ���� ��ũ��Ʈ ����

        ���� Base �����ȿ� �ִ� BaseScene.cs �� �������� ���� ����ϴ�.
        �ڼ��Ѱ� �Ʒ�����



**�� �ʿ��� �������� �߰��Ͽ� ����ô�. �� �������� ���սô�.**


## �⺻ ����

������ ���۵Ǹ� GameSettings ��ũ��Ʈ�� ����˴ϴ�.

```cs
 //���ӽ������ڸ��� �Ʒ��� �Լ��� ȣ�����ش�.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {

        if (GameManager.Instance == null)
        {
            GameObject gameManager = new GameObject() { name = "GameManager" };
            gameManager.AddComponent<GameManager>();
        }
    }

```

����� �̱��� ���� ������� ���ӸŴ����� ȣ���մϴ�.

�Ǵٸ� �Ŵ����� �߰��ϰ�ʹٸ� 

�̰��� �ν��Ͻ��� �߰����ְ� 

InitManagers() �޼ҵ忡 �߰����ݽô�.


```cs
    //...�߷�
    
    //SceneManager
    private static SceneManagerEX sceneManager;
    public static SceneManagerEX Scene { get { return sceneManager; } }
    
    //..�߷�
    private void InitManagers()
    {

        GameObject dobj = new GameObject() { name = "SceneManagerEX" };
        dobj.transform.SetParent(transform);
        sceneManager = dobj.AddComponent<SceneManagerEX>();

    }
```

���� ó�� ���Ŵ����� �߰��Ҽ����ֽ��ϴ�.


## Scene System

���� ���� �ſ� �߿��� �����Դϴ�.

���� ���� �߰��ϰ������

**SceneDefine �ҽ��� �����̸�**���� �߰��Ͽ� ����մϴ�.

```cs
    public class SceneDefine    
    {
        public enum Scene
        {
            Unknown, // ����Ʈ ��
            LoadingScene, // �ε���
            TitleScene, // Ÿ��Ʋ �� 
            GameScene, // �ΰ��� ��
            EndingScene, // ���� ���� �� 
        }
    }
```

��� ������ BaseScene �� ����Ͽ� ����ô�

Base Scene�� ���ž��� �⺻ ���ø� �صξ����ϴ�..

���� �����

![Inspector](./Images/SceneInsperctor.png)

�� ��ũ���� ó��

�� ���� �� ������Ʈ�� Scene �� �ϳ������ (�ƹ��̸��̶� �������)

(�ٸ� ������Ʈ�� ���̸� �ֱ����� @ ǥ�ø� �߽��ϴ�.)

BaseScene�� ����ϴ� ���Ӿ� ��ũ���긦 �����ָ�˴ϴ�.

```cs
public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = SceneDefine.Scene.GameScene; // ���� �� �������־���մϴ�.
    }



    public override void Clear()
    {
    }
}
```

���� �߿��Ѱ� Init �� ���� ���� �������ݽô�.

�̰� ������� ���Ŵ����� ���ư��ϴ�.

