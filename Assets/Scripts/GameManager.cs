using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManagerInstance;

    // Singleton Instance to manage the game
    public static GameManager Instance
    {
        get
        {
            if (gameManagerInstance == null)
                gameManagerInstance = FindObjectOfType<GameManager>();

            return gameManagerInstance;
        }
    }

    // current amount of lives and money, provided in the editor
    public static int MaxLives = 5;
    public int InitialMoney;

    //Two final states, victory or game over
    public GameObject VictoryText;
    public GameObject GameOverText;


    // economy of the game , each time that the player builds a tower the price increases for that particular type of tower in an amount *IncreasePrice
    public int initialLakituPrice;
    public int initialBrickTowerPrice;
    public int initialHammerTowerPrice;

    public int lakituIncreasePrice;
    public int brickTowerIncreasePrice;
    public int hammerTowerIncreasePrice;


    private int currentLakituPrice;
    private int currentBrickTowerPrice;
    private int currentHammerTowerPrice;

    public static int Lives;
    private int money;

    // different UI elements to make explicit the state of the game
    private LifeDrawer lifeDrawer;
    private WaveDrawer waveDrawer;
    private MoneyDrawer moneyDrawer;

    //private counter to know how to declare victory
    private int remainingEnemies;

    // Use this for initialization
    void Start()
    {
        money = InitialMoney;
        Lives = MaxLives;

        currentLakituPrice = initialLakituPrice;
        currentHammerTowerPrice = initialHammerTowerPrice;
        currentBrickTowerPrice = initialBrickTowerPrice;

        moneyDrawer = GetComponent<MoneyDrawer>();
        moneyDrawer.Draw(InitialMoney);

        lifeDrawer = GetComponent<LifeDrawer>();
        lifeDrawer.Draw(Lives);

        waveDrawer = GetComponent<WaveDrawer>();
        int waves = GetComponent<EnemyWaveScheduler>().EnemyWaves.Count;
        waveDrawer.DrawCurrentWave(1);
        waveDrawer.DrawMaxWave(waves);

        remainingEnemies = GetComponent<EnemyWaveScheduler>().EnemyWaves.Sum(w => w.Amount);
    }


    /// <summary>
    /// This method is called by Enemy script once that they touch the castle
    /// </summary>
    /// <param name="enemy"></param>
    public void EnemyEscaped(GameObject enemy)
    {

        Lives--;
        if (Lives <= 0)
        {
            lifeDrawer.Draw(0);
            
             Pool.Instance.ActivateObject("gameOver").SetActive(true);
            GameOver();
        }
        else
        {
            CameraShaker.Instance.Shake();
            // Debug.Log("Quitar vida");
            lifeDrawer.Draw(Lives);
            remainingEnemies--;
            if (remainingEnemies < 1)
            {
                Victory();
            }
            else 
            {

                Pool.Instance.ActivateObject("stompSoundEffect").SetActive(true);
            }

        }
    }


    /// <summary>
    /// Method called by the enemy once that the health of the current Mario goes to 0 or below
    /// </summary>
    /// <param name="enemy"></param>
    public void EnemyKilled(GameObject enemy)
    {
        remainingEnemies--;
        if (remainingEnemies < 1)
        {
            Victory();
        }
    }

    public int GetMoney()
    {
        return money;
    }


    /// <summary>
    /// In the future a UI element could announce the incoming wave, for this prototype we only update the current Wave in the UI
    /// </summary>
    /// <param name="wave"></param>
    public void AnnounceWave(int wave)
    {

        Pool.Instance.ActivateObject("stompSoundEffect").SetActive(true);
        waveDrawer.DrawCurrentWave(wave);

    }

    /// <summary>
    /// We add money to the game state and we announce it to the UI
    /// </summary>
    /// <param name="value"></param>
    public void AddMoney(int value)
    {
        money += value;
        moneyDrawer.Draw(money);
    }



    /// <summary>
    /// Here goes the logic to pay for the current tower that we want to build, as a second step we update the price for the next tower
    /// </summary>
    /// <param name="tower"></param>
    public void TurretBuilt(GameObject tower)
    {

        if (tower.CompareTag("lakituTower"))
        {
            money -= currentLakituPrice;
            currentLakituPrice += lakituIncreasePrice;
        }

        if (tower.CompareTag("hammerTower"))
        {
            money -= currentHammerTowerPrice;
            currentHammerTowerPrice += hammerTowerIncreasePrice;
        }
        if (tower.CompareTag("brickTower"))
        {
            //  Debug.Log("actualizando precio");
            money -= currentBrickTowerPrice;
            currentBrickTowerPrice += brickTowerIncreasePrice;

        }
        moneyDrawer.Draw(money);

        Pool.Instance.ActivateObject("stompSoundEffect").SetActive(true);
    }


    /// <summary>
    /// Method called everytime that we collect coins spreaded over the map.
    /// </summary>
    /// <param name="coin"></param>
    public void CoinCollected(GameObject coin)
    {
        money += CoinScript.Value;
        moneyDrawer.Draw(money);
    }


    /// <summary>
    /// Sanity check to now if we have enough money to build a tower
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public bool EnoughMoneyForTurret(string tag)
    {
        if (tag == "lakituTower")
        {
            return money >= currentLakituPrice;
        }
        if (tag == "hammerTower")
        {
            return money >= currentHammerTowerPrice;
        }
        if (tag == "brickTower")
        {
            return money >= currentBrickTowerPrice;
        }
        return false;
    }


    public int MoneyForTurret(string tag)
    {
        // Debug.Log(" Current tag" + tag);

        if (tag == "lakituTower")
        {
            return currentLakituPrice;
        }
        if (tag == "hammerTower")
        {
            return currentHammerTowerPrice;
        }
        //last option, brick tower
        if (tag == "brickTower")
        {
            return currentBrickTowerPrice;
        }
        // Debug.Log("No deberia nunca llegar a esta linea: " + tag);
        return -1;
    }



    /// <summary>
    /// When we declare victory, we activate the corresponding text (Victory) while forcint the game over text to remain inactive
    /// after 10 seconds we go back to the intro scene
    /// </summary>
    public void Victory()
    {


        VictoryText.SetActive(true);
        GameOverText.SetActive(false);
        Invoke("BackToMainMenu", 10.0f);
        Pool.Instance.ActivateObject("victorySoundEffect").SetActive(true);
    }



    /// <summary>
    /// When we declare game over, we activate the corresponding text (Game Over) while forcint the Victory text to remain inactive
    /// after 5 seconds we go back to the intro scene
    /// </summary>
    public void GameOver()
    {

        VictoryText.SetActive(false);
        GameOverText.SetActive(true);
        Pool.Instance.ActivateObject("gameOver").SetActive(true);
        Invoke("BackToMainMenu", 5.0f);
    }


    /// <summary>
    /// We return to the Intro Scene
    /// </summary>
    public void BackToMainMenu()
    {
        //Debug.Log("Volviendo a Back To Menu");
        SceneManager.LoadScene("Intro");
    }
}
