//서버에 보내고 받을 데이터 묶음 (같은 클라이언트 끼리 동기화)
using UnityEngine;

public class ItemPackage
{
    int boxIndex;
    float itemType;
    float randomItem;
    int itemIndex;

    public void SetValues(int b, float t, float r, int i)
    {
        boxIndex = b;
        itemType = t;
        randomItem = r;
        itemIndex = i;
    }

    #region 설정 한 값 접근
    public int BoxIndex => boxIndex;
    public float ItemType => itemType;
    public float RandomItem => randomItem;
    public int ItemIndex => itemIndex;
    #endregion

    /// <summary>
    /// 적용한 값들을 출력 (확인용)
    /// </summary>
    public void ShowValues()
    {
        Debug.Log($"Box : {boxIndex} | Item Type : {itemType} | Random Item : {randomItem} | Item Index : {itemIndex}");
    }
}
