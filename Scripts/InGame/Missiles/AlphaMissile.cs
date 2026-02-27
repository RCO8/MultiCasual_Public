using System.Collections;
using UnityEngine;

public class AlphaMissile : MissileAttack
{
    private Material alphaEffect;
    private Transform modelInChild;

    private void Start()
    {
        shootSpeed = 70f;
        shootPower = 15f;
    }

    public override void Shooting(Transform pos, int player = 0)
    {
        modelInChild = transform.GetChild(0);
        alphaEffect = modelInChild.GetComponent<MeshRenderer>().materials[0];

        base.Shooting(pos);

        rgdby.velocity = transform.up;
        rgdby.AddForce(transform.up * shootSpeed, ForceMode.Acceleration);

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float alpha = 1f;
        float speed = 3f;

        // 셰이더 설정
        Color color = alphaEffect.color;

        while (alpha > 0f)
        {
            alpha -= speed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);
            color.a = alpha;
            alphaEffect.color = color;
            yield return new WaitForSeconds(Time.deltaTime / speed);
        }
    }
}