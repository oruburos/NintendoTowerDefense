using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public float MaxHealth;
    public int Money;
    public GameObject Coin;
    private Transform canvas;
    private Slider healthBar;
    public float health;

    private void OnEnable()
    {
        canvas = transform.Find("Canvas");
        healthBar = canvas.Find("HealthBar").GetComponent<Slider>();
        canvas.gameObject.SetActive(false);

        health = MaxHealth;
        healthBar.maxValue = MaxHealth;
        healthBar.value = health;
    }

    private void Update()
    {
        canvas.rotation = Quaternion.identity;
        canvas.localScale = Vector3.one * 0.5f;
    }

    /// <summary>
    /// Every time that we kill one Mario, this enemy can (randomly) spawn some coins ( from 0 to 3 ),
    /// This amount is an added extra,  the current Money that the enemy was carrying is added at the end
    /// </summary>
    private void SpawnCoins()
    {
       
        int  numCoins = Random.Range(0, 4);

        for (int i = 0; i < numCoins; i++)
        {
           
            float x = i  * 5.0f;
            float y = i * 5.0f;

            var coin = Pool.Instance.ActivateObject(Coin.tag);
            coin.transform.position = transform.position + new Vector3(x, y, 0);
            coin.SetActive(true);
        }

        GameManager.Instance.AddMoney(Money);
    }



    /// <summary>
    /// 
    /// Basic rules: kind of rock paper scissors
    /// turtle shells are able to damage any enemy
    /// hammers can't reach the flying Marios
    /// bricks can't reach the normal Marios.
    /// if any enemy enters into the castle we consider that that Mario escaped and we go to the Game Manager to check the state of the game and see what proceeds now
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!gameObject.activeSelf)
        {
            return;
        }
        if (collision.CompareTag("finish"))
        {
            GameManager.Instance.EnemyEscaped(gameObject);
        }

        else if (collision.CompareTag("shellprojectile")
            || (collision.CompareTag("brickprojectile") && CompareTag("marioheavy") || (collision.CompareTag("brickprojectile") && CompareTag("mariofly") ||
            (collision.CompareTag("hammerprojectile") && !CompareTag("mariofly")))))
        {
            var shot = collision.gameObject.GetComponent<ShotScript>();
            var damage = shot.Damage;
            health -= damage;
            healthBar.value = health;
            canvas.gameObject.SetActive(true);
            shot.BlowUp();

            if (health <= 0)
            {
                SpawnCoins();
                GameManager.Instance.EnemyKilled(gameObject);
                Pool.Instance.DeactivateObject(gameObject);
                EnemyManagerScript.Instance.DeleteEnemy(gameObject);
            }
        }
    }

    /// <summary>
    ///  After passing through the Castle we deactivate the enemy and returnit to the pool
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "finish")
        {
            EnemyManagerScript.Instance.DeleteEnemy(gameObject);
            Pool.Instance.DeactivateObject(gameObject);
        }

    }
}
