using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGunFireAbility : MonoBehaviour
{
    private Animator _animator;

    public Gun CurrentGun; // 현재 들고있는 총
    private int _currentGunIndex; // 현재 들고 있는 총의 순서


    private float _timer;

    private const int DefaultFOV = 60;
    private const int ZoomFOV = 20;
    private bool _isZoomMode = false; // 줌 모드냐?

    private const float ZoomInDuration = 0.3f;
    private const float ZoomOutDuration = 0.2f;
    private float _zoomProgress; // 줌 진행률: 0 ~ 1

    public GameObject CrosshairUI;
    public GameObject CrosshairZoomUI;

    // 총을 담는 인벤토리
    public List<Gun> GunInventory;

    // 목표: 마우스 왼쪽 버튼을 누르면 시선이 바라보는 방향으로 총을 발사하고 싶다.
    // 필요 속성
    // - 총알 튀는 이펙트 프리팹
    public ParticleSystem HitEffect;

    // UI 위에 text로 표시하기 (ex. 30/30)
    public Text GunTextUI;
    public TextMeshProUGUI ReloadTextUI;

    private bool _isReloading = false;

    // 무기 이미지 UI
    public Image GunImageUI;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public List<GameObject> MuzzleEffects;

    private void Start()
    {
        foreach (GameObject muzzleEffect in MuzzleEffects)
        {
            muzzleEffect.SetActive(false);
        }

        _currentGunIndex = 0;

        RefreshUI();
        RefreshGun();
    }

    public void RefreshUI()
    {
        GunImageUI.sprite = CurrentGun.ProfileImage;
        GunTextUI.text = $"{CurrentGun.BulletRemainCount}/{CurrentGun.BulletMaxCount}";

        CrosshairUI.SetActive(!_isZoomMode);
        CrosshairZoomUI.SetActive(_isZoomMode);
    }

    // 줌 모드에 따라 카메라 FOV(Field Of View:시야) 수정해주는 메서드
    private void RefreshZoomMode()
    {
        if (!_isZoomMode)
        {
            Camera.main.fieldOfView = DefaultFOV;

        }
        else
        {
            Camera.main.fieldOfView = ZoomFOV;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Go)
        {
            return;
        }

        // 마우스 휠 버튼 눌렀을 때 && 현재 총이 스나이퍼
        if (Input.GetMouseButtonDown(2) && CurrentGun.GType == GunType.Sniper)
        {
            _isZoomMode = !_isZoomMode; // 줌 모드 뒤집기
            _zoomProgress = 0f;
            RefreshUI();

        }

        if (CurrentGun.GType == GunType.Sniper && _zoomProgress < 1)
        {
            if (_isZoomMode)
            {
                _zoomProgress += Time.deltaTime / ZoomInDuration;
                Camera.main.fieldOfView = Mathf.Lerp(DefaultFOV, ZoomFOV, _zoomProgress);
            }
            else
            {
                _zoomProgress += Time.deltaTime / ZoomOutDuration;
                Camera.main.fieldOfView = Mathf.Lerp(ZoomFOV, DefaultFOV, _zoomProgress);
            }
        }





        if (Input.GetKeyDown(KeyCode.LeftBracket)) // '['
        {
            _currentGunIndex--;
            if (_currentGunIndex < 0)
            {
                _currentGunIndex = GunInventory.Count - 1;
            }
            CurrentGun = GunInventory[_currentGunIndex];
            _isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();
            RefreshGun();
            RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket)) // ']'
        {
            _currentGunIndex++;
            if (_currentGunIndex >= GunInventory.Count)
            {
                _currentGunIndex = 0;
            }
            CurrentGun = GunInventory[_currentGunIndex];
            _isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();
            RefreshGun();
            RefreshUI();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentGunIndex = 0;
            CurrentGun = GunInventory[0];
            _isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();
            RefreshGun();
            RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentGunIndex = 1;
            CurrentGun = GunInventory[1];
            _isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();
            RefreshGun();
            RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _currentGunIndex = 2;
            CurrentGun = GunInventory[2];
            _isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();
            RefreshGun();
            RefreshUI();
        }

        // 실습 과제 16. R키 누르면 1.5초 후 재장전(중간에 총 쏘는 행위를 하면 재장전 취소)
        if (Input.GetKeyDown(KeyCode.R) && CurrentGun.BulletRemainCount < CurrentGun.BulletMaxCount)
        {
            if (!_isReloading)
            {

                StartCoroutine(Reload_Coroutine());
            }

        }
        _timer += Time.deltaTime;

        // 1. 만약에 마우스 왼쪽 버튼을 누른 상태 && 쿨타임이 다 지난 상태 && 총알 개수 > 0
        if (Input.GetMouseButton(0) && _timer >= CurrentGun.FireCooltime && CurrentGun.BulletRemainCount > 0)
        {


            // 재장전 취소
            if (_isReloading)
            {
                ReloadTextUI.text = "";
                StopAllCoroutines();

                _isReloading = false;
            }
            _animator.SetTrigger("Shot");

            CurrentGun.BulletRemainCount--;
            RefreshUI();

            _timer = 0;

            StartCoroutine(MuzzleEffectOn_Coroutine());


            // 2. 레이(광선)을 생성하고, 위치와 방향을 설정한다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // 3. 레이를 발사한다.
            // 4. 레이가 부딪힌 대상의 정보를 받아온다.
            RaycastHit hitInfo;
            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                // 실습 과제 18. 레이저를 몬스터에게 맞출 시 몬스터 체력 닳는 기능 구현

                IHitable hitableObject = hitInfo.collider.GetComponent<IHitable>();
                if (hitableObject != null)
                {
                    hitableObject.Hit(CurrentGun.Damage);
                }

                // 5. 부딪힌 위치에 (총알이 튀는)이펙트를 위치한다.
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. 이펙트가 쳐다보는 방향을 부딪힌 위치의 '법선 벡터'로 한다.
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play();
            }


        }

    }

    private IEnumerator MuzzleEffectOn_Coroutine()
    {
        // 총 이펙트 중 하나를 켜준다.
        int randomIndex = UnityEngine.Random.Range(0, MuzzleEffects.Count);
        MuzzleEffects[randomIndex].SetActive(true);

        // 0.1초 후...
        yield return new WaitForSeconds(0.1f);

        // 꺼준다. 
        MuzzleEffects[randomIndex].SetActive(false);
    }

    private IEnumerator Reload_Coroutine()
    {
        _isReloading = true;
        ReloadTextUI.text = $"재장전 중...!";
        yield return new WaitForSeconds(CurrentGun.ReloadTime);

        CurrentGun.BulletRemainCount = CurrentGun.BulletMaxCount;
        RefreshUI();

         ReloadTextUI.text = "";
        _isReloading = false;
        yield break;
    }

    private void RefreshGun()
    {
        foreach (Gun gun in GunInventory)
        {
            /*if (gun == CurrentGun)
            {
                gun.gameObject.SetActive(true);
            }
            else
            {
                gun.gameObject.SetActive(false);
            }
*/
            gun.gameObject.SetActive(gun == CurrentGun);
        }
    }

    /* private IEnumerator ZoomIn_Coroutine()
    {
        float time = 0.3f;    // 원하는 시간
        float timer = 0f;     // 시간 누적 변수

        while (true)
        {
            timer += Time.deltaTime / time;
            Camera.main.fieldOfView = Mathf.Lerp(DefaultFOV, ZoomFOV, timer);
            yield return null;  // 다음 프레임까지 대기한다

            if (timer > 1f)
            {
                yield break;    // 코루틴의 실행을 즉시 종료한다
            }
        }
    }
        private IEnumerator ZoomOut_Coroutine()
    {
        float time = 0.3f;    // 원하는 시간
        float timer = 0f;     // 시간 누적 변수

        while (true)
        {
            timer += Time.deltaTime / time;
            Camera.main.fieldOfView = Mathf.Lerp(ZoomFOV, DefaultFOV, timer);
            yield return null;

            if (timer > 1f)
            {
                yield break;
            }
        }
    }*/
}
