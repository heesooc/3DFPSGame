using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemInventory : MonoBehaviour
{
    public Text HealthItemCountTextUI;
    public Text StaminaItemCountTextUI;
    public Text BulletItemCountTextUI;

    public static UI_ItemInventory Instance;
    private void Awake()
    {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
    }

    // UI를 새로고침 하는 함수
    public void Refresh()
    {

        HealthItemCountTextUI.text = $"x{ItemManager.Instance.GetItemCount(ItemType.Health)}";
        StaminaItemCountTextUI.text = $"x{ItemManager.Instance.GetItemCount(ItemType.Stamina)}";
        BulletItemCountTextUI.text = $"x{ItemManager.Instance.GetItemCount(ItemType.Bullet)}";
    }
}
