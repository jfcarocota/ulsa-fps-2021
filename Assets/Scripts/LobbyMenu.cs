using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField]
    Button btnStartServer;
    [SerializeField]
    Button btnStartHost;
    [SerializeField]
    Button btnStartClient;

    void Awake()
    {
        btnStartServer.onClick.AddListener(()=>{
            NetworkManager.Singleton.StartServer();
        });

        btnStartHost.onClick.AddListener(()=>{
            NetworkManager.Singleton.StartHost();
            gameObject.SetActive(false);
        });

        btnStartClient.onClick.AddListener(()=>{
            NetworkManager.Singleton.StartClient();
            gameObject.SetActive(false);
        });
    }
}
