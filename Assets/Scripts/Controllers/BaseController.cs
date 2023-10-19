using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField] protected Vector3 _destPos;
    [SerializeField] protected GameObject _lockTarget;
    [SerializeField] protected Define.State _state = Define.State.Idle;

    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;
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
                    break;
                case Define.State.Moving:
                    //anim.SetFloat("speed", _stat.MoveSpeed);
                    //anim.SetBool("Attack", false);
                    anim.CrossFade("RUN", 0.1f);
                    break;
                case Define.State.Idle:
                    //anim.SetFloat("speed", 0);
                    //anim.SetBool("Attack", false);
                    anim.CrossFade("WAIT", 0.1f);
                    break;
                case Define.State.Skill:
                    //anim.SetBool("Attack", true);
                    //애니메이션이 처음으로 돌아간다.
                    anim.CrossFade("ATTACK", 0.1f, -1, 0);
                    break;
            }
        }
    }

    private void Start()
    {
        Init();
    }

   

    void Update()
    {
        //if (_moveToDest)
        //{
        //    Vector3 dir = _destPos - transform.position;//남은 거리
        //    if (dir.magnitude < 0.0001f)
        //    {
        //        _moveToDest = false;
        //    }
        //    else
        //    {
        //        float moveDist = Mathf.Clamp(_stat.Speed * Time.deltaTime, 0, dir.magnitude);
        //        //if (moveDist >= dir.magnitude)
        //        //    moveDist = dir.magnitude;
        //        transform.position += dir.normalized * moveDist;
        //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        //    }
        //}

        switch (State)
        {
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Moving:
                UpdateMove();
                break;
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Skill:
                UpdateSkill();
                break;
        }
    }

    public abstract void Init();
    protected virtual void UpdateDie() { }
    protected virtual void UpdateMove() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateSkill() { }
}
