using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupController : MonoBehaviour
{
    public List<UnitController> _unitList;

    void Start()
    {
        
    }

    void Update()
    {
        for (int i = 0; i < _unitList.Count; i++)
        {
            _unitList[i].OnUpdate();
        }
    }
}
