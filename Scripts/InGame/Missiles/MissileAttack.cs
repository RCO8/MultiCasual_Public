using System.Collections;
using UnityEngine;

public class MissileAttack : MonoBehaviour
{
    protected Rigidbody rgdby;
    protected Collider collider;
    protected float shootSpeed;
    protected float shootPower;

    private int owner = 0;  //발사한 대상(1P가 쐈으면 1로 하고 충돌 무효)

    protected void Awake()
    {
        rgdby = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        int targetLayer = other.gameObject.layer;
        int playerTarget = LayerMask.NameToLayer("Player");
        int boxTarget = LayerMask.NameToLayer("Box");

        if (playerTarget.Equals(targetLayer))
        {
            GameObject target = other.gameObject;
            int check = target.GetComponent<PlayerBase>().CurIndex;

            if (owner != check) HitExplose();
        }

        if (boxTarget.Equals(targetLayer))
        {
            BoxObject box = other.gameObject.GetComponent<BoxObject>();
            box.GetDamage(shootPower);
            HitExplose();
        }

        if(this as NuclearMissile)  //7번 미사일
            HitExplose();
    }

    private void HitExplose()
    {
        Debug.Log("폭발");
        gameObject.SetActive(false);
    }

    public virtual void Shooting(Transform pos, int player = 0)
    {
        transform.position = pos.position;
        transform.rotation = pos.rotation;

        transform.Translate(Vector3.up * 0.9f);
        transform.Rotate(Vector3.right * 90);

        owner = player;

        //단시간 후 collider 활성
        StartCoroutine(ColliderEnableWait());
    }

    private IEnumerator ColliderEnableWait()
    {
        yield return new WaitForSeconds(1f);
        collider.enabled = true;
    }
}
