using UnityEngine;

public static class RotationTracker
{
    private static Quaternion _quatRotation = new Quaternion(0, 0, 0, 1);
    private static Quaternion _hmd = new Quaternion(0, 0, 0, 1);

    public static void UpdateRotation(Quaternion rotation)
    {
        _quatRotation = rotation;
    }

    public static void UpdateRotation(Vector3 rotation)
    {
        _quatRotation = Quaternion.Euler(rotation);
    }

    public static void UpdateHMD(Quaternion rotation)
    {
        _hmd = rotation;
    }

    public static void UpdateHMD(Vector3 rotation)
    {
        _hmd = Quaternion.Euler(rotation);
    }

    public static Quaternion GetRotation()
    {
        return _quatRotation;
    }

    public static Quaternion GetHMD()
    {
        return _hmd;
    }
}
