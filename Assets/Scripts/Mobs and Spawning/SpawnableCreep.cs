using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HitPoints))]
public class SpawnableCreep : MonoBehaviour
{
    public int m_Hit = 10;
    [SerializeField] private int m_CreepId;
    public int CreepId { get => m_CreepId; set => m_CreepId = value; }
    
    private float m_NextHit;

    public Color m_BaseColor;
    public GameObject m_CreepModel;
    private Renderer m_Renderer;
    private HitPoints m_CreepHP;

    private void Awake()
    {
        m_Renderer = m_CreepModel.GetComponent<Renderer>();
        m_CreepHP = gameObject.GetComponent<HitPoints>();
        m_CreepHP.OnHealthChanged += OnHealthChangeCB;
        m_CreepHP.OnDied += OnDeathCB;
        m_Renderer.material.color = m_BaseColor;
    }

    void Start() {
        m_NextHit = Time.time;
    }

    // Update is called once per frame
    void Update() {
        if (m_CreepHP.IsAlive)
        {
            Heartbeat();
        }
    }

    private void Heartbeat() {
        if (Time.time >= m_NextHit)
        {
            transform.position += new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
            m_CreepHP.ModifyHP(m_Hit, HPModifierT.PhysicalAttack, out int _, out bool _);
            m_NextHit += 0.5f;
        }
    }

    private void OnDestroy()
    {
        m_CreepHP.OnHealthChanged -= OnHealthChangeCB;
        m_CreepHP.OnDied -= OnDeathCB;
    }

    public void OnDeathCB(GameObject thisObject)
    {
        Destroy(thisObject, .5f);
    }

    public void OnHealthChangeCB(int currentHP, int previousHP) {
        float healthPercentage = (float)currentHP / m_CreepHP.MaximumHP;
        m_Renderer.material.color = Color.Lerp(Color.gray, m_BaseColor, healthPercentage);
    }
}
