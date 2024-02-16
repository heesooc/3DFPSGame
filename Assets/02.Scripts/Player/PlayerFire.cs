using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    public GameObject BombPrefab;
    public Transform FirePosition;
    public float ThrowPower = 20f;

    public int BombRemainCount;
    public int BombMaxCount = 3;

    public Text BombTextUI;

   /*이팩트에다가도 오브젝트풀링써줘*/
    public List<GameObject> BombPool; //폭탄 창고
    public int BombPoolSize = 5;
    
    private void Start()
    {
        // 폭탄 창고 생성
        BombPool = new List<GameObject>();
        for (int i = 0; i < BombMaxCount; i++) // 생성할 폭탄 개수 만큼 반복
        {
            GameObject bombObject = Instantiate(BombPrefab); // 1. 생성
            bombObject.SetActive(false);                     // 2. 비활성화
            BombPool.Add(bombObject);                        // 3. 창고에 집어 넣는다.
        }

        BombRemainCount = BombMaxCount;
        RefreshUI();
    }
    private void RefreshUI() 
    {
        BombTextUI.text = $"{BombRemainCount}/{BombMaxCount}";
    }
    private void Update()
    {
        /* 수류탄 투척 */
        // 1. 마우스 오른쪽 버튼을 눌렀을 때 && 수류탄 개수가 0보다 크면
        if (Input.GetMouseButtonDown(1) && BombRemainCount > 0)
        {
            BombRemainCount--;

            RefreshUI();

            // 2. 창고에서 수류탄을 꺼낸 다음 던지는 위치로 조절
            GameObject bomb = null;
            for (int i =0;  i < BombPool.Count; ++i) // 1. 창고를 뒤진다.
            {
                if (BombPool[i].activeInHierarchy == false) // 2. 쓸만한 폭탄을 찾는다.
                {
                    bomb = BombPool[i];
                    bomb.SetActive(true); // 3. 꺼낸다.
                    break;
                }
            }

            /* foreach (GameObject go in _bombPool)
             {
                if (!go.gameObject.activeInHierarchy)
                 {
                     bomb.SetActive(true);
                     bomb.transform.position = FirePosition.position;
                 }

             }*/

            bomb.transform.position = FirePosition.position;

            // 3. 시선이 바라보는 방향(카메라가 바라보는 방향 = 카메라의 전방)으로 수류탄 투척
            Rigidbody rigidbody = bomb.GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Camera.main.transform.forward * ThrowPower, ForceMode.Impulse);
                // AddForce: Rigidbody에 힘을 가하는 함수
                // ForceMode.Impulse: Rigidbody에 순간적인 힘을 가하는 모드. 이 모드로 힘을 가하면 물체에 갑작스럽고 강한 힘을 줄 수 있음
        
        }
    }
}
