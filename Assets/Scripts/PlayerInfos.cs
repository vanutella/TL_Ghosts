using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInfos : MonoBehaviour
{
    LoadScenes sceneHandler;
    UserListHandler listHandler;
    public string username;
    public string id;
    public GameObject selectedChar;
    public GameObject charPrefab;
    public float timeLeft = 600;
    public float timeDespawn = 600;
    public string message;
    public GameObject messageBubble;
    public TMPro.TMP_Text messageLabel;
    public TMPro.TMP_Text userNameLabel;
    public Image heart;
    public RawImage EmoteIMG;
    

    public float messageTimer;
    public float showTime = 3f;

    private MeshRenderer MeshRen;
    // Start is called before the first frame update
    void Start()
    {
        sceneHandler = GameObject.FindObjectOfType <LoadScenes>();
        listHandler = GameObject.FindObjectOfType<UserListHandler>();

        if(listHandler != null)
        {
            Debug.Log("Found List Handler");
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        if(charPrefab != selectedChar)
        {
            listHandler.ReplacePlayer(this.gameObject);            
        }

        userNameLabel.text = username;

        if (sceneHandler.activeMessageBubble)
        {
            ShowMessageBubble();
        }
        else
        {
            messageBubble.SetActive(false);
        }
        

        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else if(timeLeft <= 0)
        {
            Debug.Log("Destroy Player. Time is up " + timeLeft);
            DestroyPlayer();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            timeLeft = 20f;
        }

    }
    public void ShowMessageBubble()
    {
        if(messageTimer < showTime)
        {
            // show message bubble panel
            messageBubble.SetActive(true);
            messageLabel.text = message;
            // count time
            messageTimer += Time.deltaTime;
        }
        else {
            // hide the message bubble panel
            // messageTimer = 0;
            messageBubble.SetActive(false);
        }
           
    }

    //void SetBubbleVisibility()
    //{
    //    messageBubble.SetActive(false);
    //}

    public GameObject HeartParticles;
    public GameObject SkullParticles;
    public GameObject HypeParticles;
    public void ShowHuggingUI()
    {
       // Debug.Log("Should Show " + username + "'s heart now");
      //  messageBubble.SetActive(false);
        Instantiate(HeartParticles, this.transform);
        heart.gameObject.SetActive(true);
    }

    public void HideHuggingUI()
    {
       // Debug.Log("Should Hide " + username + "'s hugging heart now");
       // messageBubble.SetActive(true);
        heart.gameObject.SetActive(false);
    }
    
    // Wenn eine neue Nachricht kommt, wird diese Funktion aufgerufen
    public void ResetTimer()
    {
        timeLeft = timeDespawn;
    }

    public void DestroyPlayer()
    {   Vector3 DeathPos = transform.position;
        Instantiate(SkullParticles, DeathPos, Quaternion.identity);
        listHandler.DeletePlayer(username);
        Destroy(this.gameObject);
    }
}
