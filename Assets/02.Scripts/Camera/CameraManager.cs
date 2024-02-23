using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraMode
{
    Start,
    FPS,
    TPS,
    Top,
    Back,
}

// 역할: 카메라를 관리하는 관리자
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    public static bool Focus
    {
        get
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                return false;
            }

            Vector3 mousePosition = Input.mousePosition;
            bool isScreen = mousePosition.x < 0 ||
                            mousePosition.x > Screen.width ||
                            mousePosition.y < 0 ||
                            mousePosition.y > Screen.height;
            return !isScreen;
        }
    }

    public CameraShake CameraShake;


    private FPSCamera _FPSCamera;
    private TPSCamera _TPSCamera;

    public float RotationSpeed = 400;

    public float X { get; private set; }
    public float Y { get; private set; }

    public Vector2 XY => new Vector2(X, Y);

    public CameraMode Mode = CameraMode.Start;

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
    }

    private void Start()
    {
        // 마우스 커서 없애고, 고정
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SetCameraMode(CameraMode.TPS);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (Input.GetKeyDown(KeyCode.Minus))
        {
            X = 0;
            Y = 0;

            FindObjectOfType<PlayerRotateAbility>()?.ResetX();
        }
    }

    private void LateUpdate()
    {
        if (!Focus)
        {
            return;
        }

        // 3. 마우스 입력을 받는다.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 4. 마우스 입력에 따라 회전 방향을 누적한다.
        X += mouseX * RotationSpeed * Time.deltaTime;
        Y += mouseY * RotationSpeed * Time.deltaTime;

        Y = Mathf.Clamp(Y, -90, 90);
    }

    public void SetCameraMode(CameraMode mode)
    {

        if (Mode == CameraMode.FPS)
        {
            //Y += _TPSCamera.Offset.y;
        }

        Vector3 currentRotation = transform.eulerAngles;

        Mode = mode;

        //X = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
        //Y = Mathf.Atan2(forward.y, Mathf.Sqrt(forward.x * forward.x + forward.z * forward.z)) * Mathf.Rad2Deg;


        _FPSCamera.enabled = (mode == CameraMode.FPS);
        _TPSCamera.enabled = (mode == CameraMode.TPS);
    }
}