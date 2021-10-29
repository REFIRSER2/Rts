using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Sirenix.Serialization;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public static ServerManager Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        Client client = new Client();

    }

    private void Update()
    {

    }

}
