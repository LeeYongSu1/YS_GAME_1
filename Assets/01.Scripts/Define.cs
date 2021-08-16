using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
   public enum State
    {
        Idle,
        Move,
        Attack,
        Skill,
        Jump,
        Die
    }

    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
        Idle
    }

    public enum Layer
    {
        Monster = 10,
        Ground = 11,
        Block = 12,
    }

    public enum WeaponType
    {

        Melee,
        Range
    }

    public enum CretureType
    {
        Unknown,
        Player,
        Enemy
    }

    public enum Scene
    {
        Main,
        Load,
        Game,
    }

    public enum Gizmos
    {
        Normal, WayPoint
    }
}
