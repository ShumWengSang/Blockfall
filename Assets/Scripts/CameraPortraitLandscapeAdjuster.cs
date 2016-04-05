using UnityEngine;
using System.Collections;
using AdvancedInspector;
public class CameraPortraitLandscapeAdjuster : MonoBehaviour {
    [Group("Camera Ortho Size")]
    public int PortraitCameraSize;
    [Group("Camera Ortho Size")]
    public int LandScapeCameraSize;

	// Use this for initialization
    Camera mainCam;
	void Start () {
        //DeviceChange.OnOrientationChange += OnOrientationChange;
        mainCam = Camera.main;
        OnOrientationChange(Input.deviceOrientation);
	}

    void OnDisable()
    {
        DeviceChange.OnOrientationChange -= OnOrientationChange;
    }

    void OnOrientationChange(DeviceOrientation newOrientation)
    {
        switch(newOrientation)
        {
            case DeviceOrientation.Portrait:
                mainCam.orthographicSize = PortraitCameraSize;
                break;
            case DeviceOrientation.LandscapeLeft:
            case DeviceOrientation.LandscapeRight:
                mainCam.orthographicSize = LandScapeCameraSize;
                break;
            default:
                Debug.LogWarning("Device orientation not matched with existing switch case statements");
                break;
        }
    }
}
