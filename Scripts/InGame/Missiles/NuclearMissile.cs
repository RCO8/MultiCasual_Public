using UnityEngine;

public class NuclearMissile : MissileAttack
{
    private void Start()
    {
        shootPower = 150f;
        shootSpeed = 70f;
    }

    public override void Shooting(Transform pos, int player = 0)
    {
        transform.position = pos.localPosition;
        transform.rotation = pos.localRotation;

        transform.Rotate(180f, 0f, 0f);
        transform.Translate(Vector3.down * 5);

        collider.enabled = true;
    }
}