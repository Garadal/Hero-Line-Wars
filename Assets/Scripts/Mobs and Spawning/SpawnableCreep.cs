using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableCreep : MonoBehaviour
{
    // TODO(mf): Move health and resource like mana to other script component - responsible for abilities
    [SerializeField]
    private float m_Health;
    [SerializeField]
    private float m_Mana;

    public delegate void OnCreepDeath(SpawnableCreep dyingCreep);
    public event OnCreepDeath m_OnCreepDeathEvent;
    [SerializeField]
    private int m_CreepId;
    public int CreepId { get => m_CreepId; set => m_CreepId = value; }

    private float m_NextHit;


    private void Awake()
    {
        m_Health = 100.0f;
        m_Mana = 20.0f;
        m_NextHit = 0.0f;
    }

    void Start() { }

    // Update is called once per frame
    void Update() {
        if (m_Health <= 0.0f){
            CreepDeath();
        }

        if (Time.time >= m_NextHit) {
            m_Health -= 5.0f;
            m_NextHit += 1.0f;
        }
    }
    private void CreepDeath(){
        m_OnCreepDeathEvent?.Invoke(this);
        Destroy(this, 0.5f);
    }
}
