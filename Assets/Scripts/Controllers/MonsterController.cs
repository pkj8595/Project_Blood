using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    Stat _stat;
    [SerializeField] private float _scanRange = 10;
    [SerializeField] private float _attackRange = 2;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Monster;

        _stat = gameObject.GetComponent<Stat>();
        if(gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateIdle()
    {

        //TODO 매니저가 생기면 옮기자
        GameObject player = Managers.Game.GetPlayer();
        if (player == null)
            return;

        float distance = (player.transform.position - transform.position).magnitude;
        if (distance <= _scanRange)
        {
            _lockTarget = player;
            if (_lockTarget.GetComponent<Stat>().Hp > 0)
                State = Define.State.Moving;
        }
    }

    protected override void UpdateMove()
    {
        //몬스터가 내 사정거리보다 가까우면 공격
        if (_lockTarget != null )
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            _destPos = _lockTarget.transform.position;
            float distance = (_lockTarget.transform.position - transform.position).magnitude;
            if (distance <= _attackRange && targetStat.Hp > 0)
            {
                State = Define.State.Skill;
                NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
                nma.SetDestination(transform.position);
                return;
            }
        }

        //move
        Vector3 dir = _destPos - transform.position;//남은 거리
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            //목표지점으로 이동한다.
            nma.SetDestination(_destPos);
            nma.speed = _stat.MoveSpeed;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }

    protected override void UpdateSkill()
    {

        if (_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;//방향벡터 생성
            Quaternion quat = Quaternion.LookRotation(dir);//쿼터니언 생성
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    protected override void UpdateDie()
    {
    }

    void OnHitEvent()
    {
        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);

            if (targetStat.Hp > 0)
            {
                float distance = (_lockTarget.transform.position - transform.position).magnitude;
                if(distance <= _attackRange)
                    State = Define.State.Skill;
                else
                    State = Define.State.Moving;
            }
            else
            {
                State = Define.State.Idle;
            }
        }
        else
        {
            State = Define.State.Idle;
        }
    }

}
