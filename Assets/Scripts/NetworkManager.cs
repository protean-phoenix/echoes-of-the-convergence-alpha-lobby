using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json;

public class Lobby
{
    public int port1 { get; set; }
    public int port2 { get; set; }
    public String player1 { get; set; }
    public String player2 { get; set; }
}

public class ThreadInitInfo
{
    public int port { get; set; }
    public int player { get; set; }
}

public class SocketInfo
{
    public Socket socket { get; set; }
    public int player { get; set; }
}

public class NetworkManager : MonoBehaviour
{
    private static bool game_active = true;
    HttpClient client = new HttpClient();
    Lobby lobby;
    private static Thread playerSocketThread1;
    private static Thread playerSocketThread2;

    private static Thread playerSendThread1;
    private static Queue<byte[]> outBuffer1;
    private static Thread playerSendThread2;
    private static Queue<byte[]> outBuffer2;

    private static Thread playerReceiveThread1;
    private static Queue<byte[]> inBuffer1;
    private static Thread playerReceiveThread2;
    private static Queue<byte[]> inBuffer2;
    public static bool started = false;
    private float timer = 0;
    private byte count = 0;

    // Start is called before the first frame update
    void Start()
    {
        client.BaseAddress = new Uri("http://localhost:8080");
        outBuffer1 = new Queue<byte[]>();
        outBuffer2 = new Queue<byte[]>();
        getPrimedLobby();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSendThread1 != null && playerSendThread2 != null)
        {
            started = true;
        }
    }

    void onApplicationQuit()
    {
        game_active = false;
    }

    async void getPrimedLobby()
    {
        var response = await client.GetAsync("get-primed");
        String data = await response.Content.ReadAsStringAsync();
        Debug.Log(data);
        lobby = JsonConvert.DeserializeObject<Lobby>(data);

        ThreadInitInfo info1 = new ThreadInitInfo();
        info1.port = lobby.port1;
        info1.player = 1;
        playerSocketThread1 = new Thread(StartServer);
        playerSocketThread1.Start(info1);

        ThreadInitInfo info2 = new ThreadInitInfo();
        info2.port = lobby.port2;
        info2.player = 2;
        playerSocketThread2 = new Thread(StartServer);
        playerSocketThread2.Start(info2);
    }

    public static void StartServer(object info_obj)
    {
        ThreadInitInfo info = (ThreadInitInfo)info_obj;

        // Get Host IP Address that is used to establish a connection
        // In this case, we get one IP address of localhost that is IP : 127.0.0.1
        // If a host has multiple addresses, you will get a list of addresses
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, info.port);

        try
        {

            // Create a Socket that will use Tcp protocol
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // A Socket must be associated with an endpoint using the Bind method
            listener.Bind(localEndPoint);
            // Specify how many requests a Socket can listen before it gives Server busy response.
            // We will listen 10 requests at a time
            listener.Listen(10);

            Debug.Log("Waiting for a connection...");
            Socket handler = listener.Accept();

            Debug.Log("Connection established!");

            if (info.player == 1)
            {
                SocketInfo s_info = new SocketInfo();
                s_info.socket = handler;
                s_info.player = 1;
                playerSendThread1 = new Thread(SendThread);
                playerSendThread1.Start(s_info);
            }else if(info.player == 2)
            {
                SocketInfo s_info = new SocketInfo();
                s_info.socket = handler;
                s_info.player = 2;
                playerSendThread2 = new Thread(SendThread);
                playerSendThread2.Start(s_info);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        Debug.Log("\n Press any key to continue...");
        Console.ReadKey();
    }

    public static void SendThread(object info)
    {
        SocketInfo s_info = (SocketInfo)info;
        Socket handler = s_info.socket;
        int player = s_info.player;
        try
        {
            while (game_active)
            {
                if(player == 1)
                {
                    while(outBuffer1.Count > 0)
                    {
                        handler.Send(outBuffer1.Dequeue());
                    }
                }
                else if (player == 2)
                {
                    while (outBuffer2.Count > 0)
                    {
                        handler.Send(outBuffer2.Dequeue());
                    }
                }
            }

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

    }

    public static void sendPlayer1(byte[] msg)
    {
        outBuffer1.Enqueue(msg);
    }

    public static void sendPlayer2(byte[] msg)
    {
        outBuffer2.Enqueue(msg);
    }

    public static void broadcast(byte[] msg)
    {
        sendPlayer1(msg);
        sendPlayer2(msg);
    }
}