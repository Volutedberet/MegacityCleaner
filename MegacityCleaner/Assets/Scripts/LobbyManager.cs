using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class LobbyManager : MonoBehaviour{
    [SerializeField] Button host;
    [SerializeField] Button client;

    private void Start() {
        host.onClick.AddListener(HostGame);
        client.onClick.AddListener(JoinGame);
    }

    public void HostGame(){
        NetworkManager.Singleton.StartHost();
        Destroy(this.gameObject);
    }

    public void JoinGame(){
        NetworkManager.Singleton.StartClient();
        Destroy(this.gameObject);
    }
}