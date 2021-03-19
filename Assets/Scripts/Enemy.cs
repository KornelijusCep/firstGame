using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy", order = 1)]
public class Enemy : ScriptableObject
{
    public string name;

    public int health;
    public int resistance;
    public int attack;

    public int _XPdrop;
    
}
