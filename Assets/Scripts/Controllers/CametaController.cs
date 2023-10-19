using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CametaController : MonoBehaviour
{
    [SerializeField] private Define.CameraMode _mode = Define.CameraMode.QuaterView;
    [SerializeField] private Vector3 _delta = new Vector3(0.0f, 6.0f, -3.0f);
    [SerializeField] private GameObject _player;

    public void SetPlayer(GameObject player)
    {
        _player = player;
    }

    void LateUpdate()
    {
        if(_mode == Define.CameraMode.QuaterView)
        {
            if (_player.IsValid() == false)
            {
                return;
            }

            //레이캐스트 (시작위치, 카메라가 있는 방향, hit, 카메라 방향으로의 일직선 크기
            if(Physics.Raycast(_player.transform.position, _delta, out RaycastHit hit, _delta.magnitude, 1 << (int)Define.Layer.Block))
            {
                //캐릭터와 카메라가 위치하는 고정위치 사이에 wall 객체가 감지되면 카메라의 위치를 그 중간으로 이동시킨다.
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
                transform.position = _player.transform.position + _delta.normalized * dist;
            }
            else
            {
                transform.position = _player.transform.position + _delta;
                transform.LookAt(_player.transform);
            }

        }

    }

    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuaterView;
        _delta = delta;

    }
}
