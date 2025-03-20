using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void ExitTheGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  //在编辑器模式下退出游戏
#else
    Application.Quit();  //在打包的游戏中正常退出
#endif
    }
}
