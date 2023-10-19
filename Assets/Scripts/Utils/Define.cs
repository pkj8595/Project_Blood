using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster
    }

    public enum Layer
    {
        Monster = 8,
        Ground = 9,
        Block = 10,
    }

    public enum State
    {
        Die,
        Moving,
        Idle,
        Skill,
    }

    //�� �̸��� ����
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Sample,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        Count,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }


    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
    }

    public enum CameraMode
    {
        QuaterView
    }

}
