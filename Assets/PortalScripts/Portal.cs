using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Camera portalCamera;  //传送门摄像机、不断调整位置进行贴图绘制
    private Camera playerCamera;  //玩家摄像机

    //优化
    private Collider objectCollider;
    private bool isVisible = false;
    private float hFov = 0;

    private MeshRenderer portalScreen;   //本传送门的屏幕
    private GameObject screenGameObject;
    private GameObject maskGameObject;  //在无法看见镜子时、显示这个


    //斜平面裁剪相关系数
    public float nearClipOffset = 0.05f;
    public float nearClipLimit = 0.2f;



    private RenderTexture portalTexture;    //本Portal的贴图


    //由外部调用，检测这个物体是否可被当前的镜子看见
    public bool CheckObjectVisibleInMirror(GameObject target)
    {
        //当前镜子不在玩家的可视范围则直接认为该镜子看不到此物体
        if (!isVisible) return false;


        // 计算相机的视锥体
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(portalCamera);

        //检测物体是否有collider，如果有按照collider算
        if (target.GetComponent<Collider>()) return GeometryUtility.TestPlanesAABB(planes, target.GetComponent<Collider>().bounds);

        //否则用renderer获取物体的包围盒
        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer == null) return false; // 没有 Renderer 可能是空物体

        //判断包围盒是否在视锥体内
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);

    }


    public void Start()
    {
        

        portalCamera = gameObject.GetComponentInChildren<Camera>();
        playerCamera = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>();
        objectCollider = gameObject.GetComponentInChildren<Collider>();

        //镜子物体获取
        screenGameObject = transform.Find("Screen").gameObject;
        maskGameObject = transform.Find("Mask").gameObject;
        portalScreen = screenGameObject.GetComponent<MeshRenderer>();

        //hFov获取
        float vFov = Camera.main.fieldOfView; // 获取纵向FOV
        float aspectRatio = Camera.main.aspect; // 宽高比 (width / height)
        hFov = 2f * Mathf.Atan(Mathf.Tan(vFov * Mathf.Deg2Rad / 2f) * aspectRatio);
    }



    public void Render()
    {
        //优化，如果不在视锥体则不渲染
        if (playerCamera != null && objectCollider != null)
        {
            // 获取摄像机的视锥体平面
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(playerCamera);


            //先判断人物是否在镜子前方，如果不在的话将直接不允许渲染,使用点乘

            Debug.Log((Mathf.Acos(transform.forward.x * playerCamera.transform.forward.x + transform.forward.z * playerCamera.transform.forward.z)));
            if ((transform.forward.x * (playerCamera.transform.position - this.transform.position).x + transform.forward.z * (playerCamera.transform.position - this.transform.position).z)<0)
            {
                isVisible = false;
            }
            //使用视锥体的夹角进行剔除
            else if(Mathf.Acos(transform.forward.x*playerCamera.transform.forward.x+ transform.forward.z * playerCamera.transform.forward.z)<(0.5 * Mathf.PI-0.5*hFov))
            {
                isVisible = false;
            }
            // 判断物体的包围盒是否在视锥体内
            else if (GeometryUtility.TestPlanesAABB(frustumPlanes, objectCollider.bounds))
            {
                isVisible = true;
            }
            else
            {
                isVisible = false;
            }
        }
        //无法看见时替换镜子平面
        screenGameObject.SetActive(isVisible);
        maskGameObject.SetActive(!isVisible);
        if (!isVisible) return;


        CreatePortalTexture();  //创造portalTexture，这个材质中的画面由portalCam提供、渲染到此传送门上



        portalCamera.projectionMatrix = playerCamera.projectionMatrix;


        //新的位置对应的localToWorldMatrix:通过旧的localToWorldMatrix 再进行一次双门迭代计算出


        Vector3 cameraPosition = playerCamera.transform.position;
        Vector3 mirrorNormal = transform.forward;
        Vector3 reflectedPosition = cameraPosition - 2 * Vector3.Dot((cameraPosition - transform.position), mirrorNormal) * mirrorNormal;

        Quaternion reflectedRotation = Quaternion.LookRotation(
            Vector3.Reflect(playerCamera.transform.forward, mirrorNormal),
            Vector3.Reflect(playerCamera.transform.up, mirrorNormal)
        );

        var renderPosition = reflectedPosition;
        var renderRotation = reflectedRotation;

 

        // Hide screen so that camera can see through portal
        this.portalScreen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;


        // displayMask：set to 1 to display texture, otherwise will draw test colour
        //为1的时候会绘制材质，为0的时候会显示默认颜色
        //迭代是有深度限制的。这里采用的方法是、最深层迭代采用默认的颜色，其他的按照正常绘制
        //因此在第一次绘制的时候、需要使用displayMask将最后一层设置某种默认颜色，然后绘制出第一张Render。此时材质中已经有了最后一层迭代看到的正确场景、可以设置Mask将其画在传送门上、继续n-1层迭代

        portalScreen.material.SetInt("displayMask", 0);

        portalCamera.transform.SetPositionAndRotation(renderPosition, renderRotation);  //设置portalCam的位置
        SetNearClipPlane();  //设置该轮迭代的裁剪平面
        portalCamera.Render();

        portalScreen.material.SetInt("displayMask", 1);
        // Unhide objects hidden at start of render
        this.portalScreen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

    }




    //函数功能：在需要时执行(开始时以及屏幕长宽比改变时) 创造一个RenderTexture赋给传送门摄像机(portalCam)。传送门摄像机的渲染画面将渲染至此texture上。
    void CreatePortalTexture()
    {
        //在portalTexture材质不存在、以及屏幕缩放比例改变时执行
        if (portalTexture == null || portalTexture.width != Screen.width || portalTexture.height != Screen.height)
        {
            if (portalTexture != null)
            {
                portalTexture.Release();  //释放之前的纹理资源
            }
            portalTexture = new RenderTexture(Screen.width, Screen.height, 0);   //生成RenderTexture对象
            portalCamera.targetTexture = portalTexture;  //把portalTexture对象赋给传送门摄像机targetTexture:可以创建一个渲染纹理应用给相机，相机视图将自动渲染到portalTexture中
            portalScreen.material.SetTexture("_MainTex", portalTexture);  //这个贴图将作为portalScreen的材质 保存在_MainTex里。由Shader后续调用
        }
    }






    //函数功能：设置裁剪平面的函数

    // Use custom projection matrix to align portal camera's near clip plane with the surface of the portal：使用自定义投影矩阵将传送门相机的近剪辑平面与传送门的表面对齐
    // Note that this affects precision of the depth buffer, which can cause issues with effects like screenspace AO
    void SetNearClipPlane()
    {
        //用于裁剪的平面、其transform直接使用linkedPortal的transform
        Transform clipPlane = this.transform;

        //（portal.forward 点乘 由portalCamera位置指向linkedPortal的向量）
        int dot = System.Math.Sign(Vector3.Dot(clipPlane.forward, this.transform.position - portalCamera.transform.position));

        Vector3 camSpacePos = portalCamera.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
        Vector3 camSpaceNormal = portalCamera.worldToCameraMatrix.MultiplyVector(clipPlane.forward) * dot;
        float camSpaceDst = -Vector3.Dot(camSpacePos, camSpaceNormal) + nearClipOffset;

        // Don't use oblique clip plane if very close to portal as it seems this can cause some visual artifacts
        if (Mathf.Abs(camSpaceDst) > nearClipLimit)
        {
            Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);

            // Update projection based on new clip plane
            // Calculate matrix with player cam so that player camera settings (fov, etc) are used
            portalCamera.projectionMatrix = portalCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
        }
        else
        {
            portalCamera.projectionMatrix = portalCamera.projectionMatrix;
        }
    }


}
