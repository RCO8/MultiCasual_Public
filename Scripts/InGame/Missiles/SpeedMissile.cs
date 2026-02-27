using UnityEngine;

public class SpeedMissile : MissileAttack
{
    private void Start()
    {
        shootSpeed = 120;
        shootPower = 10f;
    }

    public override void Shooting(Transform pos, int player = 0)
    {
        base.Shooting(pos);

        rgdby.velocity = transform.up;
        rgdby.AddForce(transform.up * shootSpeed, ForceMode.Acceleration);
    }
}
