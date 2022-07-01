using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueHandler : MonoBehaviour
{
    public Queue<Action> actionQueue = new Queue<Action>();
    public Action currentAction = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (currentAction == null)
        {
            Debug.Log(currentAction + " no action found. Enqueueing new action");
            if(actionQueue.Count > 0)
            {
                currentAction = actionQueue.Dequeue();
                currentAction.Start();
            }
        }
        else
        {
            Debug.Log("Updating Action ... ");
            currentAction.Update();
            if (currentAction.IsFinished())
            {
                Debug.Log("Finished Action");
                currentAction = null;
            }
        }
    }
}
