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
    public TMP_InputField[] digitInputs;
    // 正确的4位密码（可根据需要修改）
    public string correctPassword = "2333";

    void Start()
    {
        // 初始化时隐藏密码面板
        HidePanel();
    }

    /// <summary>
    /// 打开密码界面，清空所有输入，并将焦点设置到第一个输入框
    /// </summary>
    public void ShowPanel()
    {
        passwordPanel.SetActive(true);
        ClearInputs();
        // 设置焦点到第一个输入框
        EventSystem.current.SetSelectedGameObject(digitInputs[0].gameObject);
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
    /// 处理每个输入框的数值变化，自动校验输入是否为数字，且当输入完成后自动移动焦点或提交密码
    /// </summary>
    /// <param name="index">当前输入框的索引（0~3）</param>
    void OnDigitInputChanged(int index)
    {
        // 当前输入的内容不为空
        if (!string.IsNullOrEmpty(digitInputs[index].text))
        {
            // 只保留第一个字符
            char ch = digitInputs[index].text[0];
            // 如果不是数字，则清空
            if (!char.IsDigit(ch))
            {
                digitInputs[index].text = "";
                return;
            }
            else
            {
                digitInputs[index].text = ch.ToString();
            }
        }

        // 当当前输入框已有内容且不是最后一个时，自动将焦点移动到下一个输入框
        if (digitInputs[index].text.Length == 1)
        {
            if (index < digitInputs.Length - 1)
            {
                EventSystem.current.SetSelectedGameObject(digitInputs[index + 1].gameObject);
            }
            else
            {
                // 如果已经是最后一个输入框，则尝试提交密码
                TrySubmitPassword();
            }
        }
    }

    /// <summary>
    /// 这里提供四个公开方法，分别用于4个输入框的OnValueChanged事件调用。
    /// 你可以在Inspector中将每个Input Field的On Value Changed事件分别绑定以下方法。
    /// </summary>
    public void OnDigit0Changed(string value) { OnDigitInputChanged(0); }
    public void OnDigit1Changed(string value) { OnDigitInputChanged(1); }
    public void OnDigit2Changed(string value) { OnDigitInputChanged(2); }
    public void OnDigit3Changed(string value) { OnDigitInputChanged(3); }

    /// <summary>
    /// 在Update中监听Backspace键的按下，
    /// 当当前选中输入框内容为空时，自动将焦点切换到前一个输入框
    /// </summary>
    void Update()
    {
        if (passwordPanel.activeSelf && Input.GetKeyDown(KeyCode.Backspace))
        {
            GameObject current = EventSystem.current.currentSelectedGameObject;
            if (current != null)
            {
                for (int i = 0; i < digitInputs.Length; i++)
                {
                    if (current == digitInputs[i].gameObject)
                    {
                        // 如果当前输入框为空且不是第一个，则将焦点移到前一个
                        if (string.IsNullOrEmpty(digitInputs[i].text) && i > 0)
                        {
                            EventSystem.current.SetSelectedGameObject(digitInputs[i - 1].gameObject);
                        }
                        break;
                    }
                }
            }
        }

       // Esc 或 Q 键关闭面板
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
            {
                HidePanel();
            }
        }

    /// <summary>
    /// 检查4个输入框是否已输入内容，并验证密码是否正确
    /// </summary>
    void TrySubmitPassword()
    {
        string entered = "";
        foreach (var input in digitInputs)
        {
            entered += input.text;
        }
        if (entered.Length == digitInputs.Length)
        {
            if (entered == correctPassword)
            {
                Debug.Log($"密码正确，箱子打开！（{entered}）");
                // 在这里触发密码正确后的行为，例如播放动画、打开箱子等
                HidePanel();
            }
            else
            {
                Debug.Log($"密码错误！（{entered}，正确密码为 {correctPassword}）");
                ClearInputs();
                EventSystem.current.SetSelectedGameObject(digitInputs[0].gameObject);
            }
        }
    }
}
