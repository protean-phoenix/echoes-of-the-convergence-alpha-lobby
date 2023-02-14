using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] private GameObject rocket;
    private static bool game_active = true;
    public static Queue<byte[]> buffer;
    int count = 0;
    private static readonly object _lock = new object();

    // Start is called before the first frame update
    void Start()
    {
        buffer = new Queue<byte[]>();
        //NOTE: Opening a socket will cause the current process to hang
        //When starting a networking task, start it in a new thread
        Task.Factory.StartNew(StartClient, TaskCreationOptions.LongRunning);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void onApplicationQuit()
    {
        game_active = false;
    }

    public static void Enqueue(byte[] packet)
    {
        lock (_lock)
        {
            buffer.Enqueue(packet);
        }
    }

    public static void StartClient()
    {
        byte[] bytes = new byte[1024];

        try
        {
            // Connect to a Remote server
            // Get Host IP Address that is used to establish a connection
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1
            // If a host has multiple addresses, you will get a list of addresses
            IPAddress ipAddress = new IPAddress(new byte[] { 127, 0, 0, 1 });
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP  socket.
            Socket sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.
            try
            {
                // Connect to Remote EndPoint
                sender.Connect(remoteEP);

                while (game_active) {
                    // Encode the data string into a byte array.
                    byte[] payload = null;
                    lock (_lock)
                    {
                        if (buffer.Count > 0) { 
                            payload = buffer.Dequeue();
                        }
                    }
                    // Send the data through the socket.
                    if(payload != null) { 
                        int bytesSent = sender.Send(payload);
                    }
                }

                // Release the socket.
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch (ArgumentNullException ane)
            {
                Debug.Log("ArgumentNullException : " + ane.ToString());
            }
            catch (SocketException se)
            {
                Debug.Log("SocketException : " + se.ToString());
            }
            catch (Exception e)
            {
                Debug.Log("Unexpected exception : " + e.ToString());
            }

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

    }
}