using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : BaseCtrl
{
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    IEnumerator enumerator;
    public Transform player;
    public Animator anim;
    public Rigidbody rbody;
    public Weapon weapon;
    public CapsuleCollider col;
    [SerializeField]
    private MeshRenderer[] meshs;
    public GameObject iceRad;
    public float speed;
    public int health;
    
    private float v, h;
    private float attackTime = 0f;
    private float attackInter = 0.7f;
    private float animInter = 1.0f;
    float angularPower = 2f;
    float scaleValue = 0.1f;

    [SerializeField]
    private int attackCount;

    private bool isMove;
    private bool runDown;
    private bool isDamage;
    [SerializeField]
    public bool _stopSkill = true;
    bool isShoot;

    Vector3 moveVec;
    Manager manager = new Manager();
    GameObject Ice;
    public void Update()
    {
        if (Time.time > attackTime + attackInter + 0.2f && Input.GetMouseButtonDown(0) == false)
        {
            //_stopSkill = true;
            attackCount = 0;
        }
        else if(Time.time < attackTime + attackInter && Input.GetMouseButtonDown(0) == true)
        {
            attackCount++;
        }
    }

    public override void Init()
    {
        cretureType = Define.CretureType.Player;
        meshs = GetComponentsInChildren<MeshRenderer>();
        anim = GetComponent<Animator>();
        Manager.Input.KeyAction -= GetInput;
        Manager.Input.KeyAction += GetInput;
        Manager.Input.MouseAction -= OnMouseEvent;
        Manager.Input.MouseAction += OnMouseEvent;

        speed = 3f;
        isMove = false;
        enumerator = PointerUpCor();
        attackTime = 0f;
        //Ice = Resources.Load<GameObject>("Prefabs/IceSlash");
        Ice = Resources.Load<GameObject>("Prefabs/Ice");
    }

    void GetInput()
    {
        if (_stopSkill != true || manager.IsGameOver == true) return;
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        runDown = Input.GetButton("Run");
        moveVec = new Vector3(h, 0f, v).normalized;
        State = Define.State.Move;
    }

    protected override void UpdateMoving()
    {
        if (_stopSkill != true)
        {
            anim.SetBool("Run", false);
            return;
        }
            anim.applyRootMotion = false;
        rbody.velocity = Vector3.zero;
        player.position += moveVec * speed * Time.deltaTime;
        player.position += moveVec * speed * (runDown ? 1.5f : 1f) * Time.deltaTime;
        //player.transform.LookAt(player.position + moveVec);
        player.transform.rotation = Quaternion.Slerp(player.rotation, Quaternion.LookRotation(moveVec), 40f);
        anim.SetBool("Run", true);
        if (Input.anyKey == false)
        {
            transform.position = transform.position;
            State = Define.State.Idle;
            anim.SetBool("Run", false);
        }
       
    }

    protected override void UpdateAttack()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);

        if (Time.time > attackTime + attackInter + 0.1f)
        {
            anim.applyRootMotion = true;
            _destPos = hit.point;
            Vector3 dir = _destPos - transform.position;
            dir.y = 0;
            transform.rotation =  Quaternion.LookRotation(dir.normalized);
            
            attackTime = Time.time;
            anim.SetBool("Attack2",true);
            weapon.Use();
        }
    }

    protected void UpdateAttack2()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);

        Debug.Log("attack2");
        anim.applyRootMotion = true;
        _destPos = hit.point;
        Vector3 dir = _destPos - transform.position;
        transform.rotation = Quaternion.LookRotation(dir.normalized);
        attackTime = Time.time;
        anim.SetBool("Attack2", false);
        anim.SetBool("Attack3", true);
        //weapon.Use();
    }

    public void Skill(int btn)
    {
        if (_stopSkill == false) return;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);
        if(btn == 0)
        {
            StartCoroutine(SkillCor(hit.point, btn));
        }
        else if(btn == 1)
        {
            StartCoroutine(SkillCor(hit.point, btn));
        }
    }


    void OnMouseEvent(Define.MouseEvent evt)
    {
        if (manager.IsGameOver == true) return;
        if (Time.time < attackTime + attackInter) return;
        _stopSkill = false;
        switch (State)
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                //State = Define.State.Idle;
                break;
            case Define.State.Move:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Attack:
                {
                }
                break;
        }
    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        _stopSkill = false;
        switch (evt)
        {
            case Define.MouseEvent.Click:
                {
                    
                    StartCoroutine(PointerUpCor());
                }
                break;
            case Define.MouseEvent.PointerDown:
                {
                    /* attackTime = Time.time;
                     _stopSkill = false;
                     _destPos = hit.point;
                     State = Define.State.Attack;*/
                }
                break;
            case Define.MouseEvent.Press:
                {

                }
                break;
            case Define.MouseEvent.PointerUp:
                {
                    
                }
                break;
        }
    }
    void ColliderEvent()
    {
        StartCoroutine(Swing());
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            if(!isDamage)
            {
                EnemyCtrl enemy = other.GetComponent<EnemyCtrl>();
                health -= enemy.damage;
                StartCoroutine(OnDamage());
                if (health <= 0)
                {
                    State = Define.State.Die;
                    Manager.Instance.IsGameOver = true;
                }
            }
        }
    }

    IEnumerator PointerUpCor()
    {
        _stopSkill = false;
        State = Define.State.Attack;

        yield return new WaitForSeconds(0.7f);
        State = Define.State.Idle;
        anim.SetBool("Attack2", false);
        anim.applyRootMotion = false;
        rbody.velocity = Vector3.zero;

        if (attackCount > 1)
        {
            UpdateAttack2();
            yield return new WaitForSeconds(1.3f);
            anim.SetBool("Attack3", false);
            anim.applyRootMotion = false;
            rbody.velocity = Vector3.zero;
        }
        _stopSkill = true;
    }

    IEnumerator OnDamage()
    {
        isDamage = true;
        yield return new WaitForSeconds(1f);
        isDamage = false;
    }

    IEnumerator Swing()
    {
        weapon.Use2();
        yield return null;
    }

    IEnumerator SkillCor(Vector3 dir, int btn)
    {
        attackTime = Time.time;
        _destPos = dir - transform.position;
        transform.rotation = Quaternion.LookRotation(_destPos);
        _stopSkill = false;
        anim.applyRootMotion = true;
        if(btn == 0)
        {
            anim.SetTrigger("Skill1");
            yield return new WaitForSeconds(1f);
            GameObject _Ice = Instantiate(Ice, transform.localPosition + transform.TransformDirection(Vector3.forward * 2) + Vector3.up * 1.1f, transform.rotation);
            _Ice.GetComponent<Projectile>().Velocity = transform.forward * 15f;
            _Ice.transform.rotation = Quaternion.LookRotation(_destPos);
            yield return new WaitForSeconds(1f);
        }
        else if(btn == 1)
        {
            anim.SetTrigger("Skill2");
            yield return new WaitForSeconds(1f);
            GameObject _Ice2 = Manager.Resource.Instantiate("Spell_Ice_3");
            //GameObject _iceRad = Instantiate(iceRad, _Ice2.transform.position, _Ice2.transform.rotation);
            /*StartCoroutine(GainPowerTimer(_iceRad));
            StartCoroutine(GainPower(_iceRad));*/
            Collider[] col = Physics.OverlapSphere(_Ice2.transform.position, 5.0f);
            foreach(Collider hit in col)
            {
                EnemyCtrl enemy = hit.GetComponent<EnemyCtrl>();
                //enemy.curHealth -= 100;
            }
            _Ice2.transform.position = transform.localPosition + transform.TransformDirection(Vector3.forward);
            yield return new WaitForSeconds(1f);
            Manager.Resource.Destroy(_Ice2);
        }
        anim.applyRootMotion = false;
        rbody.velocity = Vector3.zero;
        _stopSkill = true;
    }

    IEnumerator GainPowerTimer(GameObject obj)
    {
        yield return new WaitForSeconds(0.8f);
        isShoot = true;
        Destroy(obj);
    }

    IEnumerator GainPower(GameObject obj)
    {
        while (!isShoot)
        {
            scaleValue += 0.18f;
            obj.transform.localScale = Vector3.one * scaleValue;
            yield return null;
        }
    }
}