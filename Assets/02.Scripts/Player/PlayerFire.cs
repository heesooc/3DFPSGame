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

   /* public int PoolSize = 3;
    private List<GameObject> _bombPool = null;

    private void Awake()
    {
        // 오브젝트 풀 할당
        _bombPool = new List<GameObject>();

        // 폭탄 프리팹으로부터 폭탄을 풀 사이즈만큼 생성
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject bomb = Instantiate(BombPrefab);
            bomb.SetActive(false); 

            // 생성한 총알을 풀에다가 넣음
            _bombPool.Add(bomb);
        }
    }*/
    private void Start()
    {
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
        // 1. 마우스 오른쪽 버튼을 감지 && 수류탄 개수가 0보다 크면
        if (Input.GetMouseButtonDown(1) && BombRemainCount > 0)
        {
            BombRemainCount--;

            RefreshUI();

            // 2. 수류탄 던지는 위치에다가 수류탄 생성
            GameObject bomb = Instantiate(BombPrefab);
            bomb.transform.position = FirePosition.position;

            // 3. 시선이 바라보는 방향(카메라가 바라보는 방향 = 카메라의 전방)으로 수류탄 투척
            Rigidbody rigidbody = bomb.GetComponent<Rigidbody>();
            rigidbody.AddForce(Camera.main.transform.forward * ThrowPower, ForceMode.Impulse);
                // AddForce: Rigidbody에 힘을 가하는 함수
                // ForceMode.Impulse: Rigidbody에 순간적인 힘을 가하는 모드. 이 모드로 힘을 가하면 물체에 갑작스럽고 강한 힘을 줄 수 있음
        
        }
    }
}
