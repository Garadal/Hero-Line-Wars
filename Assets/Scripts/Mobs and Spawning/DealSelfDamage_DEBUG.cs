using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO(mf) maybe remove it and force error when there is no HP component attached to gameObject
[RequireComponent(typeof(HitPoints))]
public class DealSelfDamage_DEBUG : MonoBehaviour
{
    public HitPoints m_MyHp;
    public int m_MeeleDamage = 10;
    public int m_MagicalDamage = 10;
    public int m_Healing = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        if (m_MyHp == null)
            m_MyHp = gameObject.GetComponent<HitPoints>();

        m_MyHp.OnDied += DiedCB;
        m_MyHp.OnHealthChanged += HealthChangedCB;
        m_MyHp.OnRevived += RevivedCB;
    }

    public void HealthChangedCB(int currentHP, int previousHP) {
        Debug.Log($"HP changed from {previousHP} to {currentHP}.");
    }

    public void RevivedCB(GameObject revieved) {
        Debug.Log($"Revived has been {revieved.name}!");
    }

    public void DiedCB(GameObject died) {
        Debug.Log($"{died.name} just died!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("--- Q key pressed - dealing physical attack to self.");
            Attack(m_MeeleDamage, HPModifierT.PhysicalAttack);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("--- W key pressed - dealing magical attack to self.");
            Attack(m_MagicalDamage, HPModifierT.MagicialAttack);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("--- E key pressed - healing self.");
            Heal(m_Healing);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("--- K key presed - Instant kill myself");
            m_MyHp.InstantKill();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("--- R key pressed - Revive myself to full");
            m_MyHp.Revive();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("--- R key pressed - Revive myself to 55 HP");
            m_MyHp.Revive(55);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("--- R key pressed - Revive myself to 33% health");
            m_MyHp.Revive((float)33/100);
        }
    }
    private void Attack(int amount, HPModifierT mod) { 
        m_MyHp.ModifyHP(amount, mod, out int effect, out bool killingBlow);
        Debug.Log($"Attack dealt {effect} {mod} Damage (out of {amount})");
        if (killingBlow) {
            Debug.Log($"This attack was a Killing Blow!");
        }
    }
    private void Heal(int amount) {
        m_MyHp.ModifyHP(amount, HPModifierT.Healing, out int effect, out bool _);
        Debug.Log($"Healing target for {effect} HP (out of {amount}, overheal {amount - effect} HP)");
    }
}
