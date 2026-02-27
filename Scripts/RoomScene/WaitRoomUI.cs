using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class WaitRoomUI : MonoBehaviour
{
    #region UI 속성
    [SerializeField] private Toggle ReadyButton;
    [SerializeField] private Button StartButton;
    [SerializeField] private TextMeshProUGUI RoomName;
    [SerializeField] private RectTransform NotReadyPanel;
    #endregion

    [SerializeField] private List<PlayerViews> UserViews = new List<PlayerViews>(8);
    [SerializeField] private RectTransform UserImageGroup;
    private List<RawImage> UserImages = new List<RawImage>(8);

    private NetworkManager receiveFromServer;   //NetworkManager에서 받아올 속성
    Hashtable sendForReady;

    //리더 속성
    private bool isLeader = false;
    private bool allReady = false;
    //인원 속성
    private bool imReady = false;

    private int curIndex = 0;
    private bool activeInRoom;  //오류방지용

    private int curMap = 0; //맵 생성 설정

    #region MonoBehavior
    private void Awake()
    {
        receiveFromServer = NetworkManager.Instance;

        sendForReady = new Hashtable
        {
            {"Ready", imReady },
            {"Start", false },
            {"MapIndex", 0 }
        };

        //Raw Image등록
        for (int i = 0; i < UserImageGroup.childCount; i++)
            UserImages.Add(UserImageGroup.GetChild(i).GetComponent<RawImage>());
    }
    private void Start()
    {
        //유저 패널 비활성
        for (int i = 1; i < UserViews.Count; i++)
        {
            UserImages[i].gameObject.SetActive(false);
            UserViews[i].gameObject.SetActive(false);
        }

        curIndex = GameManager.Instance.MyIndex - 1;

        UserImages[curIndex].gameObject.SetActive(true);
        UserViews[curIndex].gameObject.SetActive(true);
        UserViews[curIndex].UpdateUserName(GameManager.Instance.UserName);

        //방장이 룸생성
        isLeader = curIndex.Equals(0);
        StartButton.gameObject.SetActive(isLeader);
        ReadyButton.gameObject.SetActive(!isLeader);

        NotReadyPanel.gameObject.SetActive(false);  //에러 일단 숨김

        RoomName.text = GameManager.Instance.YourRoomName;

        activeInRoom = true;
    }
    private void Update()
    {
        if (activeInRoom)
        {
            UpdateUsers();
            GetReadyUsers();
        }
    }
    #endregion

    /// <summary>
    /// 실시간 유저가 들어오고 나오는지
    /// </summary>
    private void UpdateUsers()
    {
        //데이터가 없으면 비활성
        for(int i=0;i<UserViews.Count;i++)
        {
            if (receiveFromServer.PlayersInRoom.Count <= i)
            {
                //Debug.Log($"{i + 1} 비활성 | 현재 인원 : {receiveFromServer.PlayersInRoom.Count}");
                UserImages[i].gameObject.SetActive(false);
                UserViews[i].gameObject.SetActive(false);
            }
        }

        foreach (var pl in receiveFromServer.PlayersInRoom)
        {
            //방장인지 확인
            int idx = pl.ActorNumber - 1;
            bool leader = idx.Equals(0);

            //Debug.Log($"idx : {idx}");
            UserImages[idx].gameObject.SetActive(true);
            UserViews[idx].gameObject.SetActive(true);
            UserViews[idx].UpdateUserName(pl.NickName);

            //준비 상태 업데이트
            if (!leader && pl.CustomProperties.ContainsKey("Ready"))
                UserViews[idx].SetReady((bool)pl.CustomProperties["Ready"]);
        }
    }

    public void StartGame()
    {
        //게임씬으로 이동
        if (allReady) //다른 클라이언트에게 시작하겠다는 신호 전송
        {
            //랜덤한 맵 생성(방장이 먼저 생성후 다른 클라이언트에게 설정된 값 전달)
            if (isLeader)
                sendForReady["MapIndex"] = GameManager.Instance.InitMapIdx();
            sendForReady["Start"] = allReady;
            PhotonNetwork.LocalPlayer.SetCustomProperties(sendForReady);
        }
        else
            NotReadyPanel.gameObject.SetActive(true);
    }

    #region 준비 기능 요소들
    /// <summary>
    /// 방장 이외 인원이 준비 신호 전송
    /// </summary>
    public void ReadyToGame()
    {
        imReady = ReadyButton.isOn;

        //UserView UI 반영
        UserViews[curIndex].SetReady(imReady);

        //내가 준비한 신호를 타 클라이언트가 확인하도록
        sendForReady["Ready"] = imReady;
        PhotonNetwork.LocalPlayer.SetCustomProperties(sendForReady);
    }

    /// <summary>
    /// 모든 플레이어가 준비 됐는지 확인
    /// </summary>
    private void GetReadyUsers()
    {
        for (int i = 1; i < receiveFromServer.CurRoom.PlayerCount; i++)
        {
            if (UserViews[i].ImReady)
                allReady = true;
            else
            {
                allReady = false;
                break;
            }
        }
    }
    #endregion

    public void ExitRoom()
    {
        activeInRoom = false;
        //서버에서 룸 제거
        if (receiveFromServer.CurRoom.PlayerCount == 1)
            receiveFromServer.LobbyInRooms.Remove(receiveFromServer.CurRoom);
        //룸 서버 나가고 로비로 재접속
        PhotonNetwork.LeaveRoom();
    }
}
