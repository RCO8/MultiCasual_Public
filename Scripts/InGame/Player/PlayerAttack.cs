using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    #region 파이어볼 속성
    private float curAttackRange = 0.5f;
    private float curAttackPower = 1f;
    private float curAttackBoost = 3.5f;
    private string fireballTag;
    #endregion

    Queue<int> missileType = new Queue<int>(3);
    private string missileTag;

    private void Awake()
    {
        //태그이름 등록
        fireballTag = "fireball";
        missileTag = "missile";
    }

    #region 파이어볼 공격
    /// <summary>
    /// 공격 신호를 타 클라이언트에 전달
    /// </summary>
    public void Attack(int idx)
    {
        GameObject obj = ObjectPools.Instance.GetPrefab("fireball");
        FireBall fireball = obj.GetComponent<FireBall>();
        fireball.Shooting(transform, curAttackRange, curAttackPower, curAttackBoost, idx);
    }

    public void SetRange(float rng) => curAttackRange = rng;
    public void SetPower(float dmg) => curAttackPower = dmg;
    public void SetBoost(float bst) => curAttackBoost = bst;
    #endregion


    public void GetMissile(int idx)
    {
        if(missileType.Count > 3)
            missileType.Dequeue();
        missileType.Enqueue(idx);
    }
    public void UseMissile(int pl)
    {
        if (missileType.Count > 0)
        {
            missileTag += missileType.Dequeue().ToString();
            //Debug.Log(missileTag);
            GameObject obj = ObjectPools.Instance.GetPrefab(missileTag);
            MissileAttack missile = obj.GetComponent<MissileAttack>();
            missile.Shooting(transform, pl);
            missileTag = "missile";
        }
    }
}
