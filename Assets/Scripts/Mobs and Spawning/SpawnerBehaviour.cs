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
    public int m_MaxCreepsCount = 5;
    public float m_SpawningDelayInSeconds = 3.0f;
    //private IEnumerator m_SpawningCoroutine;
    private Dictionary<int, SpawnableCreep> m_AliveCreeps;
    private Stack<int> m_CreepIdTickets;
    private Random m_Random;
    // Start is called before the first frame update

    void Awake(){
        m_Random = new Random();
        m_CreepIdTickets = new Stack<int>();
        for (int i = 0; i < m_MaxCreepsCount; ++i) {
            m_CreepIdTickets.Push(i);
        }
    }

    void Start(){
        StartCoroutine(SpawnToFull());
    }

    // Update is called once per frame
    void Update(){}

    // on spawned creep death event handler - substract the number of spawned creeps
    void HandleCreepDeath(SpawnableCreep dyingCreep){
        print("removing creep from dict");
        m_AliveCreeps.Remove(dyingCreep.CreepId);
        m_CreepIdTickets.Push(dyingCreep.CreepId);
        --m_CurrentCreepCount;
    }

    // spawn creeps on coroutine after some time period
    private IEnumerator SpawnToFull(){
        // mf: maybe move it to Update?
        while (true){
            yield return new WaitForSeconds(m_SpawningDelayInSeconds);
            if (m_CurrentCreepCount <= m_MaxCreepsCount){
                
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
        GameObject newCreepGO = Instantiate(m_CreepPrefabs[creepReferenceIndex], m_SpawningPoint.position, Quaternion.identity);
        SpawnableCreep newCreep = newCreepGO.GetComponent<SpawnableCreep>();
        newCreep.m_OnCreepDeathEvent += HandleCreepDeath;
        newCreep.CreepId = m_CreepIdTickets.Pop();
        m_AliveCreeps.Add(newCreep.CreepId, newCreep);
        m_CurrentCreepCount++;

    }

}
