using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int Health;
    public int MaxHealth = 100;
    void Start()
    {
        Init();
    }

    public void Init()
    {
        Health = MaxHealth;   
    }
}
