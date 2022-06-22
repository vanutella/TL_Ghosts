using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Unity;
using TwitchLib.Api.Core.Models.Undocumented.Chatters;
using TwitchLib.Client.Models;
using TwitchLib.Api.Helix;
using System;
using TwitchLib.Api.Helix.Models.ChannelPoints.CreateCustomReward;

public class TwitchApi : MonoBehaviour
{

    
    UnityFollowerService followerService;
    TwitchClient _client;
    public Api api;

    List<string> channels_to_manage = new List<string>();
    private string rewardid5;

    void Start()
    {
        // Run this App in the background/when minimized
        Application.runInBackground = true;

        // Inizialize the API Connection
        api = new Api();
        api.Settings.AccessToken = Secrets.bot_access_token;
        api.Settings.ClientId = Secrets.client_id;

        // Zuweisen des Scripts um auch die richtigen Variablen Werte nutzen zu können
        _client = GetComponent<TwitchClient>();

        // Do RewardStuff

        //// Follower Event Stuff 
        //channels_to_manage.Add(Secrets.channel_id); // Add channel_id to a list bc it requires a list
        //followerService = new UnityFollowerService(api, 30,25); // Inverval & ? QueryCount?? idk
        //followerService.SetChannelsById(channels_to_manage);

    }

    // Update is called once per frame
    void Update()
    {
        // ask the Api for Information about my Chatters
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            api.Invoke(
                api.Undocumented.GetChattersAsync(_client.client.JoinedChannels[0].Channel)
                , GetChatterListCallback
                );
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

        }
    }


    // Get List of all Chatters 
    private void GetChatterListCallback(List<ChatterFormatted> listOfChatters)
    {
        // Get count of people in Chat (Viewers)
        Debug.Log("List of " + listOfChatters.Count + " Viewers: ");
        // Go through each item in List of Chatters and print their Usernames
        foreach (var chatterObject in listOfChatters)
        {
            // Wenn VIP
            if(chatterObject.UserType == TwitchLib.Api.Core.Enums.UserType.VIP)
            {
                _client.client.SendMessage(_client.client.JoinedChannels[0].Channel, "VIP User " + chatterObject.Username + " ist im Chat!");
            }
            // Wenn Moderator
            else if (chatterObject.UserType == TwitchLib.Api.Core.Enums.UserType.Moderator)
            {
                
                _client.client.SendMessage(_client.client.JoinedChannels[0].Channel, "Moderator " + chatterObject.Username + " ist im Chat!");
            }
            Debug.Log(chatterObject.Username);
        }
        
    }
}
