using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CoverShooter;
using UnityEngine;
using UnityEngine.UI;

public class NewLevelController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Areas;

    [SerializeField]
    private int StartLevel;
    private TcpListener server;
    private Thread serverThread;
    private bool isRunning = false;
    private int NowLevel;
    private float lastDDATime;
    private Queue<Action> mainThreadActions = new Queue<Action>();
    private object queueLock = new object();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < StartLevel; i++) {
            Areas[i].SetActive(true);
        }
        NowLevel = StartLevel;
        lastDDATime = Time.time;
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(20);
        StartServer("127.0.0.1", 5005);
    }

    void Update() {
        lock (queueLock)
        {
            while (mainThreadActions.Count > 0)
            {
                var action = mainThreadActions.Dequeue();
                action?.Invoke();
            }
        }
    }

    void OnApplicationQuit()
    {
        StopServer();
    }

    void StartServer(string ip, int port) {
        serverThread = new Thread(() =>
        {
            try
            {
                server = new TcpListener(IPAddress.Parse(ip), port);
                server.Start();
                isRunning = true;

                while (isRunning)
                {
                    if (server.Pending())
                    {
                        using (TcpClient client = server.AcceptTcpClient())
                        using (NetworkStream stream = client.GetStream())
                        {
                            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                            while (client.Connected && isRunning)
                            {
                                string data = reader.ReadLine();
                                if (data != null)
                                {
                                    lock (queueLock)
                                    {
                                        mainThreadActions.Enqueue(() =>
                                        {
                                            if (Time.time - lastDDATime > 15)
                                            {
                                                if (data == "up")
                                                {
                                                    if (NowLevel != 5)
                                                        DDA_up();
                                                }
                                                else if (data == "down")
                                                {
                                                    if (NowLevel != 1)
                                                        DDA_down();
                                                }
                                            }
                                        });
                                    }
                                    
                                } else {
                                    Debug.Log("No data");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Server error: {ex.Message}");
            }
            finally
            {
                Debug.Log("server stopped");
                StopServer();
            }
        });

        serverThread.IsBackground = true;
        serverThread.Start();
    }

    public void StopServer()
    {
        if (isRunning)
        {
            isRunning = false;
            server?.Stop();
            serverThread?.Join();  // 스레드 종료 대기
            Debug.Log("Server successfully stopped.");
        }
        else
        {
            Debug.Log("Server was already stopped.");
        }
    }

    void DDA_up() {
        Areas[NowLevel].SetActive(true);
        NowLevel += 1;
        lastDDATime = Time.time;
    }

    void DDA_down() {
        Areas[NowLevel - 1].SetActive(false);
        NowLevel -= 1;
        lastDDATime = Time.time;
    }
}
