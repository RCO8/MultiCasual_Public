using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public enum SAttribute { PSpeed, PRange, PPower, PBoost }

    #region Const Fields
    //Speed Up
    private const float baseSpeed = 1f;
    private const float upgradeSpeed = 2f;

    //RangeUp
    private const float baseRange = 0.5f;
    private const float upgradeRange = 1.2f;

    //PowerUp
    private const float baseDamage = 3f;
    private const float upgradeDamage = 5f;

    //BoostUp
    private const float baseBoost = 3.5f;
    private const float upgradeBoost = 6f;
    #endregion

    #region Current Fields
    private float curSpeed;
    private float curRange;
    private float curDamage;
    private float curBoost;

    public float MySpeed
    {
        get => curSpeed;
        private set => curSpeed = value;
    }
    public float MyRange
    {
        get => curRange;
        private set => curRange = value;
    }
    public float MyDamage
    {
        get => curDamage;
        private set => curDamage = value;
    }
    public float MyBoost
    {
        get => curBoost;
        private set => curBoost = value;
    }
    #endregion

    /// <summary>
    /// 속성 업그레이드
    /// </summary>
    /// <param name="attr">Enum SAttribute</param>
    public void Upgrade(SAttribute attr)
    {
        switch(attr)
        {
            case SAttribute.PSpeed:
                MySpeed = upgradeSpeed;
                break;
            case SAttribute.PRange:
                MyRange = upgradeRange;
                break;
            case SAttribute.PPower:
                MyDamage = upgradeDamage;
                break;
            case SAttribute.PBoost:
                MyBoost = upgradeBoost;
                break;
        }
    }

    public void Downgrade(SAttribute attr)
    {
        switch(attr)
        {
            case SAttribute.PSpeed:
                MySpeed = baseSpeed;
                break;
            case SAttribute.PRange:
                MyRange = baseRange;
                break;
            case SAttribute.PPower:
                MyDamage = baseDamage;
                break;
            case SAttribute.PBoost:
                MyBoost = baseBoost;
                break;
        }
    }
}
