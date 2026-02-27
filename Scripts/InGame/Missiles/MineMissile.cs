using System.Collections;
using UnityEngine;

public class MineMissile : MissileAttack
{
    private Transform missileChild; //땅에 심어줄 모델

    private void Start()
    {
        shootPower = 60f;
    }

    public override void Shooting(Transform pos, int player = 0)
    {
        missileChild = transform.GetChild(0);
        base.Shooting(pos);
        transform.rotation = Quaternion.Euler(Vector3.zero);

        //땅으로 지뢰 심기
        StartCoroutine(GroundDown());
    }

    /// <summary>
    /// 땅으로 폭탄 심기
    /// </summary>
    /// <returns></returns>
    private IEnumerator GroundDown()
    {
        collider.enabled = false;
        while (missileChild.position.y > -1.1f)
        {
            missileChild.Translate(Vector3.down * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        collider.enabled = true;
    }
}
