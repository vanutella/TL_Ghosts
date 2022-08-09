using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(UserListHandler))]
public class TwitchClient : MonoBehaviour
{
    public Client client;
    // Channel to connect to
    private string channel_name = "vanutella";

    private UserListHandler listHandler;

    public string[] AllCommands;
    // public Texture myTexture;
    // public RawImage myImage;
    public enum CommandName
    {
        hug,
        jump
    }

    void Start()
    {
        // This script should run in the background
        Application.runInBackground = true;

        // Set up Bot and tell it which channel to join
        ConnectionCredentials credentials = new ConnectionCredentials("vanu_bot", Secrets.bot_access_token);
        client = new Client();
        client.Initialize(credentials, channel_name);

        // Event subscribing to catch changes 
        client.OnMessageReceived += ReceivedMessageFunction;

        // connect bot to the channel
        client.Connect();


        // Get List Handler Script
        listHandler = GetComponent<UserListHandler>();
    }

    private void ReceivedMessageFunction(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
    {
        //Debug.Log("The bot just read a message in chat: " + e.ChatMessage.Message);


        // Get Color
        Color UserColor = GetUserColor(e);
        // Check if user is already spawned as Character
        listHandler.CheckUserList(e.ChatMessage.Username, int.Parse(e.ChatMessage.UserId), UserColor);

        //if (e.ChatMessage.EmoteSet.Emotes.Count > 0)
        //{
        //    Debug.Log(e.ChatMessage.EmoteSet.Emotes[0].Name);
        //  //  StartCoroutine(GetTexture(e.ChatMessage.EmoteSet.Emotes[0].ImageUrl));

        //}



        // Check Commands
        if (StartsWithCommand(e.ChatMessage.Message))
        {
            CheckCommand(AllCommands, e);
        }
        else
        {
            //    Debug.Log("Should Display Message");
            listHandler.ShowMessage(e.ChatMessage);
        }
    }

    // Whe there is an emote, get Image
    //IEnumerator GetTexture(string emoteURL)
    //{
    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture(emoteURL);
    //    yield return www.SendWebRequest();

    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

    //    }
    //}

    Color GetUserColor(TwitchLib.Client.Events.OnMessageReceivedArgs e)
    {
        if (e.ChatMessage.ColorHex.StartsWith("#"))
        {
            Color textcolorRGB;
            ColorUtility.TryParseHtmlString(e.ChatMessage.ColorHex, out textcolorRGB);
            return textcolorRGB;

        }
        else
        {
            // get random color and convert to hex
            Color tempC = (new Color(
                           Random.Range(0f, 1f),
                           Random.Range(0f, 1f),
                           Random.Range(0f, 1f)
                           ));
            return tempC;
        }

    }

    string randomColor()
    {
        return string.Concat(Enumerable.Range(0, 6));
    }

    private bool StartsWithCommand(string message)
    {
        if (message.StartsWith("!"))
        {
            return true;
        }
        else return false;
    }

    // Loop trough all commands in Array
    void CheckCommand(string[] commands, TwitchLib.Client.Events.OnMessageReceivedArgs e)
    {
        string message = e.ChatMessage.Message;
        for (int i = 0; i < commands.Length; i++)
        {
            if (message.Contains(commands[i]))
            {
                // Extract command name without ! 
                string CommandName = message.Substring(1, commands[i].Length);

                string CommandAddOn;
                //  client.SendMessage(client.JoinedChannels[0], "Command detected " + CommandName);
                if (CommandName == "hug")
                {
                    if (message.Length > CommandName.Length + 1)
                    {

                        // Extract name behind command
                        CommandAddOn = e.ChatMessage.Message.Substring(commands[i].Length + 2);


                        if (CommandAddOn != null)
                        {
                            //  Debug.Log("Found Add On " + CommandAddOn);

                            if (CommandAddOn == "random")
                            {
                                Debug.Log("Looking for random target");
                                listHandler.LookingForRandomTarget(e.ChatMessage.Username);
                            }
                            else
                            {
                                // beginnt es mit @??? 
                                if (CommandAddOn.StartsWith("@"))
                                {
                                    string CommandAddOnSubstring = CommandAddOn.Substring(1);
                                    listHandler.LookingForHugTarget(e.ChatMessage.Username, CommandAddOnSubstring);
                                }
                                else { listHandler.LookingForHugTarget(e.ChatMessage.Username, CommandAddOn); }
                            }
                        }

                    }
                    else { client.SendMessage(client.JoinedChannels[0], "Ein anderer Nutzer muss erwähnt werden. '!hug username'"); };
                }
                else if (CommandName.Equals("hype"))
                {
                    // Do Hype Stuff for this Player User 
                    listHandler.StartHype(e.ChatMessage.Username);
                }

                else if (CommandName.Equals("color"))
                {
                    if (message.Length > CommandName.Length + 1)
                    {
                        CommandAddOn = e.ChatMessage.Message.Substring(commands[i].Length + 2);
                        if (CommandAddOn.Equals("white"))
                        {

                        }
                        else if (CommandAddOn.Equals("blue"))
                        {

                        }
                    }
                }
                else if (CommandName.Equals("dance"))
                {
                    Debug.Log("Found Mass Hype Command akaka dance <3 ");
                    if (e.ChatMessage.UserType == TwitchLib.Client.Enums.UserType.Broadcaster || e.ChatMessage.UserType == TwitchLib.Client.Enums.UserType.Moderator)
                    {

                        listHandler.StartMassHype();
                    }
                }
                return;
            }
        }
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    OnButtonPress();
        //}


    }

    public void OnButtonPress()
    {
        client.SendMessage(client.JoinedChannels[0], "Ich kann Nachrichten durch mein Unity Projekt senden! BobDance ");

    }



    private void CheckmessageSenderStatus()
    {

        //if(e.ChatMessage.UserType == TwitchLib.Client.Enums.UserType.Broadcaster)
        // {
        //    client.SendMessage(client.JoinedChannels[0], "Die Nachricht kam von Vanu!!!(Broadcaster) vanuHype");
        // }
        //  if (e.ChatMessage.UserType == TwitchLib.Client.Enums.UserType.Moderator)
        // {
        //    client.SendMessage(client.JoinedChannels[0], "Die Nachricht kam von einem Mod! Macht euren Job Kappa vanuHype");
        // }
    }
}
