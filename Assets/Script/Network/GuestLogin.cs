using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;  // 씬 전환을 위한 네임스페이스

public class GuestLogin : MonoBehaviourPunCallbacks
{
    public void GuestLoginbtnClick()
    {
        // 서버에 연결
        UIManager.userLogin = false;
        PhotonNetwork.ConnectUsingSettings();
    }
    
   

    // 로비에 성공적으로 입장했을 때 호출
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");

        // 방을 생성하거나 방이 있으면 참가
        PhotonNetwork.JoinOrCreateRoom("Room1", new RoomOptions { MaxPlayers = 10 }, TypedLobby.Default);
    }

    // 방에 성공적으로 입장했을 때 호출
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");

        // 게임 씬으로 전환 (PhotonNetwork.LoadLevel()을 사용)
        PhotonNetwork.LoadLevel("InGame");  
        }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected from server: {cause}");
    }
}
