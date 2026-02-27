using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CurHP, MaxHP;
    [SerializeField] private Slider HPBar;

    private void Awake()
    {
        GameManager.Instance.YourHP = this;
        //Debug.Log(GameManager.Instance.YourHP == null);
    }

    /// <summary>
    /// 시작할 때 현재 체력 세팅
    /// </summary>
    /// <param name="cur"></param>
    /// <param name="max"></param>
    public void SetPlayerHP(float cur, float max)
    {
        HPBar.value = cur;
        CurHP.text = cur.ToString();
        MaxHP.text = max.ToString();
    }
    public void SetPlayerHP(float cur)
    {
        HPBar.value = cur;
        CurHP.text = cur.ToString();
    }
}
