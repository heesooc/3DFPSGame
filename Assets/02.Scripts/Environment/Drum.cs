using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour, IHitable
{

    private int _hitCount = 0;

    public void Hit(int damage)
    {
        _hitCount += 1;
        if( _hitCount >= 3 )
        {
            Destroy(gameObject);
        }
    }
}

