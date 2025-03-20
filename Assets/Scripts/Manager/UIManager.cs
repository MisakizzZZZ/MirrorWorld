using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : UnitySingleton<UIManager>
{
    //config
    public float textFadeInDuration = 0.5f; //淡入时间
    public float textFadeOutDuration = 0.5f; //淡出时间
    public float textDisplayDuration = 4f;   // 文本显示时间


    //组件
    private GameObject canvas;


    //
    private TextMeshProUGUI subtitleText; // UI 文本

    //密码界面
    private ChestPasswordGUI passwordPanel; //使用外部的脚本接口控制

    //受伤面板
    private GameObject screenDamageCanvas;


    void Awake()
    {
        base.Awake();
        canvas = GetComponentInChildren<Canvas>().gameObject;
        subtitleText = transform.Find("SubtitleText").GetComponent<TextMeshProUGUI>();
        screenDamageCanvas = transform.Find("ScreenDamageCanvas").gameObject;
        passwordPanel = transform.Find("PasswordPanelManager").GetComponent<ChestPasswordGUI>();
    }
    

    void Start()
    {
        //隐藏密码界面
        HidePasswordPanel();

        ShowSubtitle("You are testing subtitle");
    }



    //-----------------Subtitle相关--------------

    private Coroutine subtitleCoroutine; // 记录当前的协程
    public void ShowSubtitle(string message)
    {
        if (subtitleCoroutine != null)
        {
            StopCoroutine(subtitleCoroutine);
        }
        subtitleCoroutine = StartCoroutine(ShowSubtitleRoutine(message));
    }

    private IEnumerator ShowSubtitleRoutine(string message)
    {
        subtitleText.text = message;
        subtitleText.DOFade(1, textFadeInDuration); // 淡入
        yield return new WaitForSeconds(textDisplayDuration); //显示秒
        subtitleText.DOFade(0, textFadeOutDuration); // 淡出
    }



    //------------------受伤特效----------------

    const float damageCD = 4f;
    float lastTimeGetHurt = 0;

    //外部接口，调用后触发受伤
    public void GetHurt()
    {
        if(Time.time - lastTimeGetHurt>damageCD)
        {
            lastTimeGetHurt= Time.time;
            screenDamageCanvas.SetActive(false);
            screenDamageCanvas.SetActive(true);
            screenDamageCanvas.GetComponentInChildren<AudioSource>().enabled = true;
            screenDamageCanvas.GetComponentInChildren<Animator>().enabled = true;
        }
    }

    //----------------密码输入相关----------

    public void ShowPasswordPanel()
    {
        passwordPanel.ShowPanel();
    }


    public void HidePasswordPanel()
    {
        passwordPanel.HidePanel();
    }





}
