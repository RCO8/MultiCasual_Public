using Photon.Pun;
using UnityEngine;

public class PlayerBase : MonoBehaviourPunCallbacks
{
    PlayerEvent playerEvent;
    PlayerMovement playerMovement;
    PlayerActions playerActions;
    PlayerStatus playerStatus;

    PhotonView photonView;

    [SerializeField] private int OriginIndexNumber; //캐릭터 고유 인덱스
    private int currentRank; //게임 중 현재 순위

    public int CurIndex { get; private set; }

    private void Awake()
    {
        playerEvent = GetComponent<PlayerEvent>();
        playerMovement = GetComponent<PlayerMovement>();
        playerActions = GetComponent<PlayerActions>();
        playerStatus = GetComponent<PlayerStatus>();

        photonView = GetComponent<PhotonView>();

        GetInfoFromRoom();
    }

    private void Start()
    {
        playerEvent.enabled = true; //시작시 초기
    }

    /// <summary>
    /// 방 모드에 따른 설정 등록
    /// </summary>
    private void GetInfoFromRoom()
    {
        //인덱스 발급
        CurIndex = OriginIndexNumber;
        if (photonView.IsMine)  //내 플레이어 인덱스라면
        {
            playerEvent.OnMoveEvent += playerMovement.GetMoveInput;
            playerEvent.OnActionEvent += playerActions.GetActionInput;
        }

        //게임모드 임포트
    }

    #region 아이템 획득
    public void GetItem(ItemObjects item)
    {
        if (item is PotionItem)
            GetPotion((PotionItem)item);
        else if (item is MissileItem)
            GetMissile((MissileItem)item);
    }
    private void GetPotion(PotionItem item)
    {
        switch(item)
        {
            case SpeedUpPotion:
                playerStatus.Upgrade(PlayerStatus.SAttribute.PSpeed);
                playerMovement.SetSpeed(playerStatus.MySpeed);
                break;
            case RangeUpPotion:
                playerStatus.Upgrade(PlayerStatus.SAttribute.PRange);
                playerActions.attack.SetRange(playerStatus.MyRange);
                break;
            case PowerUpPotion:
                playerStatus.Upgrade(PlayerStatus.SAttribute.PPower);
                playerActions.attack.SetPower(playerStatus.MyDamage);
                break;
            case BoostUpPotion:
                playerStatus.Upgrade(PlayerStatus.SAttribute.PBoost);
                playerActions.attack.SetBoost(playerStatus.MyBoost);
                break;
            case HiddenItem:
                playerActions.Invisible(5f);
                break;
        }
    }
    private void GetMissile(MissileItem item)
    {
        //Debug.Log("미사일 획득");
        playerActions.GetMissile(item.MissileType);
    }
    #endregion

    public void GetDamage(float dmg)
    {
        playerActions.damage.GetDamage(dmg);

        if (playerActions.damage.IsDead)
        {
            photonView.RPC("GetDead", RpcTarget.All);
            GetRank();
        }
    }

    [PunRPC]
    private void GetDead()
    {
        //Debug.Log("Game Over");

        gameObject.SetActive(false);
        playerEvent.enabled = false;
    }

    private void GetRank()
    {
        GameManager justManager = GameManager.Instance; //임시로 싱글톤 가져오기

        if(justManager.CurMembers.Equals(1))
            currentRank = justManager.CurMembers;
        else
            currentRank = justManager.GetRank();
    }
}
