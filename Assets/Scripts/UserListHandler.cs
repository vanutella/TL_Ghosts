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
    public Transform SpawnPoint;

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
        if (IgnoredUsers(name)) { Debug.Log("Found ignored user."); return; }
        // Are there already Users that have chatted? 
        if (ChattedUsers.Count <= 0){
            Vector3 spawnCenter = SpawnPoint.position;
            float radius = 2.5f;
            Vector3 randomSpawnPoint = spawnCenter + (Vector3)(Random.insideUnitCircle * radius);
            Debug.Log(randomSpawnPoint);
            // Spawn and set script
            GameObject newGO = Instantiate(PlayerPrefabs[randomIndex()], randomSpawnPoint /*new Vector3(Random.Range(-5, 6), Random.Range(0, 7), Random.Range(-28, -24))*/, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
            
            ChattedUsers.Add(new UserClass(name, id, newGO));
            SpawnedChars.Add(newGO);
            newGO.name = name;

            Player_Infos = newGO.GetComponent<PlayerInfos>();
            if (Player_Infos)
            {
                Player_Infos.charPrefab = newGO; // Set User GameObject
                Player_Infos.selectedChar = newGO;
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
            Vector3 spawnCenter = SpawnPoint.position;
            float radius = 2.5f;
            Vector3 randomSpawnPoint = spawnCenter + (Vector3)(Random.insideUnitCircle * radius);
            Debug.Log(randomSpawnPoint);
            // Spawn and set script
            GameObject newGo = Instantiate(PlayerPrefabs[randomIndex()], randomSpawnPoint /*new Vector3(Random.Range(-5, 6), Random.Range(0, 7), Random.Range(-28, -24))*/, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
            
            ChattedUsers.Add(new UserClass(name, id, newGo));
            SpawnedChars.Add(newGo);
            newGo.name = name;
            Player_Infos = newGo.GetComponent<PlayerInfos>();
            // do sth when found script on GO
            if (Player_Infos != null)
            {

                Player_Infos.charPrefab = newGo; // Set User GameObject
                Player_Infos.selectedChar = newGo;
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
    public GameObject FindPlayer(string Username)
    {
        string name = Username.ToLower();
        GameObject playerItem = SpawnedChars.Find(x => x.name == name);
        if (playerItem != null) { 
        return playerItem;
        }
        else return null;
    }

    
    public void SearchPlayerList(string name, int id, Color color)
    {
        // Query 
        GameObject playerItem = SpawnedChars.Find(x => x.name == name);
        Debug.Log(SpawnedChars.Count);
        if (playerItem == null)
        {
            // random spawn point
            Vector3 spawnCenter = SpawnPoint.position;
            float radius = 2.5f;
            Vector3 randomSpawnPoint = spawnCenter+ (Vector3)(Random.insideUnitCircle * radius);
            Debug.Log(randomSpawnPoint);
            // Spawn and set script
            GameObject newGo = Instantiate(PlayerPrefabs[randomIndex()], randomSpawnPoint /*new Vector3(Random.Range(-5, 6), Random.Range(0, 7), Random.Range(-28, -24))*/, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
            
            ChattedUsers.Add(new UserClass(name, id, newGo));
            SpawnedChars.Add(newGo);
            newGo.name = name;
            Player_Infos = newGo.GetComponent<PlayerInfos>();
            
            // do sth when found script on GO
            if (Player_Infos != null)
            {

                Player_Infos.charPrefab = newGo; // Set User GameObject
                Player_Infos.selectedChar = newGo;
                Player_Infos.userNameLabel.color = color;
                Player_Infos.username = name; // Set User Name
                Player_Infos.id = id.ToString(); // Set User ID
                Player_Infos.messageTimer = 0;
                
            }
        }
        else
        {
         //   Debug.Log("Already got a User with this name in the scene");
            // Set time for last message of playerItem
            playerItem.GetComponent<PlayerInfos>().ResetTimer();
        }
    }



    private int randomIndex()
    {
        int tempRandom = Random.Range(0, PlayerPrefabs.Length);
        return tempRandom;
    }

    public void StartHype(string username)
    {
        GameObject playerObject = SpawnedChars.Find(x => x.name == username);
        GameObject hypeParticles = playerObject.GetComponent<PlayerInfos>().HypeParticles;
        Instantiate(hypeParticles, playerObject.transform);
        playerObject.GetComponentInChildren<Animator>().Play("Hype");
    }

    public void StartMassHype()
    {
        foreach(GameObject gO in SpawnedChars)
        {
            Debug.Log("Starting Mass Hype");
            GameObject hypeParticles = gO.GetComponent<PlayerInfos>().HypeParticles;
            Instantiate(hypeParticles, gO.transform);
            gO.GetComponentInChildren<Animator>().Play("Hype");
        }
    }

    // is there even a target available? Or is the name invalid?
    // return a sender AND target object and Enqueue Hug Action 
    public void LookingForHugTarget(string senderName, string targetName)
    {
        string tempTargetName = targetName.ToLower();
        string tempSenderName = senderName.ToLower();
        GameObject targetItem = SpawnedChars.Find(x => x.name == tempTargetName);
        GameObject senderItem = SpawnedChars.Find(x => x.name == tempSenderName);
       // Debug.Log(" Found objects: Sender: " + senderItem.name + " Target: " + targetItem.name);
        // Error Message if no taarget found
        if (targetItem == null)
        {
            _client.client.SendMessage(_client.client.JoinedChannels[0], "No Player found. Use '!hug username'");
        }

        // Found Target -> Stop Target, Set Target, (Start Coroutine?)
        else if (targetItem != null && targetItem.GetComponent<GhostState>().isTarget == false)
        {
           
            senderItem.gameObject.GetComponent<QueueHandler>().actionQueue.Enqueue(new HugAction(senderItem, targetItem));
           // senderItem.gameObject.GetComponent<GhostMovement>().SetStartVariables(targetItem);
        }
        else
        {
            _client.client.SendMessage(_client.client.JoinedChannels[0], "Could not hug right now, sorry.");
        }
        
    }

    public void LookingForRandomTarget(string senderName)
    {
        string tempSenderName = senderName.ToLower();
        GameObject senderItem = SpawnedChars.Find(x => x.name == tempSenderName);
        int randomTargetIndex = Random.Range(0, SpawnedChars.Count-1);
        if (SpawnedChars[randomTargetIndex].gameObject.name != tempSenderName)
        {
            GameObject targetItem = SpawnedChars[randomTargetIndex];
            senderItem.gameObject.GetComponent<QueueHandler>().actionQueue.Enqueue(new HugAction(senderItem, targetItem));
            string targetName = targetItem.name;
            _client.client.SendMessage(_client.client.JoinedChannels[0], "Hugging " + targetName);
        }
        else 
        {
            GameObject targetItem = SpawnedChars[SpawnedChars.Count-1].gameObject;

            senderItem.gameObject.GetComponent<QueueHandler>().actionQueue.Enqueue(new HugAction(senderItem, targetItem));
            string targetName = targetItem.name;
            _client.client.SendMessage(_client.client.JoinedChannels[0], "Hugging " + targetName); 
        }
    }

    public void ShowMessage(ChatMessage message)
    {
        GameObject senderItem = SpawnedChars.Find(x => x.name == message.Username);
        if (senderItem)
        {
          //  Debug.Log("Found User");
            senderItem.gameObject.GetComponent<PlayerInfos>().message = message.Message;
            senderItem.gameObject.GetComponent<PlayerInfos>().messageTimer = 0f;
        }
    }

    public void DeletePlayer(string playername)
    {
        GameObject playerToDelete = SpawnedChars.Find(x => x.name == playername);
        SpawnedChars.Remove(playerToDelete);
    }

    public string[] UsersToIgnore;
    public bool IgnoredUsers(string username)
    {
        foreach (string user in UsersToIgnore)
        {
            if (username.Equals(user)) {  return true; }
        }
        return false;
    }

    public void ReplacePlayer(GameObject PlayerToReplace)
    {
            GameObject playerItem = FindPlayer(PlayerToReplace.gameObject.name);
            Debug.Log("Found another Prefab. Should Change now");
            GameObject newGo = Instantiate(PlayerToReplace.GetComponent<PlayerInfos>().selectedChar, playerItem.gameObject.transform.position, playerItem.gameObject.transform.rotation);

            SpawnedChars.Remove(playerItem);
            newGo.name = PlayerToReplace.name;
            newGo.GetComponent<PlayerInfos>().selectedChar = newGo;
            newGo.GetComponent<PlayerInfos>().charPrefab = newGo;
            newGo.GetComponent<PlayerInfos>().username = PlayerToReplace.name;

            SpawnedChars.Add(newGo);
            Destroy(PlayerToReplace);


        
    }
}
