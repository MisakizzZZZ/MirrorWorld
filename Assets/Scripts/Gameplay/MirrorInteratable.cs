using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MirrorInteratable : MonoBehaviour
{
    //真实世界和镜子中的GameObject
    public GameObject objectInRealworld;
    public GameObject objectInMirror;

    //公共变量
    static List<Portal> mirrorScripts; //所有镜子的脚本

    //判断某个gameObject是否可见？这里的意思是，玩家当前能通过镜子看见这个物体
    public static bool CheckIsVisibleInAnyMirror(GameObject gameObject)
    {
        foreach (var mirrorScript in mirrorScripts)
        {
            if (mirrorScript.CheckObjectVisibleInMirror(gameObject))
            {
                return true;
            }
        }
        return false;
    }


    //状态
    protected bool isVisibleInPlayerCamera;  //是否在玩家视锥体内可见
    protected bool isVisibleInAnyMirror;  //是否在镜子中可见。这里的意思是，玩家当前能通过镜子看见这个物体

    public void Start()
    {
        if (mirrorScripts == null)
        {
            var mirrors  = GameObject.FindGameObjectsWithTag("Mirror");
            mirrorScripts = new List<Portal>();
            for(int i=0;i< mirrors.Length;i++)
            {
                mirrorScripts.Add(mirrors[i].GetComponentInChildren<Portal>());
            }
        }
    }

    private bool CheckIsVisibleInPlayerCamera()
    {
        // 计算相机的视锥体
        Camera mainCamera = Camera.main;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        //检测物体是否有collider，如果有按照collider算
        if (this.GetComponent<Collider>()) return GeometryUtility.TestPlanesAABB(planes, this.GetComponent<Collider>().bounds);

        //否则用renderer获取物体的包围盒
        Renderer renderer = this.GetComponent<Renderer>();
        if (renderer == null) return false; // 没有 Renderer 可能是空物体

        //判断包围盒是否在视锥体内
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }
    
    private bool CheckIsVisibleInAnyMirror()
    {
        return CheckIsVisibleInAnyMirror(this.gameObject);
    }



    // Update is called once per frame
    public void Update()
    {
        isVisibleInPlayerCamera = CheckIsVisibleInPlayerCamera();
        isVisibleInAnyMirror = CheckIsVisibleInAnyMirror();
    }

}
