using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public delegate void AttackHandler(Collider other);
    public event AttackHandler weaponCollision;

    private void OnTriggerEnter(Collider other)
    {
        weaponCollision?.Invoke(other);
    }
}