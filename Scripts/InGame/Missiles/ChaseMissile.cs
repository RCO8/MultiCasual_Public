using UnityEngine;

public class ChaseMissile : MissileAttack
{
    private void Start()
    {
        shootSpeed = 100f;
        shootPower = 35f;
    }

    public override void Shooting(Transform pos, int player = 0)
    {
        base.Shooting(pos);
        //근처에 있는 상대방 선택
        //if(Vector3.distance(trasnform.position, targetplayer) == min)

        rgdby.velocity = transform.up;
        rgdby.AddForce(transform.up * shootSpeed, ForceMode.Acceleration);
    }
}
