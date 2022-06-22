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
	
	private Vector3 eulerAngleVelocity;
	private Quaternion startingDir;

	public bool isWandering, isHugging, isWaiting;

	void Awake()
	{
		playerInfos = GetComponent<PlayerInfos>();

		rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
		rb.maxAngularVelocity = 60f; //set it to something pretty high so it can actually follow properly!

		eulerAngleVelocity = new Vector3(0, 100, 0);

		startingDir = transform.rotation;
		Debug.Log(startingDir);
		isWandering = true;
	}

    private void Update()
    {
		if(target != null) // placeholder if statement, replace with boolean for state machines
        {
			// IF in Follow-Mode, find Target, calculate dist, do  more logic stuff 
			dist = Vector3.Distance(rb.transform.position, target.position);
        }

        if (target != null && isHugging)
        {
			Debug.Log("Started Coroutine " + this.name);
			StartCoroutine(hugging());
		}

	}
    void FixedUpdate()
	{
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
		else if(target == null && isHugging)
        {
			Reset();
        }

		// has a target that it is moving to
		else if (target != null)
        {
			if (dist > playerRadius)
			{
				Move();

				Rotate();
			}
			else if(dist <= playerRadius)
			{
				Reset();
				isHugging = true;
			}
			
		}
				

		
	}

	public bool startedCoroutine = false;
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
		playerInfos.userNameLabel.gameObject.SetActive(false);
		playerInfos.messageBubble.SetActive(false);
		playerInfos.heart.gameObject.SetActive(true);
		target.GetComponent<PlayerInfos>().userNameLabel.gameObject.SetActive(false);
		target.GetComponent<PlayerInfos>().messageBubble.SetActive(false);
		target.GetComponent<PlayerInfos>().heart.gameObject.SetActive(true);
		yield return new WaitForSeconds(3f);
		//then do this
		if (target != null)
		{	playerInfos.userNameLabel.gameObject.SetActive(true);
			playerInfos.messageBubble.SetActive(true);
			playerInfos.heart.gameObject.SetActive(false);
			target.GetComponent<PlayerInfos>().userNameLabel.gameObject.SetActive(true); 
			target.GetComponent<PlayerInfos>().messageBubble.SetActive(true);
			target.GetComponent<PlayerInfos>().heart.gameObject.SetActive(false);
			target.GetComponent<GhostMovement>().isHugging = false;
			target.GetComponent<GhostMovement>().isWandering = true;
			isHugging = false;
			isWandering = true;
			target = null;
		}
		
    }
    private void Move()
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


	// Könnte man besser machen z.B. mit rb.MoveRotation weil Physics
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

    private void Reset()
    {
		// stop when close to target 
		rb.velocity = Vector3.zero;
		// reset roation 
		rb.rotation = Quaternion.Slerp(transform.rotation, startingDir, 8f * Time.deltaTime);
	}
}
