using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SpawnerBehaviour : MonoBehaviour
{
    public Transform m_SpawningPoint;
    public List<GameObject> m_CreepPrefabs;
    private int m_CurrentCreepCount = 0;
    public int m_MaxCreepsCount;
    public float m_SpawningDelayInSeconds;
    //private IEnumerator m_SpawningCoroutine;
    [SerializeField] private Dictionary<int, SpawnableCreep> m_AliveCreeps;
    private Stack<int> m_CreepIdTickets;
    private Random m_Random;
    // Start is called before the first frame update

    void Awake(){
        m_Random = new Random();
        m_AliveCreeps = new Dictionary<int, SpawnableCreep>();
        m_CreepIdTickets = new Stack<int>();
        for (int i = 0; i < m_MaxCreepsCount; ++i) {
            m_CreepIdTickets.Push(i);
        }
        Debug.Log($"Finihsing Awake::SpawnerBehaviour with m_CreepIdTickets.Count = {this.m_CreepIdTickets.Count}");
    }

    void Start(){
        StartCoroutine(SpawnToFull());
    }

    // Update is called once per frame
    void Update(){}

    // on spawned creep death event handler - substract the number of spawned creeps
    void HandleCreepDeath(GameObject dyingCreepGO){
        print($"SpawnerBehaviour::HandleCreepDeath {gameObject.name}");
        HitPoints dyingCreepHP = dyingCreepGO.GetComponent<HitPoints>();
        dyingCreepHP.OnDied -= HandleCreepDeath;
        SpawnableCreep dyingCreep = dyingCreepGO.GetComponent<SpawnableCreep>();
        int releasedId = dyingCreep.CreepId;
        dyingCreep.name += "_Dead";
        m_AliveCreeps.Remove(releasedId);
        m_CreepIdTickets.Push(releasedId);
        --m_CurrentCreepCount;
    }

    // spawn creeps on coroutine after some time period
    private IEnumerator SpawnToFull(){
        // mf: maybe move it to Update?
        while (true){
            yield return new WaitForSeconds(m_SpawningDelayInSeconds);
            if (m_CreepIdTickets.Count > 0){
                try {
                    SpawnNewCreep(m_Random.Next(0, m_CreepPrefabs.Count));
                }
                catch (ArgumentException ae) {
                    Debug.LogException(ae);
                }
                catch (Exception e) {
                    Debug.LogException(e);
                    throw;
                }
            }
        }
    }

    private void SpawnNewCreep(int creepReferenceIndex){
        Debug.Log($"SpawnerBehaviour::SpawnNewCreep({creepReferenceIndex})");
        GameObject newCreepGO = Instantiate(m_CreepPrefabs[creepReferenceIndex], m_SpawningPoint.position + new Vector3(m_Random.Next(0,5), 0.0f, m_Random.Next(0,5)), Quaternion.identity);
        SpawnableCreep newCreep = newCreepGO.GetComponent<SpawnableCreep>();
        HitPoints creepHP = newCreepGO.GetComponent<HitPoints>();
        creepHP.OnDied += HandleCreepDeath;
        newCreep.CreepId = m_CreepIdTickets.Pop();
        newCreep.name = $"SpawnedCreep{newCreep.CreepId}"; 
        m_AliveCreeps.Add(newCreep.CreepId, newCreep);
        ++m_CurrentCreepCount;
    }

}
