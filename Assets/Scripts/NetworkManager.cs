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

public class NetworkManager : MonoBehaviour
{
    private static bool game_active = true;
    HttpClient client = new HttpClient();
    Lobby lobby;

    // Start is called before the first frame update
    void Start()
    {
        client.BaseAddress = new Uri("http://localhost:8080");
        getPrimedLobby();
    }

    // Update is called once per frame
    void Update()
    {

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
        Thread socket_thread1 = new Thread(StartServer);
        socket_thread1.Start(lobby.port1);
        Thread socket_thread2 = new Thread(StartServer);
        socket_thread2.Start(lobby.port2);
    }

    public static void Enqueue(byte[] packet)
    {

    }

    public static void StartServer(object port_obj)
    {
        int port = (int)port_obj;
     
        // Get Host IP Address that is used to establish a connection
        // In this case, we get one IP address of localhost that is IP : 127.0.0.1
        // If a host has multiple addresses, you will get a list of addresses
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

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

            // Incoming data from the client.
            string data = null;
            byte[] bytes = null;

            while (true)
            {
                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if (data.IndexOf("<EOF>") > -1)
                {
                    break;
                }
            }

            byte[] msg = Encoding.ASCII.GetBytes(data);
            handler.Send(msg);
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        Debug.Log("\n Press any key to continue...");
        Console.ReadKey();
    }
}