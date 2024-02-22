using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum MonsterState // 몬스터의 상태
{
    Idle,       // 대기
    Trace,      // 추적
    Attack,     // 공격
    Comeback,   // 복귀
    Damaged,    // 공격 당함
    Die         // 사망
}


public class Monster : MonoBehaviour, IHitable
{
    [Range(0, 100)]
    public int Health;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;

    /**************************************************************/
    private CharacterController _characterController;

    public Transform _target;           // 플레이어
    public float FindDistance = 5f;     // 감지 범위
    public float AttackDistance = 2f;   // 공격 범위
    public float MoveSpeed = 4;         // 이동 상태
    public Vector3 StartPosition;       // 시작 위치
    public float MoveDistance = 10f;    // 움직일 수 있는 거리
    public const float TOLERANCE = 0.1f; // 허용오차
    public int Damage = 10;
    public const float AttackDelay = 1f;
    private float _attackTimer = 0f;

    private Vector3 _knockbackStartPosition;
    private Vector3 _knockbackEndPosition;
    private const float KNOCKBACK_DURATION = 0.2f;
    private float _knockbackProgress = 0f;
    public float KnockbackPower = 1.2f;
    

    private MonsterState _currentState = MonsterState.Idle;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        StartPosition = transform.position;

        Init();
    }
    public void Init() //최초의 프로세스
    {
        Health = MaxHealth;
    }

    private void Update()
    {
        HealthSliderUI.value = (float)Health / (float)MaxHealth; // 0 ~ 1

        // 상태 패턴: 상태에 따라 행동을 다르게 하는 패턴
        // 1. 몬스터가 가질 수 있는 행동에 따라 상태를 나눈다.
        // 2. 상태들이 조건에 따라 자연스럽게 전환(Transition)되게 설계한다. 
        
        switch (_currentState)
        {
            case MonsterState.Idle:
                Idle();
                break;

            case MonsterState.Trace:
                Trace();
                break;

            case MonsterState.Comeback:
                Comeback();
                break;

            case MonsterState.Attack:
                Attack(); 
                break;

            case MonsterState.Damaged:
                Damaged();
                break;

        }
    }

    private void Idle()
    {
        // Todo: 몬스터의 Idle 애니메이션 재생

        // Vector3.Distance(Vector3 a, Vector3 b) - a와 b 사이에 거리를 측정해 반환하는 함수
        if (Vector3.Distance(_target.position, transform.position) <= FindDistance) 
        {
            Debug.Log("상태 전환: Idle -> Trace");
            _currentState = MonsterState.Trace;
        }
    }

    private void Trace()
    {
        // Trace 상태일 때의 행동 코드를 작성
        // 플레이어에게 다가간다. 

        // 1. 방향을 구한다. (target - me)
        Vector3 dir = _target.position - transform.position;
        dir.y = 0;
        dir.Normalize();
        // 2. 이동한다.
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
        // 3. 쳐다본다.
        transform.forward = -dir; //(_target);

        if(Vector3.Distance(transform.position, StartPosition) >= MoveDistance)
        {
            Debug.Log("상태 전환: Trace -> Comeback");
            _currentState = MonsterState.Comeback;
        }
        if (Vector3.Distance(_target.position, transform.position) <= AttackDistance)
        {
            Debug.Log("상태 전환: Trace -> Attack");
            _currentState = MonsterState.Attack;
        }

    }

    private void Comeback()
    {
        // 복귀 상태의 행동 구현:
        // 시작 지점 쳐다보면서 시작지점으로 이동하기 (이동 완료하면 다시 Idle 상태로 전환)

        // 1. 방향을 구한다. (target - me)
        Vector3 dir = StartPosition - transform.position;
        dir.y = 0;
        dir.Normalize();
        // 2. 이동한다.
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
        // 3. 쳐다본다.
        transform.forward = dir; 


        // 이동 완료
        if (Vector3.Distance(StartPosition, transform.position) <= TOLERANCE) //허용 오차
        {
            Debug.Log("상태 전환: Comeback -> idle"); 
            _currentState = MonsterState.Idle; // 이동 완료
        }
    }

    private void Attack()
    {
        // 전이 사건: 플레이어와 거리가 공격 범위보다 멀어지면 다시 Trace
        if (Vector3.Distance(_target.position, transform.position) > AttackDistance)
        {
            _attackTimer = 0f;
            Debug.Log("상태 전환: Attack -> Trace");
            _currentState = MonsterState.Trace;
            return;
        }

        // 실습 과제 35. Attack 상태일 때 N초에 한 번 때리게 딜레이 주기
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackDelay)
        {
            IHitable playerHitable = _target.GetComponent<IHitable>();
            if (playerHitable != null)
            {
                Debug.Log("때렸다!");
                playerHitable.Hit(Damage);
                _attackTimer = 0f;
            }
        }

    }

    private void Damaged()
    {
        // 1. Damage 애니메이션 실행(0.5초)
        // todo: 애니메이션 실행

        // 2. 넉백 구현
        // 2-1. 넉백 시작/최종 위치를 구한다.
        if (_knockbackProgress == 0)
        {
            _knockbackStartPosition = transform.position;
            Vector3 dir = transform.position - _target.position; 
            dir.y = 0;
            dir.Normalize();

            _knockbackEndPosition = transform.position + dir * KnockbackPower;
        }
        _knockbackProgress += Time.deltaTime / KNOCKBACK_DURATION;


        // 2-2. Lerp를 이용해 넉백
        transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackProgress);
        if(_knockbackProgress > 1)
        {
            _knockbackProgress = 0f;

            Debug.Log("상태 전환: Damaged -> Trace");
            _currentState = MonsterState.Trace;
        }
    }

    public void Hit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log("상태 전환: Any -> Damaged");
            _currentState = MonsterState.Damaged;
        }
    }

    private void Die()
    {
        // 죽을 때 아이템 생성
        ItemObjectFactory.Instance.MakePercent(transform.position);

        Destroy(gameObject);
    }
}
