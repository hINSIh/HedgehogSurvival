using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {

    [Header("Other")]
    public int healthKitProbability;
    public bool gameSituation = true;

    [Header("Items")]
    public HealthKit healthKit;
    public EnergyKit energyKit;

    public float _spawnDelay;

    [Header("Random Spawn")]
    public float spawnViewportMargin;

    [Header("Player")]
    public Transform player;
    public PlayerHealth playerHealth;
    public PlayerEnergy playerEnergy;

    [Header("Map")]
    public SpriteRenderer background;
    public FadeOutLoadScene fadeOutLoadScene;

    private Transform itemStorage;
    // Use this for initialization
    void Start()
    {
        itemStorage = new GameObject("itemStorage").transform;
        StartCoroutine(StartItemSpawn());
    }
	
	// Update is called once per frame
	void Update () {
	}

    private IEnumerator StartItemSpawn()
    {
        WaitForSeconds spawnDelay = new WaitForSeconds(_spawnDelay);

        while(gameSituation)
        {
            yield return spawnDelay;
            int randomSpawn = Random.Range(0, healthKitProbability);

            if(randomSpawn == 0)
            {

                HealthKit healthKitObject = Instantiate(healthKit.gameObject).GetComponent<HealthKit>();
                healthKitObject.name = "HealthKit";
                healthKitObject.transform.position = GetRandomSpawnPoint();
                healthKitObject.transform.SetParent(itemStorage);

                healthKitObject.playerHealth = playerHealth;
            }
            else
            {
                EnergyKit enrgyKitObject = Instantiate(energyKit.gameObject).GetComponent<EnergyKit>();
                enrgyKitObject.name = "EnergyKit";
                enrgyKitObject.transform.position = GetRandomSpawnPoint();
                enrgyKitObject.transform.SetParent(itemStorage);

                enrgyKitObject.playerEnergy = playerEnergy;
            }          
        }
    }
    #region Random point

    private Vector3 GetRandomSpawnPoint()
    {
        Vector3 result;
        float x, y;
        float randomAxis = GetRandomAxis();
        float randomPosition = GetRandomPosition();

        randomAxis += 0.5f;
        randomPosition += 0.5f;

        RandomInit(randomAxis, randomPosition, out x, out y);

        result = Camera.main.ViewportToWorldPoint(new Vector2(x, y));
        result.z = 0;
        return result;
    }

    private void RandomInit(float value1, float value2, out float a, out float b)
    {
        if (Random.Range(0, 2) == 0)
        {
            float temp = value1;
            value1 = value2;
            value2 = temp;
        }

        a = value1;
        b = value2;
    }

    private float GetRandomAxis()
    {
        float result = spawnViewportMargin;
        if (Random.Range(0, 2) == 0)
        {
            result *= -1;
        }

        return result;
    }

    private float GetRandomPosition()
    {
        float result = Random.Range(0, spawnViewportMargin * 2) - spawnViewportMargin;
        return result;
    }
    #endregion
}
