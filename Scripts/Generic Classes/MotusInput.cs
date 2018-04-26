using UnityEngine;
using Motus_Unity_Plugin;
using Motus_Unity_Plugin.VMUV_Hardware.Motus_1;
using Comms_Protocol_CSharp;

public static class MotusInput
{
    public const string Version = "1.0.0.1";
    public static RotationInput RotationSource = RotationInput.VMUV_TRACKER;
    public static UsageMode Usage = UsageMode.CAVE_VR_SYSTEM;

    static private Motus_1_MovementVector _vector = new Motus_1_MovementVector();
    static private Motus_1_Platform _platform = new Motus_1_Platform();
    static private Quaternion _steeringOffset = new Quaternion(0, 0, 0, 1);
    static private Quaternion _inGameOffset = new Quaternion(0, 0, 0, 1);
    static private Quaternion _trim = new Quaternion(0, 0, 0, 1);
    static private bool _isMoving = false;
    static private bool _movingStateChange = false;
    static private Quaternion _vmuvTrackerOffset = new Quaternion(0, 0, 0, 1);
    static private bool _skipNext = false;

    // Use this for initialization
    public static void Start()
    {
        Motus1.Initialize();
    }

    // Update is called once per frame
    public static void Update()
    {
        Motus1.Service();
        if (_skipNext)
        {
            _vector = new Motus_1_MovementVector();
            _platform = new Motus_1_Platform();
            _skipNext = false;
        }
        else
        {
            _vector = Motus1.GetMotionVector();
            _platform = Motus1.GetRawPlatformData();
        }
        RotationVector_Quat q = Motus1.GetQuat();
        ProcessVMUVTracker(new Quaternion(q.x, q.y, q.z, q.w));
    }

    // Use this to get the _inGameOffset field applied to the motus
    public static Quaternion GetInGameOffset()
    {
        return _inGameOffset;
    }

    // Use this to get a normalized translation vector in the x & z plane
    public static Vector3 GetNormalizedTranslation()
    {
        Motus_1_MovementVector local = _vector;
        local.Normalize();
        Vector3 rtn = new Vector3(local.LateralComponent, 0f, local.VerticalComponent);
        rtn = _inGameOffset * rtn;

        if (rtn.magnitude != 0)
        {
            if (!_isMoving)
            {
                _movingStateChange = true;
                _isMoving = true;
            }
        }
        else
        {
            if (_isMoving)
            {
                _movingStateChange = true;
                _isMoving = false;
            }
        }

        return rtn;
    }

    // Use this to get a raw data object with all of the Motus-1 data fields
    public static Motus_1_Platform GetRawDataObject()
    {
        return _platform;
    }

    // Use this to get the player rotation vector from your rotation tracker source
    public static Quaternion GetPlayerRotation(Quaternion rotationTracker)
    {
        Quaternion rtn = new Quaternion(0, 0, 0, 1);
        Vector3 rotationTrackerEuler = rotationTracker.eulerAngles;
        rotationTrackerEuler.x = 0;
        rotationTrackerEuler.z = 0;
        rotationTracker = Quaternion.Euler(rotationTrackerEuler);
        Quaternion steering = rotationTracker * _steeringOffset;
        rtn = steering;
        return rtn;
    }

    // Use this method to get the easy mode algorithm to trim your motion vector
    public static Quaternion GetTrim(Vector3 trans)
    {
        if (_movingStateChange)
        {
            _movingStateChange = false;

            if (_isMoving)
            {
                Quaternion motusRot = Quaternion.LookRotation(trans);
                Quaternion deltaQuat = motusRot * _steeringOffset;
                float delta = deltaQuat.eulerAngles.y;
                if (delta > 180f)
                    delta -= 360f;
                if (Mathf.Abs(delta) <= 65f)
                    _trim = Quaternion.Euler(0f, -delta, 0f);
                else
                    _trim = Quaternion.Euler(0f, 0f, 0f);
            }
            else
                _trim = Quaternion.Euler(0f, 0f, 0f);
        }

        return _trim;
    }

    // Use this to get the player rotation vector from your rotation tracker source
    public static Quaternion GetPlayerRotation(Vector3 rotationTracker)
    {
        return GetPlayerRotation(Quaternion.Euler(rotationTracker));
    }

    // Use this to track player rotation when they are not moving
    public static void SetNewSteeringOffset(Quaternion rotationTracker)
    {
        Vector3 rotationTrackerEuler = rotationTracker.eulerAngles;
        rotationTrackerEuler.x = 0;
        rotationTrackerEuler.z = 0;
        _steeringOffset = Quaternion.Euler(rotationTrackerEuler);
        _steeringOffset = Quaternion.Inverse(_steeringOffset);
    }

    // Use this to track player rotation when they are not moving
    public static void SetNewSteeringOffset(Vector3 rotationTracker)
    {
        SetNewSteeringOffset(Quaternion.Euler(rotationTracker));
    }

    // Use this to get the player's steering offset only -- do not use this
    // if you are using GetPlayerRotation() instead
    public static Quaternion GetSteeringOffset()
    {
        return _steeringOffset;
    }

    // Use this to calibrate the Motus to the game axes coordinate system
    public static void SnapMotusToGameAxes(Quaternion rotationTracker)
    {
        Vector3 rotationTrackerEuler = rotationTracker.eulerAngles;
        rotationTrackerEuler.x = 0;
        rotationTrackerEuler.z = 0;
        rotationTracker = Quaternion.Euler(rotationTrackerEuler);
        Vector3 vect = GetNormalizedTranslation();
        Quaternion latchPadOffset = Quaternion.LookRotation(vect);
        _inGameOffset = rotationTracker * Quaternion.Inverse(latchPadOffset);
    }

    // Use this to calibrate the Motus to the game axes coordinate system
    public static void SnapMotusToGameAxes(Vector3 rotationTracker)
    {
        SnapMotusToGameAxes(Quaternion.Euler(rotationTracker));
    }

    public static void SetVMUVTrackerOffset(Quaternion offset)
    {
        _vmuvTrackerOffset = offset;
        _skipNext = true;
    }

    private static void ProcessVMUVTracker(Quaternion tracker)
    {
        Vector3 vect = tracker.eulerAngles;
        vect.x = 0f;
        vect.y = -vect.z;
        vect.z = 0f;
        Quaternion rot = Quaternion.Euler(vect);
        Quaternion vmuv = rot * _vmuvTrackerOffset;
        if (RotationSource == RotationInput.VMUV_TRACKER)
            RotationTracker.UpdateRotation(vmuv);
    }
}

public enum RotationInput
{
    VIVE_TRACKER = 0,
    VMUV_TRACKER = 1
}

public enum UsageMode
{
    CAVE_VR_SYSTEM = 0,
    HMD = 1
}
