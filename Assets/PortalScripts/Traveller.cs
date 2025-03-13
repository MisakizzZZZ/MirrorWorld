using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traveller : MonoBehaviour
{
    public GameObject travellerPrefabl;  //该Traveller的预制体
    
    
    private int portalSide=1;  //当前与portal的朝向关系、使用“点乘+取符号”计算
    private GameObject cloneTraveller;  //克隆旅行者



    //对换传送：Clone物体会被传送到原位、原物体传送至对面

    //参数：（目标传送门的transform组件、出发的传送门的transform组件）
    public void SwitchTeleport(Transform linkedPortalTransform, Transform thisPortalTransform)
    {
        var localToWorldMatrix = linkedPortalTransform.localToWorldMatrix * thisPortalTransform.worldToLocalMatrix * this.transform.localToWorldMatrix;

        //将克隆物体传送至原位置
        cloneTraveller.transform.position = this.transform.position;
        cloneTraveller.transform.rotation = this.transform.rotation;

        //更新当前新位置
        this.transform.position = localToWorldMatrix.GetColumn(3);  
        this.transform.rotation = localToWorldMatrix.rotation;

    }



    //更新克隆物体位置：同步克隆物体与目标传送门的位置
    //参数：（目标传送门的transform组件、出发的传送门的transform组件）
    public void MoveClone(Transform linkedPortalTransform, Transform thisPortalTransform)
    {
        var localToWorldMatrix = linkedPortalTransform.localToWorldMatrix * thisPortalTransform.worldToLocalMatrix * this.transform.localToWorldMatrix;

        //更新克隆物体位置
        cloneTraveller.transform.position = localToWorldMatrix.GetColumn(3);
        cloneTraveller.transform.rotation = localToWorldMatrix.rotation;
    }




    //进入portal时执行的方法
    public void EnterPortal(Transform portalTransform)
    {
        //如果还没有被克隆过则进行克隆
        if (cloneTraveller == null)
        {
            cloneTraveller = GameObject.Instantiate(this.travellerPrefabl);   //克隆预制体
            cloneTraveller.transform.parent = this.transform.parent;  //具有相同的父物体
            cloneTraveller.transform.localScale = this.transform.localScale;  //具有相同的缩放比例
        }
        cloneTraveller.SetActive(true);  //激活克隆体
        CalAndSavePortalSide(portalTransform);   //初次计算与传送门的朝向关系（不用管返回值）
    }


    //离开portal时执行的方法
    public void LeavePortal()
    {
        cloneTraveller.SetActive(false);   //禁用克隆体
    }


    //计算当前是否要穿过传送门、并更新portalSide标志、返回是否穿过
    //如果true代表方向发生了变化，穿过了传送门、否则为false没穿过
    public bool CalAndSavePortalSide(Transform portalTransform)
    {

        Vector3 offsetFromPortal = this.transform.position - portalTransform.position;//offsetFromPortal矢量：（traveller位置-portal位置：由portal指向traveller的矢量） 用于判断traveller和门的朝向关系 
        int oldPortalSide = this.portalSide;  //保存原先的portalSide
        this.portalSide = System.Math.Sign(Vector3.Dot(offsetFromPortal, portalTransform.transform.forward));  //点乘+取符号 判断方向，并保存
        return oldPortalSide == this.portalSide;  //返回原先的side和新side的对比结果
    }



}
