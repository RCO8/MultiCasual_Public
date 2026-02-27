using UnityEngine;

public class PlayerSpawn
{
    private const float grid = 5.7f;
    public static Vector3 Position(int idx)
    {
        int x = 0, y = 0;

        if (idx == 1 || idx == 4 || idx == 6) x = -1;
        if (idx == 3 || idx == 5 || idx == 8) x = 1;

        if (idx <= 3) y = 1;
        if (idx >= 6) y = -1;

        return new Vector3(x * grid, 0, y * grid);
    }
}
