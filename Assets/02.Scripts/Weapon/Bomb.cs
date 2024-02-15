using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // 실습 과제 8. 수류탄이 폭발할 때(사라질 때)/ 폭발 이펙트를 자기 위치에 생성하기
    public GameObject BombEffectPrefab;

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);

        GameObject effect = Instantiate(BombEffectPrefab);
        effect.transform.position = this.gameObject.transform.position;

    }
}
