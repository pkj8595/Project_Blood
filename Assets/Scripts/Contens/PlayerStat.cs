using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField] protected int _exp;
    [SerializeField] protected int _gold;

    public int Exp 
    {
        get => _exp; 
        set 
        {
            _exp = value;
            int level = Level;
            while (true)
            {
                if (Managers.Data.StatDict.TryGetValue(level + 1, out Data.Stat stat) == false)
                    break;
                if (_exp < stat.totalExp)
                    break;
                level++;
            }
            if ( level != Level)
            {
                Debug.Log($"Level Up {Level}");
                Level = level;
                SetStat(Level);
            }
        } 
    }
    public int Gold { get => _gold; set => _gold = value; }

    public void Start()
    {
        _level = 1;
        _exp = 0;
        _gold = 0;

        SetStat(_level);
    }

    private void SetStat(int level)
    {
        var dict = Managers.Data.StatDict;
        Data.Stat stat = dict[level];
        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
        _defense = 5;
        _moveSpeed = 5.0f;
    }

    protected override void OnDead(Stat attacker)
    {
        Debug.Log("Player Dead");
    }
}
