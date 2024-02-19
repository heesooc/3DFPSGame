using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour, IHitable
{
    public GameObject DrumEffectPrefab;
    public float UpPower = 20f;
    public float DestroyTime = 3f;
    public float _timer;

    public float ExplosionRadius = 3;

    private int _hitCount = 0;

    public int Damage = 70;

    public void Hit(int damage)
    {
        _hitCount += 1;
        if( _hitCount >= 3 )
        {
           

            GameObject effect = Instantiate(DrumEffectPrefab);
            effect.transform.position = this.gameObject.transform.position;

            GameObject drum = this.gameObject;
            Rigidbody rigidbody = drum.GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Vector3.up * UpPower, ForceMode.Impulse);
            rigidbody.AddTorque(new Vector3(1, 0, 1) * UpPower / 2f); // 회전력



        }
    }

    private void Update()
    {
        if (_hitCount >= 3)
        {
            _timer += Time.deltaTime;
            
            if (_timer >= DestroyTime)
            {
                Destroy(gameObject);

            }
        }
    }

    // 1. 터질 때
    private void OnCollisionEnter(Collision other)
    {
        // 2. 범위 안에 있는 모든 콜라이더를 찾는다.
        int findLayer = LayerMask.GetMask("Player") |  LayerMask.GetMask("Monster");
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, findLayer);

        // 3. 찾은 콜라이더 중에서 타격 가능한(IHitable) 오브젝트를 찾아서 Hit()한다.
        foreach (Collider collider in colliders)
        {
            IHitable hitable = collider.GetComponent<IHitable>();
           if(collider.TryGetComponent<IHitable>(out hitable))
            // if (hitable != null)
            {
                hitable.Hit(Damage);
            }
        }
    }

}

