using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region 싱글톤
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
        set
        {
            if (instance == null)
            {
                instance = value;
                DontDestroyOnLoad(instance);
            }
        }
    }
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region 유저 속성
    private string userName;
    public string UserName
    {
        get => userName;
        set
        {
            if (userName == null)
                userName = value;
        }
    }

    private int myIndex;
    public int MyIndex { get => myIndex; set => myIndex = value; }
    #endregion

    #region 룸 속성
    private string yourRoomName;
    public string YourRoomName
    {
        get => yourRoomName;
        private set
        {
            yourRoomName = value;
        }
    }
    #endregion

    #region 메인 게임 속성
    private int gameRank = 0;
    private int myRank = 0;
    private int curMapIdx;
    private PlayerHPBar yourHP;
    private ScreenPanel gameSetPanel;

    private ExitGames.Client.Photon.Hashtable sendToServer = new ExitGames.Client.Photon.Hashtable();

    public int CurMembers
    {
        get => gameRank;
    }

    public int CurMapIdx 
    {
        get { return curMapIdx; }
    }
    public PlayerHPBar YourHP
    {
        get => yourHP;
        set
        {
            if(yourHP == null)
                yourHP = value;
        }
    }
    public ScreenPanel GameSetPanel
    {
        get => gameSetPanel;
        set
        {
            if (gameSetPanel == null)
                gameSetPanel = value;
        }
    }
    public MapStage CurStage { get; private set; }

    #endregion

    public int InitMapIdx()
    {
        curMapIdx = Random.Range(1, 4);
        return CurMapIdx;
    }

    /// <summary>
    /// 메인 게임으로
    /// </summary>
    public void GameStart(int map)
    {
        SceneManager.LoadScene("MainGame");
        StartCoroutine(InitGame(map));
    }

    /// <summary>
    /// 잠시 대기하다 플레이어 및 스테이지 설정
    /// </summary>
    /// <param name="map"></param>
    private IEnumerator InitGame(int map)
    {
        yield return new WaitForSeconds(1f);
        //GameSetPanel.GameReady();

        //인원 설정
        gameRank = PhotonNetwork.CurrentRoom.PlayerCount;
        sendToServer["RankRequest"] = gameRank;
        //Debug.Log($"인원 : {gameRank}");

        //결과 UI
        GameSetPanel.GameStart();

        //플레이어 생성
        PhotonNetwork.Instantiate($"Prefabs/Players/Player{myIndex}", PlayerSpawn.Position(myIndex), Quaternion.identity, 0);

        //맵 스테이지 생성
        GameObject obj = Resources.Load<GameObject>($"Prefabs/Maps/Map{map}");
        CurStage = Instantiate(obj).GetComponent<MapStage>();

        //Debug.Log(CurStage.gameObject.name);
        CurStage.SendDataToServer();
    }

    /// <summary>
    /// 랭킹 설정
    /// </summary>
    public int GetRank()
    {
        //생존한 인원만 다운로드
        myRank = gameRank;
        gameRank--;

        //현재 인원을 서버에 공유
        sendToServer["RankRequest"] = gameRank;
        PhotonNetwork.SetPlayerCustomProperties(sendToServer);

        Debug.Log($"Rank : {myRank}, Players : {gameRank}");
        return myRank;
    }
    /// <summary>
    /// 몇명 남았는지
    /// </summary>
    /// <param name="n"></param>
    public void SetRemain(int n)
    {
        gameRank = n;
        if(gameRank.Equals(1))
        {
            ResultGame(myRank);
        }
    }
    private void ResultGame(int rank)
    {
        string resultText;
        Color resultColor;

        if (rank <= 1)//리팩토링
        {
            resultText = "Winner!!!";
            resultColor = Color.yellow;
        }
        else
        {
            resultText = $"You are {rank} Rank.";
            resultColor = Color.cyan;
        }

        GameSetPanel.GameSet();
        GameSetPanel.Result.ApplyRankText(resultText, resultColor);

        // 3초후 ReadyScene으로 돌아감
        StartCoroutine(BackToRoom());
    }
    private IEnumerator BackToRoom()
    {
        float time = 3f;
        while (time > 0)
        {
            yield return null;
            time -= Time.deltaTime;
            GameSetPanel.Result.ApplyWaitText((int)time);
        }
        
        //Debug.Log("Wait Scene으로");
        SceneManager.LoadScene("WaitRoom");
    }

    #region 룸 메서드
    public void CreateRoomSetting(string name)
    {
        YourRoomName = name;
        MyIndex = 1;
    }
    public void JoinRoomSetting(string name)
    {
        YourRoomName = name;
        MyIndex = PhotonNetwork.CurrentRoom.PlayerCount;

        //foreach(Photon.Realtime.Player pl in PhotonNetwork.CurrentRoom.Players.Values)
        //    Debug.Log($"{pl.NickName}, {pl.UserId}. {pl.ActorNumber}");
    }
    public void LeaveRoomSetting()
    {
        YourRoomName = null;
        MyIndex = 0;
    }
    #endregion
}