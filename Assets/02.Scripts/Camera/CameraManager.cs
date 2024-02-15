using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraMode
{
    FPS,
    TPS,
    Top,
    Back,
}

// 역할: 카메라를 관리하는 관리자
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    private FPSCamera _FPSCamera;
    private TPSCamera _TPSCamera;

    public CameraMode Mode = CameraMode.FPS;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        _FPSCamera = GetComponent<FPSCamera>();
        _TPSCamera = GetComponent<TPSCamera>();

        SetCameraMode(CameraMode.FPS);
    }

    public void SetCameraMode(CameraMode mode)
    {
        Mode = mode;

    }
}