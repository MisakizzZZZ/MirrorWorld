using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChestPasswordGUI : MonoBehaviour
{
    [Header("密码界面相关")]
    // 包含4个Input Field的整个面板
    public GameObject passwordPanel;
    // 数字输入框数组（顺序依次对应第一位、第二位……）
    public TextMeshProUGUI[] digitInputs;
    // 正确的4位密码（可根据需要修改）
    public string correctPassword = "2333";


    public string currentInput = "";


    /// <summary>
    /// 打开密码界面，清空所有输入，并将焦点设置到第一个输入框
    /// </summary>
    public void ShowPanel()
    {
        passwordPanel.SetActive(true);
        ClearInputs();
    }

    /// <summary>
    /// 隐藏密码界面
    /// </summary>
    public void HidePanel()
    {
        passwordPanel.SetActive(false);
        Debug.Log("关闭了密码界面");
    }

    /// <summary>
    /// 清空所有输入框内容
    /// </summary>
    void ClearInputs()
    {
        foreach (var input in digitInputs)
        {
            input.text = "";
        }
    }




    /// <summary>
    /// 在Update中监听Backspace键的按下，
    /// 当当前选中输入框内容为空时，自动将焦点切换到前一个输入框
    /// </summary>
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

    /// <summary>
    /// 检查4个输入框是否已输入内容，并验证密码是否正确
    /// </summary>
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
}
