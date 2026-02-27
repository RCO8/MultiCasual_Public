using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomNode : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI RoomName;
    [SerializeField] private TextMeshProUGUI RoomMode;
    [SerializeField] private Button CanEnterRoom;

    private int curPlayers;
    private int maxPlayers;
    private string roomName;
    

    /// <summary>
    /// 서버에서 받을 룸
    /// </summary>
    public void ReceiveState(RoomInfo info, string mode)
    {
        curPlayers = info.PlayerCount;
        maxPlayers = info.MaxPlayers;
        roomName = info.Name;
        CanEnterRoom.interactable = curPlayers < maxPlayers;

        //Update UI
        RoomName.text = roomName;
        RoomMode.text = $"{mode} ({curPlayers}/{maxPlayers})";
    }

    public void EnterGameButton()
    {
        //방 참여
        PhotonNetwork.JoinRoom(roomName);
    }
}
