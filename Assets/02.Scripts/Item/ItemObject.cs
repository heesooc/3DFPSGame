using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ItemObject : MonoBehaviour
{
    // 실습 과제 36. 상태패턴 방식으로 일정 거리가 되면 아이템이 Slerp(구면선형보간)로 날라오게 하기 (대기 상태, 날라오는 상태)
    public enum ItemState
    {
        Idle,   // 대기 상태 (플레이어와 거리를 체크한다.)
        // 전이 (if 충분히 가까워 지면..)
        Trace,  // 날라오는 상태 (0.6초에 걸쳐서 Slerp로 플레이어에게 날라온다.)
    }
    public ItemType ItemType;
    private ItemState _itemState = ItemState.Idle;

    private Transform _player;
    public float EatDistance = 10f;

    private Vector3 _startPosition;
    private const float TRACE_DURATION = 0.3f;
    private float _progress = 0f;

    

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _startPosition = transform.position;
    }
    private void Update()
    {
        switch(_itemState)
        {
            case ItemState.Idle:
                Idle();
                break;

            case ItemState.Trace:
                Trace();
                break;  
        }
    }

    public void Init()
    {
        _progress = 0f;
        _traceCoroutine = null;
        _itemState = ItemState.Idle;
    }

    private void Idle()
    {
        // 대기 상태의 행동: 플레이어와의 거리를 체크한다.
        float distance = Vector3.Distance(_player.position, transform.position);
        // 전이 조건: 충분히 가까워 지면..
        if (distance <= EatDistance)
        {
            _itemState = ItemState.Trace;
        }
    }

    private Coroutine _traceCoroutine;
    private void Trace()
    {

        if (_traceCoroutine != null)
        {
            StopCoroutine(_traceCoroutine); // 코루틴: 세밀한 실행 제어가 가능 (시간: 대기, 조절, 일시 중지)

        }
        _traceCoroutine = StartCoroutine(Trace_Coroutine());

    }

    private IEnumerator Trace_Coroutine() 
    {
        // 실습 과제 37. 36번 과제의 날라오는 상태를 update가 아닌 코루틴 방식으로 변경

        while (_progress < 0.8f)
        {
            // 진행도를 누적하는 시간을 계산하고, 현재 위치를 갱신합니다.
            _progress += Time.deltaTime / TRACE_DURATION;
            transform.position = Vector3.Slerp(_startPosition, _player.position, _progress);
            
            // 다음 프레임까지 대기한다.
            yield return null;
        }

        // 진행도가 0.8 이상이면 아이템을 추가하고 오브젝트를 비활성화합니다.
        ItemManager.Instance.AddItem(ItemType);
        gameObject.SetActive(false);

    }

}
