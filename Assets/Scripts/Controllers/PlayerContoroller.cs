using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//1. 위치 벡터
//2. 방향 벡터(단위 벡터)

public class PlayerContoroller : BaseController
{
    PlayerStat _stat;
    //float wait_run_retio = 0.0f;
    
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    bool _stopSkill = false;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _stat = gameObject.GetComponent<PlayerStat>();

        //Managers.Input.KeyAction -= OnKeyboard;
        //Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateSkill()
    {
        if (_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;//방향벡터 생성
            Quaternion quat = Quaternion.LookRotation(dir);//쿼터니언 생성
            transform.rotation = Quaternion.Lerp(transform.rotation,quat, 20 * Time.deltaTime);
        }
    }

    protected override void UpdateIdle()
    {
        //animation

        #region blend animation
        //wait_run_retio = Mathf.Lerp(wait_run_retio, 0, 10.0f * Time.deltaTime);
        //anim.SetFloat("wait_run_ratio", wait_run_retio);
        //anim.Play("WAIT_RUN");
        #endregion
    }

    protected override void UpdateDie()
    {
        //아직 사용 안함
    }

    protected override void UpdateMove()
    {
        //몬스터가 내 사정거리보다 가까우면 공격
        if (_lockTarget != null)
        {
            float distance = (_lockTarget.transform.position - transform.position).magnitude;
            if (distance <= 1)
            {
                State = Define.State.Skill;
                return;
            }
        }

        //move
        Vector3 dir = _destPos - transform.position;//남은 거리
        dir.y = 0;

        if (dir.magnitude < 0.1f)
        {
            //_moveToDest = false;
            State = Define.State.Idle;
        }
        else
        {
            //if (moveDist >= dir.magnitude)
            //    moveDist = dir.magnitude;

            //transform.position += dir.normalized * moveDist;

            Debug.DrawRay(transform.position, dir.normalized, Color.red);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if (Input.GetMouseButton(0) == false)
                    State = Define.State.Idle;
                return;
            }
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        #region blend animation
        //Animator anim = GetComponent<Animator>();
        //wait_run_retio = Mathf.Lerp(wait_run_retio, 1, 10.0f * Time.deltaTime);
        //anim.SetFloat("wait_run_ratio", wait_run_retio);
        //anim.Play("WAIT_RUN");
        #endregion

    }


    void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            transform.position += (Vector3.forward * Time.deltaTime * _stat.MoveSpeed);
            //transform.Translate(Vector3.forward * Time.deltaTime *speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += (Vector3.back * Time.deltaTime * _stat.MoveSpeed);
            //transform.Translate(Vector3.back * Time.deltaTime *speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += (Vector3.left * Time.deltaTime * _stat.MoveSpeed);

            //transform.Translate(Vector3.left * Time.deltaTime *speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += (Vector3.right * Time.deltaTime * _stat.MoveSpeed);
            //transform.Translate(Vector3.right * Time.deltaTime *speed);
        }
        //_moveToDest = false;
    }

    //애니메이션에서 키를 직접 삽입하여 호출할 수 있다.
    //void OnRunEvent(int num)
    //{
    //    Debug.Log($"뚜벅뚜벅 {num}");
    //}

    void OnMouseEvent(Define.MouseEvent evt)
    {

        switch (State)
        {
            case Define.State.Die:
                break;
            case Define.State.Moving:
            case Define.State.Idle:
                {
                    OnMouseEvent_IdleRun(evt);
                }
                break;
            case Define.State.Skill:
                {
                    if (evt == Define.MouseEvent.PointerUp)
                        _stopSkill = true;
                }
                break;
            default:
                break;
        }

    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool isRaycastHit = Physics.Raycast(ray, out RaycastHit hit, 100.0f, _mask);

        //스크린상 위치한 마우스의 월드 레이를 가져온다.
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        switch (evt)
        {
            case Define.MouseEvent.Press:
                {
                    if (_lockTarget == null && isRaycastHit)
                        _destPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerDown:
                {
                    if (isRaycastHit)
                    {
                        _destPos = hit.point;
                        State = Define.State.Moving;
                        _stopSkill = false;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                            _lockTarget = hit.collider.gameObject;
                        else
                            _lockTarget = null;
                    }
                }
                break;
            case Define.MouseEvent.PointerUp:
                {
                    _stopSkill = true;
                }
                break;
            case Define.MouseEvent.Click:
                break;
            default:
                break;
        }
    }

    //애니메이션 이벤트 키
    public void OnHitEvent()
    {
        Debug.Log("OnHitEvent");
        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);
        }

        //Animator anim = GetComponent<Animator>();
        //anim.SetBool("Attack", false);
        if (_stopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Skill;
        }

    }
}