using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class SweeperAI : MonoBehaviour
{
    private NavMeshAgent agent; //导航网格代理
    private Transform playerTransform;

    private bool isVisibleInAnyMirror;  //是否在镜子中可见。这里的意思是，玩家当前能通过镜子看见这个物体

    //对玩家进行攻击
    private const int damageDistance = 2;



    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        isVisibleInAnyMirror = MirrorInteratable.CheckIsVisibleInAnyMirror(this.gameObject);

        if(isVisibleInAnyMirror)
        {
            agent.enabled = true;
            agent.SetDestination(playerTransform.position);
        }
        else
        {
            agent.enabled = false;
        }

        //判断和玩家的距离
        Debug.Log(Vector3.SqrMagnitude(playerTransform.position - this.transform.position));
        if(Vector3.SqrMagnitude(playerTransform.position - this.transform.position)< damageDistance* damageDistance)
        {
            UIManager.Instance.GetHurt();
        }

    }

}


