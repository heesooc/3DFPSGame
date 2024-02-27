using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackEvent : MonoBehaviour
{
    private Monster _owner;

    private void Start()
    {
        _owner = GetComponentInParent<Monster>();
    }

   public void AttackEvent()
    {
        //Debug.Log("어택이벤트 발생!");
        _owner.PlayerAttack();
    }
}
