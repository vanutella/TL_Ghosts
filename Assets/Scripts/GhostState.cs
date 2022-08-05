using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostState : MonoBehaviour
{

    private PlayerInfos playerInfos;
    private Rigidbody rb;
    private float speed = 1f;
    private Quaternion startingDir;

    [Header("State Machine")]
    public bool isWandering; // IDLE
    public bool isTarget; // Hugging Mode but IS target
    public bool isHugging; // Hugging and Moving to target
    public bool startedHugging = false, isDone = true;

    private bool startedCoroutine = false;
    [Header("Interaction Variables")]
    private float playerRadius = 0.9f;
    public float dist;
    public Transform targetObject;

    private bool timeIsUp;
    private float HuggingDuration = 25f;
    private float HuggingTime = 25f;
    void Start()
    {
        playerInfos = GetComponent<PlayerInfos>();

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.maxAngularVelocity = 60f; //set it to something pretty high so it can actually follow properly!

        startingDir = transform.rotation; // Save rotation

        isWandering = true; // starting to hover right from the beginning. idle mode should always be isWandering
    }


    void Update()
    {
        if (isHugging && targetObject)
        {
            if (HuggingTime > 0) { HuggingTime -= Time.deltaTime; timeIsUp = false; }
            else if(HuggingTime <= 0) { timeIsUp = true; }
            
            targetObject.GetComponent<GhostState>().isTarget = true; // block from getting targeted again
            targetObject.GetComponent<GhostState>().isWandering = false; // stop moving
            if (timeIsUp)
            {
                startedHugging = false;
                targetObject = null;
                isHugging = false;
                isDone = true;
                StopGhost();
                HuggingTime = HuggingDuration;
                Debug.Log("Took to long");
            }
        }
    }

    private void FixedUpdate()
    {
        // When HoverMode is on - get new HoverForce and start a new hovering Coroutine
        if (isWandering)
        {
            if (!startedCoroutine)
            {
                GetNewHoverForce();
                StartCoroutine(Hovering(standingTime()));
                startedCoroutine = true;
            }
            return;
        }

        // When a hug is in process
        // Sender is hugging someone aka has target 
        // Cannot be targeted (should be but need to implement still) -- this goes to hug command check
        if (isHugging)
        {
            
            dist = Vector3.Distance(rb.transform.position, targetObject.position);
            if (dist > playerRadius) // didn't reach target yet, keep moving!
            {
                MoveToTarget();
                RotateToTarget();
                targetObject.GetComponent<GhostState>().StopGhost(); // stop target ghost
            }
            // Initiate the hugging process, continue in Update
            else if(dist < playerRadius)
            {
                this.StopGhost();
                targetObject.GetComponent<GhostState>().StopGhost();
                if (!startedHugging)
                {
                    
                    startedHugging = true;
                    targetObject.GetComponent<GhostState>().startedHugging = true;
                    StartCoroutine(Hugging(3));
                }
            }

        }

        // When object is target of other player
        // Stop moving and block/queue incoming hugs bc it's not wandering -- this goes to hug command check
        if (isTarget)
        {

        }
    }

    private void GetNewHoverForce()
    {
        rb.AddRelativeForce(Random.onUnitSphere * (Random.Range(2, 6)));
    }

    private float standingTime()
    {
        return Random.Range(1, 5);
    }

    // Waiting for x Seconds until getting a new hover force
    private IEnumerator Hovering(float hoverTime)
    {
        yield return new WaitForSeconds(hoverTime);
        startedCoroutine = false;
    }

    ////// TARGET INVOLVED STUFF i.e. hugging ////////
    // ------------------------------------------------------------------------------------------------------ // 

    // [1] Set a Target and start the hugging process
    public void SetStartVariables(GameObject tempTarget)
    {
        targetObject = tempTarget.transform;
        isWandering = false; // make it not targetable 
        isHugging = true; // start hugging
        isDone = false;
    }

    public void MoveToTarget()
    {
        { 
            Vector3 dir = targetObject.position - rb.position; // Get the delta position 
            dir /= Time.fixedDeltaTime; // Get the velocity required to reach the target in the next frame
            dir = Vector3.ClampMagnitude(dir, speed); // Clamp that to the max speed
            rb.velocity = dir; // Apply that to the rigidbody
        }
    }

    // Koennte man besser machen z.B. mit rb.MoveRotation weil Physics
    // Rotation Stuff | get rotation direction | update vector | Rotate 
    public void RotateToTarget()
    {
        
        var targetRot = targetObject.position - transform.position;
        float singleStep = speed * Time.fixedDeltaTime;

        Vector3 newDir = Vector3.RotateTowards(rb.velocity, targetRot, singleStep, 0f);

        // only rotate on y-axis
        newDir.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newDir), 4f * Time.deltaTime);
    }

    public void StopGhost()
    {
        rb.velocity = Vector3.zero; // stop when close to target 
        rb.rotation = Quaternion.Slerp(transform.rotation, startingDir, 8f * Time.deltaTime); // reset roation 
        isWandering = false;
    }

    IEnumerator Hugging(float hugTime) 
    {
        // Set Hugging Canvas and do Stuff ---- ADD IN HUGGING STUFF 
        targetObject.GetComponent<PlayerInfos>().ShowHuggingUI();
        this.GetComponent<PlayerInfos>().ShowHuggingUI();
        startedHugging = true;
        yield return new WaitForSeconds(hugTime);
        targetObject.GetComponent<PlayerInfos>().HideHuggingUI();
        this.GetComponent<PlayerInfos>().HideHuggingUI();
        startedHugging = false;
        targetObject = null;
        isHugging = false;
        isDone = true;
        yield break;
       
    }
}
