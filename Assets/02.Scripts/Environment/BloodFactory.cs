using UnityEngine;

public class BloodFactory : MonoBehaviour
{
    public static BloodFactory Instance { get; private set; }

    [Header("피 효과 프리팹")]
    public GameObject BloodPrefab;

    // Todo. 오브젝트 풀링 적용해보세요.



    private void Awake()
    {
        Instance = this; 
    }

    public void Make(Vector3 position, Vector3 normal)
    {
        GameObject bloodObject = Instantiate(BloodPrefab);
        bloodObject.transform.position = position;
        bloodObject.transform.forward = normal;
    }

}
