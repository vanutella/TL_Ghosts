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
        senderObj.GetComponent<GhostMovement>().SetStartVariables(targetObj.gameObject);
    }
    public override void Update()
    {
        base.Update();
       
    }

    public override bool IsFinished()
    {
        // Do Stuff and Set State back to wandering/default
        if (senderObj.GetComponent<GhostMovement>().isDone == true)
        {
            Debug.Log("Is Done with Hugging");

            return true;
        }
        else
        {
            Debug.Log("Not yet done");
            return false;
        }
    }

}
