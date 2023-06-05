using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableCreep : MonoBehaviour
{
    // TODO(mf): Move health and resource like mana to other script component - responsible for abilities
    [SerializeField]
    private float m_Health;

    public delegate void OnCreepDeath(SpawnableCreep dyingCreep);
    public event OnCreepDeath m_OnCreepDeathEvent;
    [SerializeField] private int m_CreepId;
    public int CreepId { get => m_CreepId; set => m_CreepId = value; }

    private float m_NextHit;
    [SerializeField] bool m_IsAlive;

    public Color m_BaseColor;
    public GameObject m_CreepModel;
    private Renderer m_Renderer;

    private void Awake()
    {
        m_Health = 100.0f;
        m_NextHit = Time.time + 0.5f;
        m_IsAlive = true;
        m_Renderer = m_CreepModel.GetComponent<Renderer>();
        m_Renderer.material.color = m_BaseColor;
        Debug.Log($"Starting life with {m_Health} health");
    }

    void Start() { }

    // Update is called once per frame
    void Update() {
        if (m_IsAlive) {
            Heartbeat();
            m_Renderer.material.color = Color.Lerp(Color.gray, m_BaseColor, m_Health/100.0f);
        }
    }

    private void Heartbeat() {
        if (Time.time >= m_NextHit)
        {
            transform.position += new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
            m_Health -= 5.0f;
            m_NextHit += 0.1f;
        }
        
        if (m_Health <= 0.0f)
        {
            CreepDeath();
        }
    }

    private void CreepDeath(){
        m_IsAlive = false;
        m_OnCreepDeathEvent?.Invoke(this);
        Destroy(this.gameObject, 0.5f);
    }
}
