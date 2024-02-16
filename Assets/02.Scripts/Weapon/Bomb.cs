using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
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
    private void OnCollisionEnter(Collision other)
    {
        gameObject.SetActive(false); // 창고에 넣는다.

        GameObject effect = Instantiate(BombEffectPrefab);
        effect.transform.position = this.gameObject.transform.position;
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
