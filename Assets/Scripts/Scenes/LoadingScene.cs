using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : BaseScene
{
    private SceneDefine.Scene nextScene;

    [SerializeField]
    public Image progressBar;


    protected override void Init()
    {
        base.Init();
        SceneType = SceneDefine.Scene.LoadingScene;
    }

    private void Awake()
    {
        nextScene = GameManager.Scene.nextScene;
        progressBar.fillAmount = 0.1f;
    }

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = GameManager.Scene.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false; // ���� �� �ε��Ǿ �ٷ��̵����ϰ� ��ٸ���.


        float timer = 0.0f;

        while (!op.isDone)
        {
            //yield return null;
            //yield return new WaitForSeconds(1.0f);
            
            timer += Time.deltaTime;

            if (op.progress < 0.9f) 
            { //�ε����϶�
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer); 
                
                
                if (progressBar.fillAmount >= op.progress) 
                {
                    yield return new WaitForSeconds(1f);// �ε��� �ʹ����� ���� ���������ش�.
                    timer = 0f; 
                } 
            }
            else
            { // �� �ε��̵Ǿ�����
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f) 
                { 
                    op.allowSceneActivation = true; 
                    yield break; 
                }
            }
        }
    }
    public override void Clear()
    {
      
    }
}
