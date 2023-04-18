using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using UnityEngine;
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
    public String ip { get; set; }
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
        if (Utils.status == TestingStatus.TEST) {
            // !!! LOCAL TESTING !!!
            client.BaseAddress = new Uri("http://localhost:8080");
        } else if (Utils.status == TestingStatus.PROD) {
            // !!! PRODUCTION !!!
            client.BaseAddress = new Uri("http://5.161.206.210:8080");
        }
           
        outBuffer1 = new Queue<byte[]>();
        outBuffer2 = new Queue<byte[]>();
        inBuffer1 = new Queue<byte[]>();
        inBuffer2 = new Queue<byte[]>();
        getPrimedLobby();
    }

    // Update is called once per frame
    void Update()
    {
        if (!started && playerSendThread1 != null && playerSendThread2 != null)
        {
            started = true;
        }
        while(inBuffer1.Count > 0)
        {
            processByte(inBuffer1.Dequeue(), 1);
        }
        while (inBuffer2.Count > 0)
        {
            processByte(inBuffer2.Dequeue(), 2);
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
        info1.player = 0;
        info1.ip = lobby.player1;
        playerSocketThread1 = new Thread(StartServer);
        playerSocketThread1.Start(info1);

        ThreadInitInfo info2 = new ThreadInitInfo();
        info2.port = lobby.port2;
        info2.player = 1;
        info2.ip = lobby.player2;
        playerSocketThread2 = new Thread(StartServer);
        playerSocketThread2.Start(info2);
    }

    public static void StartServer(object info_obj)
    {
        ThreadInitInfo info = (ThreadInitInfo)info_obj;

        IPAddress ipAddress = null;

        if (Utils.status == TestingStatus.TEST) {
            // !!! LOCAL TESTING !!!
            IPHostEntry host = Dns.GetHostEntry("localhost");
            ipAddress = host.AddressList[0];
        }else if(Utils.status == TestingStatus.PROD) {  
            // !!! PRODUCTION !!!
            ipAddress = System.Net.IPAddress.Parse("5.161.206.210"); 
        }

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

            if (info.player == 0)
            {
                SocketInfo s_info = new SocketInfo();
                s_info.socket = handler;
                s_info.player = 0;
                playerSendThread1 = new Thread(SendThread);
                playerSendThread1.Start(s_info);
                playerReceiveThread1 = new Thread(ReceiveThread);
                playerReceiveThread1.Start(s_info);
            }else if(info.player == 1)
            {
                SocketInfo s_info = new SocketInfo();
                s_info.socket = handler;
                s_info.player = 1;
                playerSendThread2 = new Thread(SendThread);
                playerSendThread2.Start(s_info);
                playerReceiveThread2 = new Thread(ReceiveThread);
                playerReceiveThread2.Start(s_info);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            byte[] cmd = new byte[2];
            cmd[0] = 255;
            cmd[1] = 0;
            if (info.player == 0)
            {
                inBuffer1.Enqueue(cmd);
            }
            else if (info.player == 1)
            {
                inBuffer2.Enqueue(cmd);
            }
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
                if(player == 0)
                {
                    while(outBuffer1.Count > 0)
                    {
                        handler.Send(outBuffer1.Dequeue());
                    }
                }
                else if (player == 1)
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
            //send command to shut down the lobby
            byte[] cmd = new byte[2];
            cmd[0] = 255;
            cmd[1] = 0;
            if (player == 0)
            {
                inBuffer1.Enqueue(cmd);
            }
            else if (player == 1)
            {
                inBuffer2.Enqueue(cmd);
            }
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

    public static void ReceiveThread(object info)
    {
        SocketInfo s_info = (SocketInfo)info;
        Socket socket = s_info.socket;
        int player = s_info.player;
        try
        {
            while (game_active)
            {
                byte[] bytes = new byte[1024];
                int bytesRec = socket.Receive(bytes);
                if (bytesRec > 0)
                {
                    byte[] msg = new byte[bytesRec];
                    Array.Copy(bytes, 0, msg, 0, bytesRec);
                    if (player == 0)
                    {
                        inBuffer1.Enqueue(msg);
                    }
                    else if (player == 1)
                    {
                        inBuffer2.Enqueue(msg);
                    }
                    Debug.Log(BitConverter.ToString(msg));
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            //send command to shut down the lobby
            byte[] cmd = new byte[2];
            cmd[0] = 255;
            cmd[1] = 0;
            if (player == 0)
            {
                inBuffer1.Enqueue(cmd);
            }
            else if (player == 1)
            {
                inBuffer2.Enqueue(cmd);
            }
        }
    }

    static void processByte(byte[] input, int player)
    {
        switch (input[0]) //first byte
        {
            case 0: //basic requests
                switch (input[1])
                { //second byte
                    case 0: //retarget
                        GameObject origin_ship_obj = ShipScript.getShipById((int)input[2]);
                        GameObject origin_room_obj = origin_ship_obj.GetComponent<ShipScript>().getRoomById(input[3]);
                        GameObject target_ship_obj = ShipScript.getShipById((int)input[4]);
                        GameObject target_room_obj = target_ship_obj.GetComponent<ShipScript>().getRoomById(input[5]);
                        WeaponRoomScript origin_room = origin_room_obj.GetComponent<WeaponRoomScript>();
                        origin_room.SetTarget(target_room_obj);

                        break;
                } 
                break;
            case 255: //system commands
                switch (input[1]) //second byte
                {
                    case 0://shutdown
                        Application.Quit();
                        break;
                }
                break;
        }
    }
}