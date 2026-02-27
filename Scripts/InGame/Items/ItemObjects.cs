using UnityEngine;

public class ItemObjects : MonoBehaviour
{
    protected PlayerBase targetPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            targetPlayer = other.GetComponent<PlayerBase>();
            ObitainItem();
        }
    }

    /// <summary>
    /// 아이템 획득
    /// </summary>
    protected virtual void ObitainItem()
    {
        gameObject.SetActive(false);    //오브젝트 풀 활용
    }
}