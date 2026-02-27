using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using System.Linq;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Singleton
    private static NetworkManager instance;
    public static NetworkManager Instance 
    { 
        get => instance;
        private set
        {
            instance = value;
            DontDestroyOnLoad(instance);
        }
    }
    #endregion

    //로비내 룸이 있는지
    public List<RoomInfo> LobbyInRooms { get; private set; } = new List<RoomInfo>();

    #region 룸 속성
    //룸 내 데이터
    public Room CurRoom { get; private set; }
    public List<bool> ReadyInRoom { get; private set; } = new List<bool>(8);
    public List<Photon.Realtime.Player> PlayersInRoom { get; private set; }
    #endregion

    private void Awake()
    {
        //싱글톤 등록
        if (instance == null)
            Instance = this;
    }

    #region 서버 로비 접속
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.NickName = GameManager.Instance.UserName;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        //PhotonNetwork.LoadLevel("LobbyScene");
        SceneManager.LoadScene("LobbyScene");

        //유저 업데이트
        //Debug.Log(PhotonNetwork.CountOfPlayers);
        //Debug.Log(PhotonNetwork.CountOfPlayersOnMaster);

        //같은 이름은 생략(유저 패널)
        PhotonNetwork.RaiseEvent(100, PhotonNetwork.NickName, new RaiseEventOptions
        {
            Receivers = ReceiverGroup.Others
        }, SendOptions.SendReliable);
    }
    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        Debug.Log("게임 퇴장");
        SceneManager.LoadScene("TitleScene");
    }

    /// <summary>
    /// Photon서버 접속
    /// </summary>
    public void JoinToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    #endregion

    #region 로비내 기능들
    /// <summary>
    /// 룸 업데이트
    /// </summary>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        //룸 업데이트
        LobbyInRooms.Clear();
        foreach (RoomInfo room in roomList)
        {
            if (room.PlayerCount > 0)
                LobbyInRooms.Add(room);
        }
    }

    #region 방 생성 기능
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        //Debug.Log("방 생성");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        //룸 속성 등록
        CurRoom = PhotonNetwork.CurrentRoom;
        GameManager.Instance.JoinRoomSetting(PhotonNetwork.CurrentRoom.Name);
        SceneManager.LoadScene("WaitRoom");

        //업뎃 리스트에 등록하고 되는지 출력
        PlayersInRoom = PhotonNetwork.CurrentRoom.Players.Values.ToList();
        //foreach (var p in PlayersInRoom)
        //    Debug.Log($"{p.ActorNumber} : {p.NickName} ");

        //준비 상태 초기화
        ReadyInRoom.Clear();
        for (int i = 0; i < 8; i++) ReadyInRoom.Add(false);
        ReadyInRoom[0] = true;

        int idx = GameManager.Instance.MyIndex;
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }
    #endregion
    #endregion

    #region 룸내 기능들
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        //Debug.Log($"{newPlayer.NickName}님 참가");
        PlayersInRoom.Add(newPlayer);
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        #region WaitRoom Data
        //준비 신호 수신
        if (changedProps.ContainsKey("Ready"))
        {
            //Debug.Log($"{targetPlayer.ActorNumber} is ready : {changedProps["Ready"]}");
            ReadyInRoom[targetPlayer.ActorNumber - 1] = (bool)changedProps["Ready"];
        }
        //시작 신호 수신 (모든 유저가 준비가 됐으면)
        if (changedProps.ContainsKey("Start"))
        {
            if ((bool)changedProps["Start"])
            {
                int map = (int)changedProps["MapIndex"];
                GameManager.Instance.GameStart(map);
            }
        }
        #endregion


        if(changedProps.ContainsKey("HasItem"))
        {
            Debug.Log("박스 아이템 생성");
            ItemPackage package = new ItemPackage();

            int box = (int)changedProps["BoxIndex"];
            float type = (float)changedProps["ItemType"];
            float random = (float)changedProps["RandomItem"];
            int index = (int)changedProps["ItemIndex"];

            if ((bool)changedProps["HasItem"])
            {
                //아이템 설정
                package.SetValues(box, type, random, index);
                GameManager.Instance.CurStage.InsertItem(package);
            }
        }

        #region Ranking
        if (changedProps.ContainsKey("RankRequest"))
        {
            int member = (int)changedProps["RankRequest"];
            Debug.Log(member);
            GameManager.Instance.SetRemain(member);
        }
        #endregion
    }

    //퇴장
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        //otherPlayer를 PlayersInRoom에서 제거
        PlayersInRoom.Remove(otherPlayer);
        //Debug.Log($"{otherPlayer.NickName}님 퇴장 : {CurRoom.Players.Count}, {PlayersInRoom.Count}");

        //foreach (var pl in PlayersInRoom) Debug.Log($"{pl.ActorNumber} : {pl.NickName}");
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        //Debug.Log("룸 퇴장");
        CurRoom = null;
        GameManager.Instance.LeaveRoomSetting();

        PhotonNetwork.JoinLobby();
        SceneManager.LoadScene("LobbyScene");
    }
    #endregion
}
