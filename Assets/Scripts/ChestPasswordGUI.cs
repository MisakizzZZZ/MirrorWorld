using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChestPasswordGUI : MonoBehaviour
{
    [Header("����������")]
    // ����4��Input Field���������
    public GameObject passwordPanel;
    // ������������飨˳�����ζ�Ӧ��һλ���ڶ�λ������
    public TMP_InputField[] digitInputs;
    // ��ȷ��4λ���루�ɸ�����Ҫ�޸ģ�
    public string correctPassword = "2333";

    void Start()
    {
        // ��ʼ��ʱ�����������
        HidePanel();
    }

    /// <summary>
    /// ��������棬����������룬�����������õ���һ�������
    /// </summary>
    public void ShowPanel()
    {
        passwordPanel.SetActive(true);
        ClearInputs();
        // ���ý��㵽��һ�������
        EventSystem.current.SetSelectedGameObject(digitInputs[0].gameObject);
    }

    /// <summary>
    /// �����������
    /// </summary>
    public void HidePanel()
    {
        passwordPanel.SetActive(false);
        Debug.Log("�ر����������");
    }

    /// <summary>
    /// ����������������
    /// </summary>
    void ClearInputs()
    {
        foreach (var input in digitInputs)
        {
            input.text = "";
        }
    }

    /// <summary>
    /// ����ÿ����������ֵ�仯���Զ�У�������Ƿ�Ϊ���֣��ҵ�������ɺ��Զ��ƶ�������ύ����
    /// </summary>
    /// <param name="index">��ǰ������������0~3��</param>
    void OnDigitInputChanged(int index)
    {
        // ��ǰ��������ݲ�Ϊ��
        if (!string.IsNullOrEmpty(digitInputs[index].text))
        {
            // ֻ������һ���ַ�
            char ch = digitInputs[index].text[0];
            // ����������֣������
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

        // ����ǰ��������������Ҳ������һ��ʱ���Զ��������ƶ�����һ�������
        if (digitInputs[index].text.Length == 1)
        {
            if (index < digitInputs.Length - 1)
            {
                EventSystem.current.SetSelectedGameObject(digitInputs[index + 1].gameObject);
            }
            else
            {
                // ����Ѿ������һ������������ύ����
                TrySubmitPassword();
            }
        }
    }

    /// <summary>
    /// �����ṩ�ĸ������������ֱ�����4��������OnValueChanged�¼����á�
    /// �������Inspector�н�ÿ��Input Field��On Value Changed�¼��ֱ�����·�����
    /// </summary>
    public void OnDigit0Changed(string value) { OnDigitInputChanged(0); }
    public void OnDigit1Changed(string value) { OnDigitInputChanged(1); }
    public void OnDigit2Changed(string value) { OnDigitInputChanged(2); }
    public void OnDigit3Changed(string value) { OnDigitInputChanged(3); }

    /// <summary>
    /// ��Update�м���Backspace���İ��£�
    /// ����ǰѡ�����������Ϊ��ʱ���Զ��������л���ǰһ�������
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
                        // �����ǰ�����Ϊ���Ҳ��ǵ�һ�����򽫽����Ƶ�ǰһ��
                        if (string.IsNullOrEmpty(digitInputs[i].text) && i > 0)
                        {
                            EventSystem.current.SetSelectedGameObject(digitInputs[i - 1].gameObject);
                        }
                        break;
                    }
                }
            }
        }

       // Esc �� Q ���ر����
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
            {
                HidePanel();
            }
        }

    /// <summary>
    /// ���4��������Ƿ����������ݣ�����֤�����Ƿ���ȷ
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
                Debug.Log($"������ȷ�����Ӵ򿪣���{entered}��");
                // �����ﴥ��������ȷ�����Ϊ�����粥�Ŷ����������ӵ�
                HidePanel();
            }
            else
            {
                Debug.Log($"������󣡣�{entered}����ȷ����Ϊ {correctPassword}��");
                ClearInputs();
                EventSystem.current.SetSelectedGameObject(digitInputs[0].gameObject);
            }
        }
    }
}
