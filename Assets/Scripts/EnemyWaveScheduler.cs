using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// KEY Behaviour:
/// This is 
/// </summary>
public class EnemyWaveScheduler : MonoBehaviour
{

    /// <summary>
    /// Currently the waves only deal with one type of enemy
    /// So each wave is composed by the Amount of units to create
    /// what kind of Enemy to create
    /// the time to wait before spawning each enemy
    /// and finally, the rest time to allow the player to perceive the different waves and mark the pace of the gameplay
    /// </summary>
    [System.Serializable]
    public class EnemyWave
    {
        public int Amount; 
        public GameObject Enemy;
        public float SpawnTime;
        public float RestTime;
    }

    public List<EnemyWave> EnemyWaves;
    private int waveIndex = 0;
    private EnemyWave currentWave;
    private float spawnTime = 2.0f;

    // Use this for initialization
    void OnEnable()
    {
        currentWave = EnemyWaves[0];
        //Debug.Log("Start");
        //Debug.Log("Numero de olas " + Waves.Count);
    }



    /// <summary>
    /// Basically, the core of the Gameplay
    /// First, checks if the Rest time is depleted, then updates the current Enemy Wave, for each amount of enemies, the scheduler activates one enemy after the Spawn Time is depleted, 
    /// Once that the Amount of enemies is already deployed, the scheduler moves to Rest  before repeating all the Loop until no more enemy rounds are availables
    /// 
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        if (waveIndex >= EnemyWaves.Count)
        {
            return;
        }

        if (currentWave.RestTime < 0)
        {
            waveIndex += 1;
            if (waveIndex >= EnemyWaves.Count)
            {
                return;
            }
            else
            {
                // Debug.Log("Nuva Ola");
                GameManager.Instance.AnnounceWave(waveIndex + 1);
                currentWave = EnemyWaves[waveIndex];
                return;
            }
        }

        if (currentWave.Amount <= 0)
        {
            currentWave.RestTime -= Time.deltaTime;
            return;
        }

        if (spawnTime < 0)
        {
            Spawn(currentWave.Enemy);

            spawnTime = currentWave.SpawnTime;
            currentWave.Amount--;
            return;
        }

        spawnTime -= Time.deltaTime;
    }

    private void Spawn(GameObject prototype)
    {
        //Debug.Log("Spawn");
        var spawnedEnemy = Pool.Instance.ActivateObject(prototype.tag);
        spawnedEnemy.SetActive(true);
        EnemyManagerScript.Instance.RegisterEnemy(spawnedEnemy);
    }
}

