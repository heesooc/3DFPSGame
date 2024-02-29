using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public enum MonsterState // 몬스터의 상태
{
    Idle,       // 대기
    Patrol,     // 순찰    
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

    // private CharacterController _characterController;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;


    public Transform _target;           // 플레이어
    public float FindDistance = 5f;     // 감지 범위
    public float AttackDistance = 2f;   // 공격 범위
    public float MoveSpeed = 4f;         // 이동 상태
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

    public Transform PatrolTarget;
    private float _idleTimer = 0f;
    private const float IDLE_DURATION = 3f;

    private MonsterState _currentState = MonsterState.Idle;


    void Start()
    {
       // _characterController = GetComponent<CharacterController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = MoveSpeed;

        _animator = GetComponentInChildren<Animator>();

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
        if (GameManager.Instance.State != GameState.Go)
        {
            return;
        }

        HealthSliderUI.value = (float)Health / (float)MaxHealth; // 0 ~ 1

        // 상태 패턴: 상태에 따라 행동을 다르게 하는 패턴
        // 1. 몬스터가 가질 수 있는 행동에 따라 상태를 나눈다.
        // 2. 상태들이 조건에 따라 자연스럽게 전환(Transition)되게 설계한다. 
        
        switch (_currentState) // Update() 안에서 계속 호출
        {
            case MonsterState.Idle:
                Idle();
                break;

            case MonsterState.Patrol:
                Patrol();
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

            case MonsterState.Die: 
                Die(); 
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
            _animator.SetTrigger("IdleToTrace");
            _currentState = MonsterState.Trace;
        }

        // 과제 38. 순찰 상태 구현: Idle 상태에서 {N초} 이상 머물러 있으면 {특정 지점}으로 순찰을 간다.
        _idleTimer += Time.deltaTime;
        if (PatrolTarget != null && _idleTimer >= IDLE_DURATION)
        {
            // {특정 지점}으로 순찰을 감
            Debug.Log("상태 전환: Idle -> Patrol");
            _animator.SetTrigger("IdleToPatrol");
            _currentState = MonsterState.Patrol;
            _idleTimer = 0f;
        }
    }

    private void Patrol()
    {
        // NavMeshAgent의 '멈춤 거리'를 0으로 설정. 이는 목표지점까지 계속 이동하게 함.
        _navMeshAgent.stoppingDistance = 0f;
        // NavMeshAgent의 '목표지점'을 순찰 지점으로 설정
        _navMeshAgent.SetDestination(PatrolTarget.position);

        // 플레이어와 몬스터 사이의 거리를 계산하고, 이 거리가 발견 범위(FindDistance) 이내라면 추적 상태로 전환
        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태 전환: Patrol -> Trace");
            _animator.SetTrigger("PatrolToTrace");
            _currentState = MonsterState.Trace;
        }
        // NavMeshAgent의 경로 계산이 끝났고, 남아있는 거리가 허용 오차(TOLERANCE) 이하라면 복귀 상태로 전환
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE) // pathPending: 경로 보류 중
        {
            Debug.Log("상태 전환: Patrol -> Comeback");
            _animator.SetTrigger("PatrolToComeback");
            _currentState = MonsterState.Comeback;
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
        // _characterController.Move(dir * MoveSpeed * Time.deltaTime);

        // 내비게이션이 접근하는 최소 거리를 공격 가능 거리로 설정
        _navMeshAgent.stoppingDistance = AttackDistance;

        // 내비게이션의 목적지를 플레이어의 위치로 한다.
        _navMeshAgent.destination = _target.position;

        // 3. 쳐다본다.
        //transform.forward = -dir; //(_target);

        if(Vector3.Distance(transform.position, StartPosition) >= MoveDistance)
        {
            Debug.Log("상태 전환: Trace -> Comeback");
            _animator.SetTrigger("TraceToComeback");
            _currentState = MonsterState.Comeback;
        }
        if (Vector3.Distance(_target.position, transform.position) <= AttackDistance)
        {
            Debug.Log("상태 전환: Trace -> Attack");
            _animator.SetTrigger("TraceToAttack");
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
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        // 3. 쳐다본다.
        //transform.forward = -dir; 

        // 내비게이션이 접근하는 최소 거리를 오차범위로 설정
        _navMeshAgent.stoppingDistance = TOLERANCE;

        // 내비게이션의 목적지를 시작 위치로 한다.
        _navMeshAgent.destination = StartPosition;

        // 이동 완료
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE) //허용 오차
        {
            Debug.Log("상태 전환: Comeback -> Idle");
            _animator.SetTrigger("ComebackToIdle");
            _currentState = MonsterState.Idle; 
        }
        
        if (Vector3.Distance(StartPosition, transform.position) <= TOLERANCE) //허용 오차
        {
            Debug.Log("상태 전환: Comeback -> Idle");
            _animator.SetTrigger("ComebackToIdle");
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
            _animator.SetTrigger("AttackToTrace");
            _currentState = MonsterState.Trace;
            return;     // 이 메서드를 즉시 종료
        }

        // 실습 과제 35. Attack 상태일 때 N초에 한 번 때리게 딜레이 주기
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackDelay)
        {
            _animator.SetTrigger("Attack");
            //_animator.Play("Attack");
           
        }

    }

    public void PlayerAttack()
    {
        IHitable playerHitable = _target.GetComponent<IHitable>();
        if (playerHitable != null)
        {
            Debug.Log("때렸다!");

            DamageInfo damageInfo = new DamageInfo(DamageType.Normal, Damage);
            playerHitable.Hit(damageInfo);
            // 공격한 후에는 _attackTimer를 0으로 초기화하여 다시 딜레이 시간을 측정하도록 함
            _attackTimer = 0f;
        }
    }

    private void Damaged() // 넉백
    {
        // 1. Damage 애니메이션 실행(0.5초)
        // todo: 애니메이션 실행

        // 2. 넉백 구현
        // 2-1. 넉백 시작/최종 위치를 구한다.
        if (_knockbackProgress == 0) // 진행률: 0 (시작)
        {
            _knockbackStartPosition = transform.position;
            Vector3 dir = transform.position - _target.position; // dir: 방향
            dir.y = 0;
            dir.Normalize();

            _knockbackEndPosition = transform.position + dir * KnockbackPower; // 넉백 됐을 때 위치
        }
        _knockbackProgress += Time.deltaTime / KNOCKBACK_DURATION;


        // 2-2. Lerp(선형보간)를 이용해 넉백
        transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackProgress);
        if(_knockbackProgress > 1)  // 진행률: 1 (완료)
        {
            _knockbackProgress = 0f;

            Debug.Log("상태 전환: Damaged -> Trace");
            _animator.SetTrigger("DamagedToTrace");
            _currentState = MonsterState.Trace;
        }
    }

    public void Hit(DamageInfo damage)
    {
        if(_currentState == MonsterState.Die)
        {
            return;
        }

        // Todo.데미지 타입이 크리티컬이면  피흘리기
        if (damage.DamageType == DamageType.Critical)
        {
            
            BloodFactory.Instance.Make(damage.Position, damage.Normal);
        }


        Health -= damage.Amount;


        if (Health <= 0)
        {
            Debug.Log("상태 전환: Any -> Die");
            
            _animator.SetTrigger($"Die{Random.Range(1,3)}");
            _currentState = MonsterState.Die;
        }
        else
        {
            Debug.Log("상태 전환: Any -> Damaged");
            _animator.SetTrigger("Damaged");
            _currentState = MonsterState.Damaged;
        }
    }

    private Coroutine _dieCoroutine;

    private void Die()
    {
        // 매 프레임마다 해야 할 행동을 추가

        if (_dieCoroutine == null)
        {
            _dieCoroutine = StartCoroutine(Die_Coroutine());
        }
        
    }

    private IEnumerator Die_Coroutine()
    {
        _navMeshAgent.isStopped = true;
        _navMeshAgent.ResetPath(); //현재 경로를 지움

        HealthSliderUI.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        // 죽을 때 아이템 생성
        ItemObjectFactory.Instance.MakePercent(transform.position);

        Destroy(gameObject);
    }
}
