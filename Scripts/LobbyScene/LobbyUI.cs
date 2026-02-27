using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class LobbyUI : MonoBehaviour
{
    #region SerializeFields
    [Header("위젯")]
    [SerializeField] private MyUserLabel MyUserLabel;
    [SerializeField] private CreateRoomPanel CreateRoomPanel;
    [SerializeField] private HowToPlayPanel HowToPlayPanel;

    [Header("룸 프리팹")]
    [SerializeField] private RectTransform RoomGroup;
    [SerializeField] private GameObject RoomPanel;

    [Header("유저 프리팹")]
    [SerializeField] private RectTransform UserGroup;
    [SerializeField] private GameObject UserPanel;
    #endregion

    private NetworkManager receiveFromServer;

    //룸 속성
    private readonly int MaxRoomIndex = 20;
    int roomCount = 0;
    List<RoomNode> roomNodes = new List<RoomNode>();

    #region MonoBehavior
    private void Awake()
    {
        receiveFromServer = NetworkManager.Instance;

        int i;  //룸 초기
        for(i = 0; i < MaxRoomIndex; i++)
        {
            GameObject obj = Instantiate(RoomPanel, RoomGroup);
            roomNodes.Add(obj.GetComponent<RoomNode>());
            obj.SetActive(false);
        }
    }
    private void Start()
    {
        MyUserLabel.UserName = GameManager.Instance.UserName;
        CreateRoomPanel.gameObject.SetActive(false);
        HowToPlayPanel.gameObject.SetActive(false);
    }
    private void Update()
    {
        UpdateRooms();
    }
    #endregion

    #region 서버에서 받을 정보를 반영

    /// <summary>
    /// 룸 변경 적용
    /// </summary>
    public void UpdateRooms()
    {
        int countOffset = receiveFromServer.LobbyInRooms.Count;

        //리스트 초기화
        for (int i = 0; i < MaxRoomIndex; i++)
            roomNodes[i].gameObject.SetActive(false);

        //박스 길이 설정
        RoomGroup.sizeDelta.Set(RoomGroup.localScale.x,
            RoomPanel.transform.localScale.y * countOffset);

        //리스트 재설정
        if (receiveFromServer.LobbyInRooms.Count > 0)
        {
            int idx = 0;
            foreach (RoomInfo room in receiveFromServer.LobbyInRooms)
            {
                roomNodes[idx].gameObject.SetActive(true);
                roomNodes[idx].ReceiveState(room, "Wating"); //여기에 게임중 표시 필요
                idx++;
            }
        }
    }
    #endregion
}
