using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// 아이템 공장의 역할: 아이템 오브젝트의 생성을 책임진다. 
// 팩토리 패턴
// 객체 생성을 공장 클래스를 이용해 캡슐화 처리하여 대신 "생성"하게 하는 디자인 패턴
// 객체 생성에 필요한 과정을 템플릿화 해놓고 외부에서 쉽게 사용한다.
// 장점:
// 1. 생성과 처리 로직을 분리하여 결합도를 낮출 수 있다. 
// 2. 확장 및 유지보수가 편리하다.
// 3. 객체 생성 후 공통으로 할 일을 수행하도록 지정해줄 수 있다. 
// 단점:
// 1. 상대적으로 쪼금 더 복잡하다.
// 2. 그래서 공부해야 한다.
// 3. 한마디로 단점이 없다. 
public class ItemObjectFactory : MonoBehaviour
{
    public static ItemObjectFactory Instance { get; private set; }

    public void Awake()
    {
        Instance = this; 
    }

    // 아이템 프리팹들
    public GameObject HealthItemPrefab;
    public GameObject StaminaItemPrefab;
    public GameObject BulletItemPrefab;
    public GameObject HeightItemPrefab;

    // 확률 생성 (공장아! 랜덤박스 주문할게!)
    public void MakePercent(Vector3 position)
    {
        int percentage = UnityEngine.Random.Range(0, 100);
        if (percentage <= 20)
        {
            Make(ItemType.Health, position);
        }
        else if (percentage <= 40)
        {
            Make(ItemType.Stamina, position);
        }
        else if (percentage <= 50)
        {
            Make(ItemType.Bullet, position);
        }
    }

    // 기본 생성 (공장아! 내가 원하는거 주문할게!)
    public void Make(ItemType itemType, Vector3 position)
    {
        GameObject gameObject = null;

        switch (itemType)
        {
            case ItemType.Health:
                {
                    gameObject = Instantiate(HealthItemPrefab);
                    break;
                }
            case ItemType.Stamina:
                {
                    gameObject = Instantiate(StaminaItemPrefab);
                    break;
                }
            case ItemType.Bullet:
                {
                    gameObject = Instantiate(BulletItemPrefab);
                    break;
                }
        }

        if (gameObject != null)
        {
            gameObject.transform.position = position;
        }
    }

}
