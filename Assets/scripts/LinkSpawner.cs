using UnityEngine;
using System.Collections;

public class LinkSpawner : MonoBehaviour
{
    public GameObject speedBoostPrefab;
    public BoxCollider spawnArea;
    public float minSpawnInterval = 5f;
    public float maxSpawnInterval = 15f;

    private GameObject currentBoost;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            if (currentBoost == null && spawnArea != null)
            {
                Vector3 spawnPosition = GetRandomPointInBox(spawnArea);
                currentBoost = Instantiate(speedBoostPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    Vector3 GetRandomPointInBox(BoxCollider box)
    {
        // World-space center
        Vector3 center = box.transform.TransformPoint(box.center);

        // World-space boyutlar (local scale ile çarpýlýyor)
        Vector3 size = Vector3.Scale(box.size, box.transform.lossyScale);

        float x = Random.Range(-size.x / 2f, size.x / 2f);
        float y = Random.Range(-size.y / 2f, size.y / 2f);
        float z = Random.Range(-size.z / 2f, size.z / 2f);

        // Local offset, world-space'e dönüþtürülüyor
        Vector3 localOffset = new Vector3(x, y, z);
        Vector3 worldOffset = box.transform.TransformDirection(localOffset);

        return center + worldOffset;
    }


    public void ClearBoost()
    {
        currentBoost = null;
    }
}
