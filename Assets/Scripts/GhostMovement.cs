using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    private PlayerInfos playerInfos;

    public Transform target;
    public float playerRadius;
    private float dist;

    private Rigidbody rb;
    public float speed;

   // private Vector3 eulerAngleVelocity;
    private Quaternion startingDir;

    public bool isWandering, isHugging = false, isWaiting;
    public bool startedCoroutine = false; // for the hover coroutine in fixedUpdate
    public bool isDone = false, startedHugging = false;
    void Awake()
    {
        playerInfos = GetComponent<PlayerInfos>();

        // Set Movement Variables 
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.maxAngularVelocity = 60f; //set it to something pretty high so it can actually follow properly!
     //   eulerAngleVelocity = new Vector3(0, 100, 0);

        startingDir = transform.rotation; // Save rotation 

        isWandering = true;
    }

    private void Update() // Habe ich ein Target, bewege User dorthin und starte hugging (hug bool set in fixedUpdate)
    {

        if (target != null) // placeholder if statement, replace with boolean for state machines
        {
            // IF in Follow-Mode, find Target, calculate dist, do  more logic stuff 
            dist = Vector3.Distance(rb.transform.position, target.position);
            target.GetComponent<GhostMovement>().isWandering = false;
        }

        if (target != null && isHugging)
        {
            if (startedHugging == false) // only call once
            {
                Debug.Log("Started Hug Coroutine with " + this.name + " and " + target.name);
                StartCoroutine(hugging()); // start hugging coroutine
                startedHugging = true;
            }
        }

    }

    void FixedUpdate() // Habe ich KEIN Target, wandere umher. 
                       // Habe ich ein target, dann Stoppe es und bewege darauf zu.
                       // Starte dann Hugging Process
    {
        // Hovering while no target 
        if (target == null && isWandering)
        {
            if (!startedCoroutine)
            {
                GetNewHoverForce();
                StartCoroutine(hovering(4f));
                startedCoroutine = true;
            }
            return;
        }

        // is a target from someone else 
        else if (target == null && isHugging)
        {
            ResetGhost();
        }

        // has a target that it is moving towards
        // Rotate towards while moving too
        else if (target != null)
        {
            isDone = false;
            if (dist > playerRadius)
            {
                Move();

                Rotate();
            }
            else if (dist <= playerRadius)
            {
                ResetGhost();
                isHugging = true; // start hugging in update
            }

        }


    }


    
    private IEnumerator hovering(float hoverTime)
    {
        yield return new WaitForSeconds(hoverTime);
        startedCoroutine = false;
    }
    void GetNewHoverForce()
    {
        rb.AddRelativeForce(Random.onUnitSphere * (10));
    }

    public float standingTime()
    {
        return Random.Range(1, 5);
    }

    IEnumerator hugging()
    {
        // set hugging images and wait...
        ActivatePlayerGUI();
        ActivateTargetGUI();

        yield return new WaitForSeconds(3f);

        //then do this aka hide hearts and start walking again
        if (target != null)
        {
            ResetPlayerGUI();
            ResetTargetGUI();
        }
    }

    public void ActivatePlayerGUI()
    {
        playerInfos.userNameLabel.gameObject.SetActive(false);
        playerInfos.messageBubble.SetActive(false);
        playerInfos.heart.gameObject.SetActive(true);
    }

    public void ResetPlayerGUI()
    {
        playerInfos.userNameLabel.gameObject.SetActive(true);
        playerInfos.messageBubble.SetActive(true);
        playerInfos.heart.gameObject.SetActive(false);
        isHugging = false;
        isWandering = true;
        startedHugging = false;
        isDone = true;
        Debug.Log("Done Hugging");
    }

    // Give target and start the coroutines in Update
    public void SetStartVariables(GameObject tempTarget)
    {
        target = tempTarget.transform;
        isWandering = false;

        target.GetComponent<GhostMovement>().isHugging = true;
        target.GetComponent<GhostMovement>().isWandering = false;
        Debug.Log("Setting everything to false and start the hug process");
    }

    public void ActivateTargetGUI()
    {
        target.GetComponent<PlayerInfos>().userNameLabel.gameObject.SetActive(false);
        target.GetComponent<PlayerInfos>().messageBubble.SetActive(false);
        target.GetComponent<PlayerInfos>().heart.gameObject.SetActive(true);
        target.GetComponent<GhostMovement>().isHugging = true;
        target.GetComponent<GhostMovement>().isWandering = false;
    }

    // start walking again
    public void ResetTargetGUI()
    {
        target.GetComponent<GhostMovement>().isHugging = false;
        target.GetComponent<GhostMovement>().isWandering = true;
        target.GetComponent<PlayerInfos>().userNameLabel.gameObject.SetActive(true);
        target.GetComponent<PlayerInfos>().messageBubble.SetActive(true);
        target.GetComponent<PlayerInfos>().heart.gameObject.SetActive(false);
        target = null;
    }

    public void Move()
    {
        {
            // Get the delta position  
            Vector3 dir = target.position - rb.position;
            // Get the velocity required to reach the target in the next frame
            dir /= Time.fixedDeltaTime;
            // Clamp that to the max speed
            dir = Vector3.ClampMagnitude(dir, speed);
            // Apply that to the rigidbody
            rb.velocity = dir;
        }
    }


    // Koennte man besser machen z.B. mit rb.MoveRotation weil Physics
    void Rotate()
    {
        // Rotation Stuff | get rotation direction | update vector | Rotate 
        var targetRot = target.position - transform.position;
        float singleStep = speed * Time.fixedDeltaTime;

        Vector3 newDir = Vector3.RotateTowards(rb.velocity, targetRot, singleStep, 0f);

        // only rotate on y-axis
        newDir.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newDir), 4f * Time.deltaTime);
    }

    private void ResetGhost()
    {

        // stop when close to target 
        rb.velocity = Vector3.zero;
        // reset roation 
        rb.rotation = Quaternion.Slerp(transform.rotation, startingDir, 8f * Time.deltaTime);
    }
}
