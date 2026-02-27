using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateRoomPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField EnterRoomName;
    private string roomName;

    /// <summary>
    /// 방만들기
    /// </summary>
    public void CreateRoom()
    {
        if (EnterRoomName.text != null)
        {
            roomName = EnterRoomName.text;
            //Debug.Log("룸서버 생성");    //생성한 것을 서버에 보냄 (룸 노드 생성)

            if (PhotonNetwork.InLobby)
            {
                RoomOptions roomOptions = new RoomOptions
                {
                    MaxPlayers = 8,
                    IsVisible = true,
                    IsOpen = true,
                };

                //생성시 기본값 설정
                PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
                GameManager.Instance.CreateRoomSetting(roomName);
            }
        }
    }

    public void CancelCreate()
    {
        EnterRoomName.text = null;
        gameObject.SetActive(false);
    }
}
