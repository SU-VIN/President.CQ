using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineManager : MonoBehaviourPunCallbacks
{
    public Text onlineMonitoringText;
    private string gameVersion = "1";
    public DatabaseManager db;

    public Button RandomBtn;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;
        db = GameObject.Find("UserInfo").GetComponent<DatabaseManager>();

        if (!PhotonNetwork.IsConnected)
        {
            onlineMonitoringText.text = "서버에 접속중..";
            PhotonNetwork.ConnectUsingSettings();

        }
        else {
            onlineMonitoringText.text = "연결됨 : 환영합니다! " + db.getNickName() + "님!";

        }

        RandomBtn.onClick.AddListener(()=> {
            Connect();
        });

    }
   

    public override void OnConnectedToMaster()
    {
        onlineMonitoringText.text = "연결됨 : 환영합니다! " + db.getNickName() + "님!";
        RandomBtn.interactable = true;
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {


        onlineMonitoringText.text = "연결 유실 : 연결정보를 잃었습니다.\n 재접속중...";
        RandomBtn.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
    }


    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        //PhotonNetwork.NickName = "Player " + UnityEngine.Random.Range(0, 1000).ToString("0000");
        PhotonNetwork.NickName = db.getNickName();
    }

    public void Connect()
    {
        Debug.Log("Try Connect Room");
        if (PhotonNetwork.IsConnected)
        {
            onlineMonitoringText.text = "연결됨 : 랜덤 룸에 접속중...";
            PhotonNetwork.JoinRandomRoom();

        }
        else
        {
            onlineMonitoringText.text = "연결 유실 : 연결정보를 잃었습니다.\n 재접속중...";
            RandomBtn.interactable = false;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {

        onlineMonitoringText.text = "연결됨 : 생성된 룸이 없음. 룸을 생성 중...";
        Debug.Log("Creating Room");
        PhotonNetwork.CreateRoom(PhotonNetwork.NickName+"의 룸", new RoomOptions { MaxPlayers = 0 });

    }

    public override void OnJoinedRoom()
    {
        onlineMonitoringText.text = "연결됨 : 룸에 접속 중...";
        Debug.Log("Join Room");
        PhotonNetwork.LoadLevel("Room_Scene");

    }
}
