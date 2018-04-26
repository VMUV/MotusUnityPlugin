using UnityEngine;

public class AutoOrienter
{
    private bool _isOriented = false;

    public void Orient(Quaternion rotation)
    {
        Vector3 vect = MotusInput.GetNormalizedTranslation();
        if (vect.magnitude != 0)
        {
            MotusInput.SnapMotusToGameAxes(rotation);
            _isOriented = true;
        }
    }

    public void Orient(Vector3 rotation)
    {
        Orient(Quaternion.Euler(rotation));
    }

    public void Orient(Quaternion rotation, Quaternion HMD)
    {
        Vector3 vect = MotusInput.GetNormalizedTranslation();
        if (vect.magnitude != 0)
        {
            Quaternion offset = HMD * Quaternion.Inverse(rotation);
            MotusInput.SetVMUVTrackerOffset(offset);
            MotusInput.SnapMotusToGameAxes(HMD);
            _isOriented = true;
        }
    }

    public void Orient(Vector3 rotation, Quaternion HMD)
    {
        Orient(Quaternion.Euler(rotation), HMD);
    }

    public bool IsOriented()
    {
        return _isOriented;
    }
}
