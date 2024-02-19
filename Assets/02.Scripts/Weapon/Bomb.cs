using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // 목표: 수류탄 폭발 범위 데미지 기능 구현
    // 필요 속성:
    // - 범위
    public float ExplosionRadius = 3;
    // 구현 순서: 
    // 1. 터질 때
    // 2. 범위 안에 있는 모든 콜라이더를 찾는다.
    // 3. 찾은 콜라이더 중에서 타격 가능한(IHitable) 오브젝트를 찾는다.
    // 4. Hit() 한다.

    public int Damage = 60;

    // 실습 과제 8. 수류탄이 폭발할 때(사라질 때)/ 폭발 이펙트를 자기 위치에 생성하기
    public GameObject BombEffectPrefab;

    /* public int PoolSize = 1;
    private List<GameObject> _bombPool = null;
    public GameObject FirePrefab;

    private void Awake()
    {
        // 폭탄 프리팹으로부터 폭탄을 풀 사이즈만큼 생성
        for (int i = 0; i < PoolSize; i++)
        {
            FirePrefab = Instantiate(FirePrefab);
            FirePrefab.SetActive(false);

            // 생성한 총알을 풀에다가 넣음
            _bombPool.Add(FirePrefab);
        }
    }*/

    // 1. 터질 때
    private void OnCollisionEnter(Collision other)
    {
        gameObject.SetActive(false); // 창고에 넣는다.

        GameObject effect = Instantiate(BombEffectPrefab);
        effect.transform.position = this.gameObject.transform.position;

        // 2. 범위 안에 있는 모든 콜라이더를 찾는다.
        // -> 피직스.오버랩 함수는 특정 영역(radius) 안에 있는 특정 레이어들의 게임 오브젝트의
        //    콜라이더 컴포넌트들을 모두 찾아 배열로 반환하는 함수
        // 영역의 형태: 스피어, 큐브, 캡슐
        int layer = /*LayerMask.GetMask("Player") | */ LayerMask.GetMask("Monster");
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, layer);

        // 3. 찾은 콜라이더 중에서 타격 가능한(IHitable) 오브젝트를 찾아서 Hit()한다.
        foreach (Collider collider in colliders)
        {
            IHitable hitable = collider.GetComponent<IHitable>();
            if(hitable != null)
            {
                hitable.Hit(Damage);
            }
        } 


        /*foreach (GameObject go in _bombPool)
        {
            if (!go.gameObject.activeInHierarchy)
            {
                FirePrefab.SetActive(true);
                FirePrefab.transform.position = gameObject.transform.position;
            }

        }*/
    }
}
