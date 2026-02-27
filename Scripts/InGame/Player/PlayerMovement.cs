using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 movePosition;
    private Rigidbody rgdby;
    private float curSpeed = 1f;

    private void Awake()
    {
        rgdby = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyRotation();
    }

    public void GetMoveInput(Vector2 dir)
    {
        movePosition.x = dir.x;
        movePosition.z = dir.y;
    }

    #region 움직임 적용
    private void ApplyMovement()
    {
        rgdby.velocity = movePosition.normalized * curSpeed;
    }
    private void ApplyRotation()
    {
        if (movePosition != Vector3.zero)
        {
            float atanZ = Mathf.Atan2(movePosition.x, movePosition.z) * Mathf.Rad2Deg;
            Quaternion newRotation = Quaternion.Euler(0, atanZ, 0);
            rgdby.MoveRotation(newRotation);
        }
    }
    #endregion

    public void SetSpeed(float spd) => curSpeed = spd;
}
