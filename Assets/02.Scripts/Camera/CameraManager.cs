using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CameraManager : MonoBehaviour
{
    // 실습 과제 4. 버튼에 따라 카메라 FPS/TPS 변경 (처음에는 FPS) (9번: FPS, 0번: TPS)
    // 카메라들을 관리하는 (TPS/FPS) 카메라 매니저 클래스 구현 및 싱글톤 적용

    public static CameraManager Instance;
    public bool FPSCamera = true;
    public bool TPSCamera = false;
    void Awake()
    {
        // 싱글톤 패턴 : 오직 한개의 클래스 인스턴스를 갖도록 보장
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // - 현재 카메라를 기억할 변수
    //private  _currentCamera = 9;
    //private int _camera;
}
