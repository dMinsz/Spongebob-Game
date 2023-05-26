using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyDutchManScene : BaseScene
{
    [SerializeField]
    public Image progressBar;
   
    private FlyDutchManController dutchMan;
    private float HPMax;
    private float nowHP;
    private float timer = 0.0f;
    private void Awake()
    {
        progressBar.fillAmount = 1.0f;
        dutchMan = GameObject.FindGameObjectWithTag("Boss").GetComponent<FlyDutchManController>();
        HPMax = dutchMan.HP;
    }

    private void Update()
    {
        nowHP = dutchMan.HP;
        
        timer += Time.deltaTime;

        if ((nowHP/HPMax)<= 0.95) // 95프로미만이면
        {
            progressBar.fillAmount = (nowHP / HPMax) ;
            
        }
        else if (nowHP == 0 || nowHP < 0)
        {
            progressBar.fillAmount = 0.0f;
        }
    }
    protected override void Init()
    {
        base.Init();
        SceneType = SceneDefine.Scene.FlyDutchManScene;
    }


    public override void Clear()
    {
    }
}
