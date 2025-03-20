using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ChestPasswordGUI : MonoBehaviour
{
    [Header("密码界面相关")]
    public GameObject passwordPanel;
    // 4个数字输入框数组（依次为第一、第二个，etc.）
    public TextMeshProUGUI[] digitInputs;
    // 正确密码
    public string correctPassword = "2333";
    

    public string currentInput = "";

    // 密码正确后隐藏箱子、显示钥匙
    public GameObject chest_opaque;
    public GameObject chest_fade;
    public GameObject key;

    // 打开密码界面，清空所有输入，并将焦点设置到第一个输入框
    public void ShowPanel()
    {
        passwordPanel.SetActive(true);
        ClearInputs();
    }

    // 隐藏密码界面
    public void HidePanel()
    {
        passwordPanel.SetActive(false);
    }

    // 清空所有输入框内容
    void ClearInputs()
    {
        currentInput = "";
        UpdatePasswordText();
    }


    void Update()
    {
        //当前输入是否发生了改变
        bool inputChanged = false;

        //删除一个当前输入字符
        if (passwordPanel.activeSelf && Input.GetKeyDown(KeyCode.Backspace))
        {
            if (currentInput.Length > 0)
            {
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
                Debug.Log(currentInput);
                inputChanged = true;
            }
        }

       // Esc 或 Q 键关闭面板
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            HidePanel();
        }

        //监听数字输入，在当前输入的字符小于4时、向后追加
        for (KeyCode key = KeyCode.Alpha0; key <= KeyCode.Alpha9; key++)
        {
            if (Input.GetKeyDown(key))
            {
                if(currentInput.Length<4)
                {
                    inputChanged = true;
                    currentInput += (key - KeyCode.Alpha0).ToString();
                }
            }
        }


        //在监听到当前有输入操作、并且输入的序列发生改变时，更新密码UI上显示的文字
        if(inputChanged)
        {
            UpdatePasswordText();
            TrySubmitPassword();
        }


    }


    private void UpdatePasswordText()
    {
        for(int i=0;i<4;i++)
        {
            if(i< currentInput.Length)
            {
                Debug.Log(currentInput[i].ToString());
                digitInputs[i].text = currentInput[i].ToString();
            }
            else
            {
                digitInputs[i].text = "";
            }
        }
    }

    // 检查4个输入框是否已输入内容，并验证密码是否正确
    void TrySubmitPassword()
    {
        //当前输入小于四位则设置成黑色、且不检测
        if(currentInput.Length<4)
        {
            for (int i = 0; i < 4; i++)
            {
                digitInputs[i].color = Color.black;
            }
            return;
        }

        //否则检测是否正确，错误则设置为红色
        if (currentInput == correctPassword)
        {
            ShowFadeChestAndKey();
            HidePanel();
            Debug.Log($"密码正确，箱子打开！");
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                digitInputs[i].color = Color.red;
            }
            Debug.Log($"密码错误！");
        }
    }

    private void ShowFadeChestAndKey()
    {
        // 隐藏实体箱子
        if (chest_opaque != null)
        {
            chest_opaque.SetActive(false);
        }
        // 显示虚化箱子
        if (chest_fade != null)
        {
            chest_fade.SetActive(true);
            StartCoroutine(FadeOutChest());
        }
        // 显示钥匙
        if (key != null)
        {
            key.SetActive(true);
        }
    }

    IEnumerator FadeOutChest()
    {
        Debug.Log("Start fading out chest");
        // 获取 chest_fade的所有 Renderer（包括子对象）
        Renderer[] renderers = chest_fade.GetComponentsInChildren<Renderer>();

        // 在开始渐隐前，确保材质的初始 alpha 为 1
        foreach (var rend in renderers)
        {
            Material mat = rend.material;
            Color col = mat.color;
            col.a = 1f;
            mat.color = col;
        }

        float fadeDuration = 1f;
        float elapsedTime = 0f;
        // 存储材质的初始颜色
        Color[] initialColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            initialColors[i] = renderers[i].material.color;

        // 插值 alpha 到 0
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            for (int i = 0; i < renderers.Length; i++)
            {
                Material mat = renderers[i].material;
                Color newColor = initialColors[i];
                newColor.a = alpha;
                mat.color = newColor;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 确保最终完全透明
        for (int i = 0; i < renderers.Length; i++)
        {
            Material mat = renderers[i].material;
            Color newColor = initialColors[i];
            newColor.a = 0f;
            mat.color = newColor;
        }
        // 隐藏 chest_fade
        chest_fade.SetActive(false);
        Debug.Log("Finished fading out chest");
        // Deactivate the Password Panel Manager this script is attached to
        gameObject.SetActive(false);
    }
}
