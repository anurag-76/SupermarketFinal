using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] npcPrefabs;
    [SerializeField] private int maxNPCs = 10;
    [SerializeField] private float spawnRadius = 40f;
    [SerializeField] private float despawnRadius = 60f;
    [SerializeField] private float spawnCheckInterval = 3f; // check every 3 seconds

    private Transform player;
    private List<GameObject> activeNPCs = new List<GameObject>();
    private float timer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Spawn initial batch
        for (int i = 0; i < maxNPCs; i++)
        {
            SpawnNPC();
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnCheckInterval)
        {
            timer = 0f;
            DespawnFarNPCs();
            SpawnMissingNPCs();
        }
    }

    void DespawnFarNPCs()
    {
        // Remove any null references first (in case NPC was destroyed elsewhere)
        activeNPCs.RemoveAll(npc => npc == null);

        List<GameObject> toRemove = new List<GameObject>();

        foreach (GameObject npc in activeNPCs)
        {
            float distance = Vector3.Distance(npc.transform.position, player.position);
            if (distance > despawnRadius)
            {
                toRemove.Add(npc);
            }
        }

        foreach (GameObject npc in toRemove)
        {
            activeNPCs.Remove(npc);
            Destroy(npc);
        }
    }

    void SpawnMissingNPCs()
    {
        // Spawn new NPCs until we reach maxNPCs again
        int missing = maxNPCs - activeNPCs.Count;
        for (int i = 0; i < missing; i++)
        {
            SpawnNPC();
        }
    }

    void SpawnNPC()
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += player.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, NavMesh.AllAreas))
        {
            int randomIndex = Random.Range(0, npcPrefabs.Length);
            GameObject npc = Instantiate(npcPrefabs[randomIndex], hit.position, Quaternion.identity);
            activeNPCs.Add(npc);
        }
    }
}