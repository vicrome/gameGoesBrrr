﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AutoHostClient : MonoBehaviour
{

    [SerializeField] NetworkManager networkManager;

    private void Start()
    {
        if (Application.isBatchMode)
        {
            Debug.Log("Client connected");
            networkManager.StartClient();
        }
        else
        {
            Debug.Log("Server starting");
        }
    }
    
    public void JoinLocal() 
    {
        networkManager.networkAddress = "localhost";
        networkManager.StartClient();
    }
}
