using System.Collections.Generic;
using UnityEngine;

public class BloodFactory : MonoBehaviour
{
    public static BloodFactory Instance { get; private set; }

    [Header("피 효과 프리팹")]
    public GameObject BloodPrefab;
    
    private List<GameObject> _pool;
    public int Poolsize = 10;


    // Todo. 오브젝트 풀링 적용해보세요.

    private void Awake()
    {
        Instance = this; 

        _pool = new List<GameObject>();
        for (int i = 0; i < Poolsize; i++)
        {
            GameObject bloodObject = Instantiate(BloodPrefab);
            _pool.Add(bloodObject);
            bloodObject.SetActive(false);
        }
    }

    public void Make(Vector3 position, Vector3 normal)
    {
        foreach(GameObject bloodObject in _pool)
        {
            if (bloodObject.activeInHierarchy == false)
            {
                bloodObject.GetComponent<DestroyTime>()?.Init();
                bloodObject.transform.position = position;
                bloodObject.transform.forward = normal;
                bloodObject.SetActive(true);
                break;
            }
        }
    }
}
