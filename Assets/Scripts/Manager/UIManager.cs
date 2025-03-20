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
    private string[] dialogues = { 
        "What—?! Agh!",
         "Ngh! Something's here—!",
        "I— I can’t see it… but it’s here!",
        "I feel it… but I can’t see it!",
        "Gah—!!",
    };

    //EKey界面
    private GameObject eKeySign;
    private TextMeshProUGUI eKeySignText;
    private CanvasGroup eKeySignCanvasGroup;

    //Exit界面
    private GameObject exitGamePanel;
    



    void Awake()
    {
        base.Awake();
        canvas = GetComponentInChildren<Canvas>().gameObject;
        subtitleText = transform.Find("SubtitleText").GetComponent<TextMeshProUGUI>();
        screenDamageCanvas = transform.Find("ScreenDamageCanvas").gameObject;
        passwordPanel = transform.Find("PasswordPanelManager").GetComponent<ChestPasswordGUI>();
        eKeySign = transform.Find("EKeySign").gameObject;
        eKeySignText = eKeySign.transform.Find("EKeySignText").gameObject.GetComponent<TextMeshProUGUI>();
        eKeySignCanvasGroup = eKeySign.GetComponent<CanvasGroup>();
        exitGamePanel = transform.Find("GameEnd").gameObject;
    }
    

    void Start()
    {
        //隐藏密码界面
        HidePasswordPanel();
    }

    void Update()
    {
        SetInteractEKeySign();
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

            //说一些话
            UIManager.Instance.ShowSubtitle(dialogues[Random.Range(0, dialogues.Length)]);
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


    //---------E key悬浮
    private InteractableObject interactObject = null;
    public void SetEKeySignActive(InteractableObject target)
    {
        interactObject = target;
    }



    private void SetInteractEKeySign()
    {
        if(interactObject&&interactObject.enabled&&interactObject.gameObject.activeSelf)
        {
            Debug.Log(interactObject.ShouldReleaseEKeySign());
            if(!interactObject.ShouldReleaseEKeySign())
            {
                if(eKeySign.activeSelf == false)
                {
                    eKeySign.SetActive(true);
                    DOTween.Kill(eKeySignCanvasGroup);
                    eKeySignCanvasGroup.alpha = 0; // 初始透明度
                    eKeySignCanvasGroup.DOFade(1, 0.5f); // 0.5 秒内淡入到 1
                }
                
                eKeySign.transform.position = Camera.main.WorldToScreenPoint(interactObject.transform.position + interactObject.interactSignOffset);
                eKeySignText.text = interactObject.interactWord == "" ? "Interact" : interactObject.interactWord;
            }
            else
            {
                interactObject = null;
            }
        }
        else
        {
            eKeySign.SetActive(false);
        }
    }

    //-----------结束游戏
    public void ShowEndGame()
    {
        exitGamePanel.SetActive(true);

        //透明度渐变
        var canvasGroup = exitGamePanel.GetComponent<CanvasGroup>();
        Cursor.visible = true; //显示鼠标
        Cursor.lockState = CursorLockMode.None;  // 解锁鼠标
        DOTween.Kill(canvasGroup);
        canvasGroup.alpha = 0; // 初始透明度
        canvasGroup.DOFade(1, 0.5f); // 0.5 秒内淡入到 1
        DOVirtual.DelayedCall(0.5f, () => Time.timeScale = 0); //0.5f后游戏暂停计时
    }
}
