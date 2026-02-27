using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviourPunCallbacks
{
    public PlayerAttack attack { get; private set; }
    public PlayerDamage damage { get; private set; }
    public PlayerInvisible invisible { get; private set; }
    
    private PhotonView photonView;
    private int getIndex;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        getIndex = GetComponent<PlayerBase>().CurIndex;

        //컴포넌트 추가
        attack = gameObject.AddComponent<PlayerAttack>();
        invisible = gameObject.AddComponent<PlayerInvisible>();
        if (photonView.IsMine)
            damage = gameObject.AddComponent<PlayerDamage>();
    }

    /// <summary>
    /// 입력에 따른 액션
    /// </summary>
    /// <param name="index"></param>
    public void GetActionInput(int index)
    {
        switch(index)
        {
            case 1:
                photonView.RPC("SendAttack", RpcTarget.All);
                break;
            case 2:
                photonView.RPC("UseItem", RpcTarget.All);
                break;
            case 3:
                //UnknownAction();
                break;
        }
    }

    #region 입력값 액션
    /// <summary>
    /// 공격 신호 전달
    /// </summary>
    [PunRPC]
    private void SendAttack()
    {
        attack.Attack(getIndex);
    }

    /// <summary>
    /// 아이템 사용 전달
    /// </summary>
    [PunRPC]
    private void UseItem()
    {
        //Debug.Log("미사일 사용");
        attack.UseMissile(getIndex);
    }

    private void UnknownAction()
    {
        //추가할 예정
        Debug.Log("추가 액션");
    }
    #endregion

    #region 플레이어 투명화
    public void Invisible(float time)
    {
        photonView.RPC("SendInvisible", RpcTarget.Others, time);
    }

    [PunRPC]
    private void SendInvisible(float time)
    {
        invisible.StartInvisible(time);
    }
    #endregion

    /// <summary>
    /// 미사일 획득
    /// </summary>
    /// <param name="idx">미사일 타입</param>
    public void GetMissile(int idx)
    {
        attack.GetMissile(idx);
    }
}
