using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour
{
    private static EnemyManagerScript enemyManagerInstance;
    public static EnemyManagerScript Instance
    {
        get
        {
            if (enemyManagerInstance == null)
                enemyManagerInstance = FindObjectOfType<EnemyManagerScript>();

            return enemyManagerInstance;
        }
    }

    private class EnemyDistancePair
    {
        public GameObject Enemy;
        public float Distance;

        public EnemyDistancePair(GameObject enemy, float distance)
        {
            Enemy = enemy;
            Distance = distance;
        }
    }

    private Dictionary<GameObject, EnemyDistancePair> enemies = new Dictionary<GameObject, EnemyDistancePair>();

    public void RegisterEnemy(GameObject enemy)
    {
        enemies.Add(enemy, new EnemyDistancePair(enemy, 0));
    }

    public void DeleteEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public void UpdateEnemy(GameObject enemy, float distance)
    {
        enemies[enemy].Distance = distance;
    }
    /// <summary>
    /// We make use of LINQ facilities to find the list of enemies that are included both in range from a position, and with similar tags
    /// As a note aside, check that when comparing distances the code is making use of range * range and sqr magnitude, given that rooting numbers is expensive
    /// </summary>
    /// <param name="position"></param>
    /// <param name="range"></param>
    /// <param name="enemyTags"></param>
    /// <returns></returns>
    public GameObject GetEnemyInRange(Vector2 position, float range, IEnumerable<string> enemyTags)
    {
        return enemies.Values
            .Where(e => ((Vector2)e.Enemy.transform.position - position).sqrMagnitude < range * range && enemyTags.Any(t => e.Enemy.CompareTag(t)))
            .OrderBy(e => e.Distance)
            .Select(e => e.Enemy)
            .FirstOrDefault();
    }


    /// <summary>
    /// Similar method to the one above, but this time we are selecting the closest of all the enemies
    /// </summary>
    /// <param name="position"></param>
    /// <param name="range"></param>
    /// <param name="enemyTags"></param>
    /// <returns></returns>
    public GameObject GetClosestEnemyInRange(Vector2 position, float range, IEnumerable<string> enemyTags)
    {
        return enemies.Values
            .Where(e => ((Vector2)e.Enemy.transform.position - position).sqrMagnitude < range * range && enemyTags.Any(t => e.Enemy.CompareTag(t)))
            .OrderBy(e => ((Vector2)e.Enemy.transform.position - position).sqrMagnitude)
            .Select(e => e.Enemy)
            .FirstOrDefault();
    }
}
