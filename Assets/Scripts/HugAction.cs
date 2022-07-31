using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugAction : Action
{
    private GameObject senderObj;
    private GameObject targetObj;

 
    public HugAction(GameObject sender, GameObject target = default(GameObject))
    {
        senderObj = sender;
        if (target)
        {
            targetObj = target;
        }

    }

    public override void Start()
    {
        base.Start();
        // Check if there is not already an action running and start the process
        if (senderObj.GetComponent<GhostState>().isWandering && targetObj.GetComponent<GhostState>().isWandering)
        {
            senderObj.GetComponent<GhostState>().SetStartVariables(targetObj.gameObject);
            
        }

    }
    public override void Update()
    {
        base.Update();
       
    }

    public override bool IsFinished()
    {
        // Do Stuff and Set State back to wandering/default
        if (senderObj.GetComponent<GhostState>().isDone == true && senderObj.GetComponent<GhostState>().isHugging == false)
        {
            Debug.Log("Is Done with Hugging");
            senderObj.GetComponent<GhostState>().isDone = true;
            senderObj.GetComponent<GhostState>().isWandering = true;
            targetObj.GetComponent<GhostState>().isWandering = true;
            targetObj.GetComponent<GhostState>().isTarget = false;
            targetObj.GetComponent<GhostState>().startedHugging = false;
            return true;
        }
        else
        {
           // Debug.Log("Not yet done");
            return false;
        }
    }

}
