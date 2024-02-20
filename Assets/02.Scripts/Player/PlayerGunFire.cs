using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGunFire : MonoBehaviour
{
    public Gun CurrentGun; // 현재 들고있는 총
    private int _currentGunIndex; // 현재 들고 있는 총의 순서


    private float _timer;

    private const int DefaultFOV = 60;
    private const int ZoomFOV = 20;
    private bool _isZoomMode = false; // 줌 모드냐?

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
    public Text ReloadTextUI;
    
    private bool _isReloading = false;

    // 무기 이미지 UI
    public Image GunImageUI;
    

    private void Start()
    {
        _currentGunIndex = 0;

        RefreshUI();
        RefreshGun();
    }

    private void RefreshUI()
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
        // 마우스 휠 버튼 눌렀을 때 && 현재 총이 스나이퍼
        if (Input.GetMouseButtonDown(2) && CurrentGun.GType == GunType.Sniper)
        {
            _isZoomMode = !_isZoomMode; // 줌 모드 뒤집기
            RefreshZoomMode();
            RefreshUI();
        }


        if (Input.GetKeyDown(KeyCode.LeftBracket)) // '['
        {
            _currentGunIndex--;
            if (_currentGunIndex < 0)
            {
                _currentGunIndex = GunInventory.Count - 1;
            }
            CurrentGun = GunInventory[_currentGunIndex];
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
            RefreshGun();
            RefreshUI();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentGunIndex = 0;
            CurrentGun = GunInventory[0];
            _isZoomMode = false;
            RefreshZoomMode();
            RefreshGun();
            RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentGunIndex = 1;
            CurrentGun = GunInventory[1];
            RefreshGun();
            RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _currentGunIndex = 2;  
            CurrentGun = GunInventory[2];
            _isZoomMode = false;
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

        // 1. 만약에 마우스 왼쪽 버튼을 누른 상태 && 쿨타임이 다 지난 상태
        if (Input.GetMouseButton(0) && _timer >= CurrentGun.FireCooltime && CurrentGun.BulletRemainCount > 0)
        {
            // 재장전 취소
            if (_isReloading)
            {
                ReloadTextUI.text = "";
                StopAllCoroutines();
                
                _isReloading = false;
            }
            CurrentGun.BulletRemainCount--;
            RefreshUI();

            _timer = 0;

            

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
                // 6. 이펙트가 쳐다보는 방향을 부딛힌 위치의 법선 벡터로 한다.
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play();
            }

            
        }

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
}
