using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserClass 
{
    public string UserName;
    public int UserID;
    public float timeSinceLastM;

    public UserClass (string username, int id, GameObject go = null)
    {
        UserName = username;
        this.UserID = id;
    }
}
