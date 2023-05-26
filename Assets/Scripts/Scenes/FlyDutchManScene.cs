using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyDutchManScene : BaseScene
{
    [SerializeField]
    public Image BossBar;
    [SerializeField]
    public Image PlayerBar;
   
    private FlyDutchManController dutchMan;
    private PlayerController player;

    private float BossHPMax;
    private float BossNowHP;

    private float PlayerHPMax;
    private float PlayerNowHP;

    private float timer = 0.0f;
    private void Awake()
    {
        
        BossBar.fillAmount = 1.0f;
        PlayerBar.fillAmount = 1.0f;

        dutchMan = GameObject.FindGameObjectWithTag("Boss").GetComponent<FlyDutchManController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        BossHPMax = dutchMan.HP;
        PlayerHPMax = player.Hp;
    }

    private void Update()
    {
        BossNowHP = dutchMan.HP;
        PlayerNowHP = player.Hp;

        timer += Time.deltaTime;

        //player hp bar
        if ((PlayerNowHP / PlayerHPMax) <= 0.95)
        {
            PlayerBar.fillAmount = (PlayerNowHP / PlayerHPMax);
        }
        else if (PlayerNowHP <= 0)
        {
            PlayerBar.fillAmount = 0.0f;
        }

        //boss hp bar
        if ((BossNowHP/BossHPMax)<= 0.95)
        {
            BossBar.fillAmount = (BossNowHP / BossHPMax) ;
        }
        else if (BossNowHP <= 0)
        {
            BossBar.fillAmount = 0.0f;
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
