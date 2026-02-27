using System.Collections;
using UnityEngine;

public class PotionItem : ItemObjects
{
    protected string textUI;
    protected Color textColor;

    protected override void ObitainItem()
    {
        base.ObitainItem();
        UpgradeToPlayer(targetPlayer);
    }

    protected virtual void UpgradeToPlayer(PlayerBase player)
    {
        player.GetItem(this);
    }

    protected virtual void ShowPotionUI()
    {
        //위치 텍스트 등 반영후 일정 시간후 사라짐
    }

    private IEnumerator ShowPotionEffect()
    {
        yield return new WaitForSeconds(1f);
    }
}
