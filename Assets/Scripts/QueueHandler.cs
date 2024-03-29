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
            
            if(actionQueue.Count > 0)
            {
                Debug.Log(currentAction + "At least 1 action found. Enqueueing new action");
                currentAction = actionQueue.Dequeue();
                currentAction.Start();

            }
        }
        else
        {
            Debug.Log("Updating Action ... ");
            //if (HuggingTime > 0)
            //{
            //    HuggingTime -= Time.deltaTime;
            //    TimeIsUp = false;
            //}
            //else if (HuggingTime <= 0)
            //{
            //    TimeIsUp = true;
            //}
            currentAction.Update();
            if (currentAction.IsFinished())
            {
                Debug.Log("Finished Action");
                currentAction = null;

            }
        }
    }

    
}
