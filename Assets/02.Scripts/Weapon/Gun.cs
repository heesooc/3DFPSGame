using System.Collections.Generic;
using UnityEngine;

public enum GunType
{
    Rifle, // 따발총
    Sniper, // 저격총
    Pistol, // 권총
}

public class Gun : MonoBehaviour // 인벤토리: 수납된 물품 목록 (껐다 킬 수 있도록. 리스트(or배열) 활용하기. Update문 NO)
{
    public GunType GType;

    /*[Header("총 프리팹")]
    public GameObject RiflePrefab;
    public GameObject SniperPrefab;
    public GameObject PistolPrefab;
    // - 풀 사이즈
    public int PoolSize = 20;
    // - 오브젝트(총) 풀 
    private List<GameObject> _GunPool = null;*/
    
    // - 대표 이미지
    public Sprite ProfileImage;

    // - 공격력
    public int Damage = 10;

    // - 발사 쿨타임
    public float FireCooltime = 0.2f;

    // - 총알 개수
    public int BulletRemainCount;
    public int BulletMaxCount = 30;

    // - 재장전 시간
    public float ReloadTime = 1.5f;


    /*private void Awake()
    {
        // 오브젝트 풀 할당
        _GunPool = new List<GameObject>();
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject Rifle = Instantiate(RiflePrefab);
            GameObject Sniper = Instantiate(SniperPrefab);
            GameObject Pistol = Instantiate(PistolPrefab);
            Rifle.SetActive(false); // 끈다.
            Sniper.SetActive(false);
            Pistol.SetActive(false);
            _GunPool.Add(Rifle);
            _GunPool.Add(Sniper);
            _GunPool.Add(Pistol);
        }
    }*/


    private void Start()
    {
        // 총알 개수 초기화
        BulletRemainCount = BulletMaxCount;
    }

    

    /*private void CheckGun()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GType = GunType.Rifle;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GType = GunType.Sniper;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GType = GunType.Pistol;
        }

    }*/

}
