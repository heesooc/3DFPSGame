using UnityEngine;

public enum DeleteType
{
    Destory,
    Inactive,
}

public class DestroyTime : MonoBehaviour
{
    public DeleteType DeleteType;

    public float DeleteTime = 1.5f;
    private float _timer = 0;

    public void Init()
    {
        _timer = 0f;
    }

    private void OnEnable() // 게임오브젝트 또는 스크립트가 
    {
        
    }

    private void OnDisable()
    {
        Init();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= DeleteTime)
        {
            if (DeleteType == DeleteType.Destory)
            {
                Destroy(this.gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
