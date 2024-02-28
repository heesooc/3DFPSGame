using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public enum ItemType
{
    Health, // 체력이 꽉찬다.
    Stamina, // 스태미나 꽉찬다.
    Bullet,  // 현재 들고있는 총의 총알이 꽉찬다.
}

public class Item 
{
    public ItemType ItemType;
    public int Count;

    public Item(ItemType itemType, int count)
    {
        ItemType = itemType;
        Count = count;
    }

    public bool TryUse()
    {

        if (Count == 0)
        {
            return false;
        }

        Count -= 1;

        switch (ItemType)
        {
            case ItemType.Health:
            {
                    Debug.Log("체력");
                    // Todo: 플레이어 체력 꽉차기
                    PlayerMoveAbility playerMoveAbility = GameObject.FindWithTag("Player").GetComponent<PlayerMoveAbility>();
                    playerMoveAbility.Health = playerMoveAbility.MaxHealth;
                    break;
            }

            case ItemType.Stamina:
            {
                    Debug.Log("체력");

                    // Todo: 플레이어 스태미나 꽉차기
                    PlayerMoveAbility playerMoveAbility = GameObject.FindWithTag("Player").GetComponent<PlayerMoveAbility>();
                    playerMoveAbility.Stamina = PlayerMoveAbility.MaxStamina;

                    break;
            }
            case ItemType.Bullet:
            {
                    Debug.Log("체력");

                    // Todo: 플레이어 현재 들고있는 총의 총알이 꽉찬다.
                    PlayerGunFireAbility playerGunAbility = GameObject.FindWithTag("Player").GetComponent<PlayerGunFireAbility>();
                    playerGunAbility.CurrentGun.BulletRemainCount = playerGunAbility.CurrentGun.BulletMaxCount;
                    playerGunAbility.RefreshUI();
                    break;
            }
        }

        return true;
    }
}
