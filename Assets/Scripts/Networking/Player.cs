using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public string DisplayName = "Loading...";

    [SyncVar]
    public RPG_Class rpgClass;

    private GameNetworkManager room;

    private GameNetworkManager Room
    {
        get
        {
            if (room != null) { return room; }

            else { return room = NetworkManager.singleton as GameNetworkManager; }
        }
    }
  

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        Room.GamePlayers.Add(this);
    }

    public override void OnNetworkDestroy()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.DisplayName = displayName;
    }
}
