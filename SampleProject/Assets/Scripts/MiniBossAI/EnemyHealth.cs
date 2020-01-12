using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyHealth : MonoBehaviour
{
    bool LogOnce;
    public bool Invinsible;

    public int Health, MaxHealth;
    private Animator Anim;
    int RandomDeath;

    public bool Boss,Dizzy,Regenerate;
}
