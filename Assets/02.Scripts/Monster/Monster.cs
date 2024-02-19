using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Monster : MonoBehaviour, IHitable
{
    [Range(0, 100)]
    public int Health;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;

    public void Hit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Init();
    }

    private void Update()
    {
        HealthSliderUI.value = (float)Health / (float)MaxHealth; // 0 ~ 1
    }

    public void Init()
    {
        Health = MaxHealth;   
    }
}
