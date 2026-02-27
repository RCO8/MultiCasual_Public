using UnityEngine;

public class MissileItem : ItemObjects
{
    //7가지의 미사일 (종류마다 방식이 다름)
    //플레이어가 먹으면 소유하다가 사용시 소멸

    // Color : Item | Type : Attack
    /*
     * 1. Red 미니미사일
     * 2. Grey 스피드 미사일
     * 3. Orange 유도탄 미사일(?)
     * 4. Blue 투명 미사일
     * 5. Pink 파워 미사일
     * 6. Green 지뢰 미사일
     * 7. Yellow 필살 미사일
     */

    [SerializeField] private byte MissileIndex;  //번호마다 미사일 타입이 다름
    [SerializeField] private GameObject MissileAttack;

    private void Start()
    {
        //미사일 발사체 생성
        MissileAttack = Resources.Load<GameObject>("Prefabs/Attacks/Missiles/Missile" + MissileIndex);
    }

    public int MissileType => MissileIndex;

    protected override void ObitainItem()
    {
        base.ObitainItem();
        targetPlayer.GetItem(this);
        //플레이어에게 미사일 공급
    }
}
