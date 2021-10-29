using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client
{
    private static Socket m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    
    public Client()
    {
        m_clientSocket.Connect(IPAddress.Parse("127.0.0.1"), 27015);
    }
    
    public void SendMessage(string msg)
    {
        byte[] buffer = new byte[1024];
        buffer = Encoding.UTF8.GetBytes(msg);

        m_clientSocket.Send(buffer);

        byte[] recvBuffer = new byte[1024];
        int receive = m_clientSocket.Receive(recvBuffer);
        byte[] data = new byte[receive];
        Array.Copy(recvBuffer, data, receive);
        
        Debug.Log(Encoding.ASCII.GetString(data));

    }
}
