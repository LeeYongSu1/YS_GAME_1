using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCtrl : MonoBehaviour
{
	[SerializeField]
	protected Vector3 _destPos;
	[SerializeField]
	protected Define.State _state = Define.State.Idle;
	
	[SerializeField]
	protected GameObject _lockTarget;

	public Define.CretureType cretureType { get; protected set; }
	 = Define.CretureType.Unknown;

	public virtual Define.State State
	{
		get { return _state; }
		set
		{
			_state = value;
            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case Define.State.Die:
					anim.SetTrigger("Die");
                    break;
                case Define.State.Idle:
                    
                    break;
                case Define.State.Move:
                    
                    break;
                case Define.State.Attack:
                    
                    break;
            }
        }
	}

	private void Start()
	{
		Init();
	}

	void FixedUpdate()
	{
		switch (State)
		{
			case Define.State.Die:
				UpdateDie();
				break;
			case Define.State.Move:
				UpdateMoving();
				break;
			case Define.State.Idle:
				UpdateIdle();
				break;
			case Define.State.Attack:
				UpdateAttack();
				break;
		}
	}

	public abstract void Init();

	protected virtual void UpdateDie() { }
	protected virtual void UpdateMoving() { }
	protected virtual void UpdateIdle() { }
	protected virtual void UpdateAttack() { }
}
