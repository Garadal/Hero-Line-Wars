using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// TODO - probably seperate classes
// - buffs and debuffs
// - DoTs and HoTs
// - Physical Armor, Resistances and Weaknesses
// - add revivable property - maybe corps has been defiled and cannot be revived - only respawn then?


public enum HPModifierT {
    PhysicalAttack,
    MagicialAttack,
    Healing
} // after adding new entry REMEMBER TO EXTEND HasHealth.ChangeHealth and other appropriate methods 

public class HitPoints : MonoBehaviour
{
    public delegate void HPChanged(int currentHP, int previousHP);
    public event HPChanged OnHealthChanged;

    public delegate void Died(GameObject thisObject);
    public event Died OnDied;

    public delegate void Revived(GameObject thisObject);
    public event Revived OnRevived;

    [SerializeField]
    private bool m_IsAlive;
    public bool IsAlive {
        get => m_IsAlive;
        private set {
            if (!m_IsAlive && value) {
                // revive
                OnRevived?.Invoke(gameObject);
            }
            else if (m_IsAlive && !value) {
                // die
                OnDied?.Invoke(gameObject);
            }
            m_IsAlive = value;
        }
    }

    [SerializeField]
    private int m_MaximumHP = 100; // default value in case of someone being forgetful
    public int MaximumHP {
        get => m_MaximumHP;
        set => m_MaximumHP = value;
    }
    [SerializeField]
    private int m_CurrentHP;
    // Now all HP changes done through getter/setter public property
    // trigger the OnHealthChanged event. To silently change this value (for what reason?)
    // one needs to access private m_CurrentHP property
    public int CurrentHP {
        get => m_CurrentHP;
        set {
            int oldHP = m_CurrentHP;
            m_CurrentHP = Math.Clamp(value, 0, m_MaximumHP);
            OnHealthChanged?.Invoke(m_CurrentHP, oldHP);
        }
    }

    public void ModifyHP(int incomingChange, HPModifierT hpChangeMod, out int resultChange, out bool killingBlow) {
        // modify HP baes on incomingChange, hpChangeMod and returns resultChange that actually has been applied

        killingBlow = false;

        // Already dead targets cannot have their health modified
        if (!IsAlive) {
            // TODO(mf) Should I throw error here?
            resultChange = 0;
            return;
        }

        // TODO(mf) For now all incomming changes are put to non negative and negated accordingly to selected HealhChangeT healthKind
        // Maybe it should be changed later.
        int _acctualChange = Math.Abs(incomingChange);

        switch (hpChangeMod) {
            case HPModifierT.PhysicalAttack:
                _acctualChange *= -1;
                break;
            case HPModifierT.MagicialAttack:
                _acctualChange *= -1;
                break;
            case HPModifierT.Healing:
                break;
        }
        resultChange = CurrentHP;
        CurrentHP += _acctualChange;
        resultChange = CurrentHP - resultChange;  // this is done this way, because changes to health involve clamp [0, m_MaximumHP]

        if (CurrentHP == 0) {
            IsAlive = false;
            killingBlow = true;
        }
    }

    public bool InstantKill() {
        // TODO(mf) Add some checks if character is not immune by any means?
        CurrentHP = 0;
        return IsAlive = false;  // if instant kill operation was a success
    }

    public void Revive() {
        ReviveCommon();
        CurrentHP = m_MaximumHP;
    }

    public void Revive(float HPFrac) {
        ReviveCommon();
        CurrentHP = Math.Max((int)Math.Round(Math.Clamp(HPFrac, 0.0f, 1.0f) * m_MaximumHP), 1);
    }
    
    public void Revive(int withHP) {
        ReviveCommon();
        CurrentHP = withHP;
    }
    
    private void ReviveCommon() {
        if (!m_IsAlive) {
            IsAlive = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // This will not raise a health change event
        // change m_CurrentHP to CurrentHP if you would like to raise one
        m_CurrentHP = m_MaximumHP;
        m_IsAlive = true;
    }
}
