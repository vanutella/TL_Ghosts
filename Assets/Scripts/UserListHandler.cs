using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using UnityEngine;

public class UserListHandler : MonoBehaviour
{
    private TwitchClient _client;
    private PlayerInfos Player_Infos; // For Text Stuff, Stats, Message Bubbles, etc.

    public List<UserClass> ChattedUsers = new List<UserClass>(); // User Class Object that stores Info about Users that Chat
    public List<GameObject> SpawnedChars = new List<GameObject>(); // List of all Chars that are active now

    public GameObject[] PlayerPrefabs;
    

    void Start()
    {
        _client = GetComponent<TwitchClient>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckUserList(string name, int id, Color color)
    {
        // Are there already Users that have chatted? 
        if (ChattedUsers.Count <= 0){
            GameObject newGO = Instantiate(PlayerPrefabs[randomIndex()], new Vector3(7, 7, -25), transform.rotation * Quaternion.Euler(0f, 180f, 0f));
            ChattedUsers.Add(new UserClass(name, id, newGO));
            SpawnedChars.Add(newGO);
            newGO.name = name;

            Player_Infos = newGO.GetComponent<PlayerInfos>();
            if (Player_Infos)
            {
                Player_Infos.charPrefab = newGO; // Set User GameObject

                Player_Infos.userNameLabel.color = color;
                Player_Infos.username = name; // Set User Name
                Player_Infos.id = id.ToString(); // Set User ID
            }

            return; // Exit when first Char was added
        }

        SearchUserList(name, id, color);
    }

    public void SearchUserList(string name, int id, Color color)
    {
        
        // Query 
        UserClass userItem = ChattedUsers.Find(x => x.UserID == id);

        if (userItem == null) // if chatter wasnt found yet, create a new player and add to chatter list
        {
            GameObject newGo = Instantiate(PlayerPrefabs[randomIndex()], new Vector3(Random.Range(-5, 6), Random.Range(0, 7), Random.Range(-28, -24)),transform.rotation * Quaternion.Euler(0f, 180f, 0f));
            ChattedUsers.Add(new UserClass(name, id, newGo));
            SpawnedChars.Add(newGo);
            newGo.name = name;
            Player_Infos = newGo.GetComponent<PlayerInfos>();
            // do sth when found script on GO
            if (Player_Infos != null)
            {

                Player_Infos.charPrefab = newGo; // Set User GameObject
                
                Player_Infos.userNameLabel.color = color;
                Player_Infos.username = name; // Set User Name
                Player_Infos.id = id.ToString(); // Set User ID
                Player_Infos.messageTimer = 0;
                
            }
        }
        else
        {
            // Search Player list here 
            SearchPlayerList(name, id, color);
        }
    }

    
    public void SearchPlayerList(string name, int id, Color color)
    {
        // Query 
        GameObject playerItem = SpawnedChars.Find(x => x.name == name);

        if (playerItem == null)
        {
            // Spawn and set script
            GameObject newGo = Instantiate(PlayerPrefabs[randomIndex()], new Vector3(Random.Range(-5, 6), Random.Range(0, 7), Random.Range(-28, -24)), transform.rotation * Quaternion.Euler(0f, 180f, 0f));
            ChattedUsers.Add(new UserClass(name, id, newGo));
            SpawnedChars.Add(newGo);
            newGo.name = name;
            Player_Infos = newGo.GetComponent<PlayerInfos>();
            // do sth when found script on GO
            if (Player_Infos != null)
            {

                Player_Infos.charPrefab = newGo; // Set User GameObject

                Player_Infos.userNameLabel.color = color;
                Player_Infos.username = name; // Set User Name
                Player_Infos.id = id.ToString(); // Set User ID
                Player_Infos.messageTimer = 0;
            }
        }
        else
        {
            Debug.Log("Already got a User with this name in the scene");
            // Set time for last message of playerItem

        }
    }

    private int randomIndex()
    {
        int tempRandom = Random.Range(0, PlayerPrefabs.Length);
        return tempRandom;
    }

    public void LookingForTarget(string senderName, string targetName)
    {
        
        GameObject targetItem = SpawnedChars.Find(x => x.name == targetName);
        GameObject senderItem = SpawnedChars.Find(x => x.name == senderName);
        Debug.Log(senderName + " " + targetName + " Found: " + senderItem + " " + targetItem);
        if (targetItem == null)
        {
            _client.client.SendMessage(_client.client.JoinedChannels[0], "No Player found. Use '!hug username'");
        }
       else if(targetItem != null)
        {
            Debug.Log("Found Player " + targetItem.name);
            // Stop Target, Set Target, (Start Coroutine?)
            senderItem.gameObject.GetComponent<GhostMovement>().target = targetItem.transform;
            senderItem.gameObject.GetComponent<GhostMovement>().isWandering = false;
           
            targetItem.gameObject.GetComponent<GhostMovement>().isWandering = false;
            targetItem.gameObject.GetComponent<GhostMovement>().isHugging = true;
            
        }
        
    }

    public void ShowMessage(ChatMessage message)
    {
        GameObject senderItem = SpawnedChars.Find(x => x.name == message.Username);
        if (senderItem)
        {
            Debug.Log("Found User");
            senderItem.gameObject.GetComponent<PlayerInfos>().message = message.Message;
        }
    }
}
