using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemType ItemType;

    // Todo 1. 아이템 프리팹을 3개(Health, Stamina, Bullet) 만든다. (도형이나 색깔 다르게해서 구별되게)
    // Todo 2. 플레이어와 일정 거리가 되면 아이템이 먹어지고 사라진다. 

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            /*// 플레이어와 나의 거리를 알고 싶다.
            float distance = Vector3.Distance(collider.transform.position, collider.transform.forward);
            Debug.Log(distance);*/

            // 1. 아이템 매니저(인벤토리)에 추가하고,
            ItemManager.Instance.AddItem(ItemType);

            // 2. 사라진다.
            gameObject.SetActive(false);
        }        
    }
}
