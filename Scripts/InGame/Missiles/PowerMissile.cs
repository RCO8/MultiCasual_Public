using UnityEngine;

public class PowerMissile : MissileAttack
{
    private void Start()
    {
        shootSpeed = 50f;
        shootPower = 150f;
    }

    public override void Shooting(Transform pos, int player = 0)
    {
        base.Shooting(pos);

        rgdby.velocity = transform.up;
        rgdby.AddForce(transform.up * shootSpeed, ForceMode.Acceleration);
    }
}