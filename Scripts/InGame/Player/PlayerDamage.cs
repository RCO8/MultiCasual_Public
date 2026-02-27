using Photon.Pun;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    private PhotonView photonView;
    private PlayerHPBar playerUI;

    private readonly float setPlayerHP = 30f;
    private float curPlayerHP;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        curPlayerHP = setPlayerHP;

        if (photonView.IsMine)
        {
            playerUI = GameManager.Instance.YourHP;
            playerUI.SetPlayerHP(curPlayerHP, setPlayerHP);
        }
    }

    public bool IsDead
    {
        get => curPlayerHP.Equals(0f);
    }

    public void GetDamage(float dmg)
    {
        curPlayerHP = Mathf.Max(curPlayerHP - dmg, 0);

        //playerHP를 UI에 표시
        playerUI.SetPlayerHP(curPlayerHP);
    }
}