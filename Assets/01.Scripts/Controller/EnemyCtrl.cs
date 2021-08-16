using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : BaseCtrl
{

    public int maxHealth;
    public int curHealth;
    public int damage;

    Rigidbody rigid;
    CapsuleCollider col;
    Material mat;
    Animator anim;
    GameObject player;
    [SerializeField]
    float _scanRange = 10;

    [SerializeField]
    float _attackRange = 2;

    private bool isDamage;

    public override void Init()
    {
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        anim = GetComponent<Animator>();
        cretureType = Define.CretureType.Enemy;
        player = GameObject.FindWithTag("Player").gameObject;
    }
    private void OnEnable()
    {
        State = Define.State.Idle;
        curHealth = maxHealth;
        col = GetComponent<CapsuleCollider>();
    }

    private void OnDisable()
    {
        
    }
    /*private void Update()
    {
        float distance = (player.transform.position - transform.position).magnitude;
        if (distance <= _scanRange)
        {
            _lockTarget = player;
            State = Define.State.Move;
            if (distance <= _attackRange)
            {
                NavMeshAgent nma = gameObject.GetComponent<NavMeshAgent>();
                nma.SetDestination(transform.position);
                State = Define.State.Skill;
                return;
            }
        }
    }*/


    protected override void UpdateIdle()
    {
        
        if (player == null)
            return;
        Debug.Log("플레이어 찾음");
        float distance = (player.transform.position - transform.position).magnitude;
        if (distance <= _scanRange)
        {
            _lockTarget = player;
            State = Define.State.Move;
            return;
        }
    }

    protected override void UpdateMoving()
    {
        // 플레이어가 내 사정거리보다 가까우면 공격
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            anim.SetBool("Move", true);
            if (distance <= _attackRange)
            {
                NavMeshAgent nma = gameObject.GetComponent<NavMeshAgent>();
                nma.SetDestination(transform.position);
                State = Define.State.Attack;
                return;
            }
        }

        // 이동
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
            anim.SetBool("Move", false);
        }
        else
        {
            NavMeshAgent nma = gameObject.gameObject.GetComponent<NavMeshAgent>();
            nma.SetDestination(_destPos);
            nma.speed = 2f;
            State = Define.State.Move;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }

    protected override void UpdateAttack()
    {
        if (_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
            anim.SetBool("Move", false);
            anim.SetBool("Attack", true);

            if (dir.magnitude > 1f)
            {
                State = Define.State.Move;
                anim.SetBool("Attack", false);
                anim.SetBool("Move", true);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee" || other.tag == "Skill")
        {
            if(!isDamage)
            {
                Weapon weapon = other.GetComponent<Weapon>();
                curHealth -= weapon.damage;
                Vector3 reactVec = transform.position - other.gameObject.transform.position;
                Vector3 hitVec = other.gameObject.transform.position - transform.position;
                StartCoroutine(Ondamage(reactVec, hitVec));
            }
        }
    }

    IEnumerator Ondamage(Vector3 reactVec, Vector3 hitVec)
    {
        transform.rotation = Quaternion.LookRotation(new Vector3(hitVec.x,0,0));
        rigid.AddForce(Vector3.back * 6, ForceMode.Impulse);
        isDamage = true;
        mat.color = Color.red;
        anim.SetTrigger("Damage");
        yield return new WaitForSeconds(0.1f);
        
        if (curHealth > 0) mat.color = Color.white;
        else
        {
            anim.SetTrigger("Die");
            rigid.velocity = Vector3.zero;
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
            PoolManager.curEnemy--;
            if(PoolManager.curEnemy <= 0)
            {
                Manager.Instance.IsGameWin = true;
            }
        }
        yield return new WaitForSeconds(0.2f);
        isDamage = false;
    }
}
