using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;  // �� ��ȯ�� ���� ���ӽ����̽�

public class GuestLogin : MonoBehaviourPunCallbacks
{
    public void GuestLoginbtnClick()
    {
        // ������ ����
        UIManager.userLogin = false;
        PhotonNetwork.ConnectUsingSettings();
    }
    
   

    // �κ� ���������� �������� �� ȣ��
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");

        // ���� �����ϰų� ���� ������ ����
        PhotonNetwork.JoinOrCreateRoom("Room1", new RoomOptions { MaxPlayers = 10 }, TypedLobby.Default);
    }

    // �濡 ���������� �������� �� ȣ��
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");

        // ���� ������ ��ȯ (PhotonNetwork.LoadLevel()�� ���)
        PhotonNetwork.LoadLevel("InGame");  
        }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected from server: {cause}");
    }
}
