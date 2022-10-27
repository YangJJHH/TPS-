using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items;
    public Transform playerTransform;
    
    private float lastSpawnTime;
    public float maxDistance = 5f;
    
    private float timeBetSpawn;

    public float timeBetSpawnMax = 7f;
    public float timeBetSpawnMin = 2f;

    private void Start()
    {
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0f;
    }

    private void Update()
    {
        if (Time.time >= lastSpawnTime + timeBetSpawn && playerTransform != null)
        {
            Spawn();
            lastSpawnTime = Time.time;
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        }
    }

    private void Spawn()
    {
        var spawPosition = Utility.GetRandomPointOnNavMesh(playerTransform.position,maxDistance,NavMesh.AllAreas);
        spawPosition += Vector3.up * 0.5f;
        var item = Instantiate(items[Random.Range(0,items.Length)],spawPosition,Quaternion.identity);
        Destroy(item,5f);
    }
}