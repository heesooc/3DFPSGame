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


    private void Start()
    {
        // 총알 개수 초기화
        BulletRemainCount = BulletMaxCount;
    }

}
