using System.Collections;
using UnityEngine;

public class HiddenItem : PotionItem
{
    private float coolTime = 5f;

    protected override void UpgradeToPlayer(PlayerBase player)
    {
        base.UpgradeToPlayer(player);
        //플레이어 숨김효과 추가
    }
}