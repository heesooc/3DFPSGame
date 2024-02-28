using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBombEvent : MonoBehaviour
{
    private PlayerBombFireAbility _owner;

    void Start()
    {
        _owner = GetComponentInParent<PlayerBombFireAbility>();
    }

    public void BombEvent()
    {
        _owner.PlayerBomb();
    }
}
   
