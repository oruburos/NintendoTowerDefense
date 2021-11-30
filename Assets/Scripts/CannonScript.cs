using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    public GameObject BulletPrototype;
    public float ShootingPeriod;
    public float Range;
    public float BulletSpeed;
    public float Damage;
    public float RotationSpeed;
    public List<GameObject> Enemies;

    private float timeToShoot = 0.0f;
    private List<string> enemyTags;

	void Start ()
	{
        // We find an enemy in range, if none is found, do nothing
	    enemyTags = Enemies.Select(e => e.tag).ToList();
        var enemy = EnemyManagerScript.Instance.GetEnemyInRange(transform.position, float.PositiveInfinity, enemyTags);
        if (enemy != null)
        {
            var angle = MathHelpers.Angle(enemy.transform.position - transform.position, transform.up);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    void FixedUpdate ()
    {
        //we need to refresh the list of enemies to not be stuck trying to kill something out of range
        var enemy = EnemyManagerScript.Instance.GetEnemyInRange(transform.position, Range, enemyTags);

        if (enemy != null)
        {
            TurnToEnemy(enemy.transform.position + enemy.transform.right * 32);

            if (timeToShoot < 0)
            {
                var bullet = Pool.Instance.ActivateObject(BulletPrototype.tag);
                bullet.transform.position = transform.position;
               
                
               bullet.transform.rotation = transform.rotation;

                var bulletScript = bullet.GetComponent<ShotScript>();
                bulletScript.Speed = BulletSpeed;
                bulletScript.Range = Range;
                bulletScript.Direction = transform.transform.up;
                bulletScript.Damage = Damage;
                bulletScript.Target = enemy;
                bulletScript.EnemyTags = enemyTags;
                bulletScript.Turret = transform;

                bullet.SetActive(true);

                timeToShoot = ShootingPeriod;

               
                return;
            }
        }
        else
        {
            var closestEnemy = EnemyManagerScript.Instance.GetClosestEnemyInRange(transform.position, Range*2, enemyTags);
            if (closestEnemy != null)
            {
                TurnToEnemy(closestEnemy.transform.position + closestEnemy.transform.right * 32);
            }
        }

    

        timeToShoot -= Time.deltaTime;
    }

    private void TurnToEnemy(Vector2 position)
    {
        var direction = position - (Vector2)transform.position;
        var angle = MathHelpers.Angle(direction, transform.up);
        transform.Rotate(0, 0, Mathf.Clamp(angle, -10, 10) * RotationSpeed * Time.deltaTime);
    }
}
