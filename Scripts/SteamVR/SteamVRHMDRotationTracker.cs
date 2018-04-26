using UnityEngine;

public class SteamVRHMDRotationTracker : MonoBehaviour {

    public SteamVR_TrackedObject _steamTrackedObject;
    public Transform _transform;

    private void OnEnable()
    {
        _steamTrackedObject = GetComponent<SteamVR_TrackedObject>();
        _transform = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        RotationTracker.UpdateHMD(_transform.rotation);
    }
}
