using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInfos : MonoBehaviour
{
    public string username;
    public string id;
    public GameObject selectedChar;
    public GameObject charPrefab;
    public float timeLeft;
    public float timeDespawn;
    public string message;
    public GameObject messageBubble;
    public TMPro.TMP_Text messageLabel;
    public TMPro.TMP_Text userNameLabel;
    public Image heart;

    public float messageTimer;
    public float showTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        userNameLabel.text = username;
        ShowMessageBubble();
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

    void SetBubbleVisibility()
    {
        messageBubble.SetActive(false);

    }
    
}
