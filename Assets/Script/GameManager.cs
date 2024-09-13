using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    readonly int xBoundry = 11;
    readonly int zBoundry = 11;
    public int unservedCustomers = 0;
    public int energyDrinkCounter = 0;
    public float energyDrinkCooldownPeriod = 0f;
    public bool gameOver = false;
    public bool gameWin = false;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI energyDrinkCooldownText;
    public TextMeshProUGUI tutorial;
    public Text gameOverText;
    public Text gameWinText;
    public Text overdoseDeathText;
    public Text slapDeathText;
    public Button restartButton;
    public Button exitGameButton;
    public GameObject rosePicture;
    public GameObject winDialoguePicture;
    public GameObject energyDrink;
    public GameObject[] customers;
    public Player playerScript;

    void Start()
    {
        InvokeRepeating(nameof(SpawnCustomer), 1, Random.Range(4, 6));
        InvokeRepeating(nameof(SpawnEnergyDrink), 1, Random.Range(12, 17));
        energyDrinkCooldownPeriod = 60f;
    }

    void Update()
    {
        if (playerScript.isGameStarted)
            tutorial.gameObject.SetActive(false);

        if (energyDrinkCounter == 3)
            gameOver = true;

        if (gameOver)
        {
            restartButton.gameObject.SetActive(true);
            exitGameButton.gameObject.SetActive(true);
            gameOverText.gameObject.SetActive(true);

            if (energyDrinkCounter == 3)
                overdoseDeathText.gameObject.SetActive(true);
            else
                slapDeathText.gameObject.SetActive(true);
        }
        
        energyDrinkCooldownPeriod -= 1 * Time.deltaTime;

        scoreText.text = "Customers served: " + playerScript.customersServed;

        if (energyDrinkCounter != 0)
            energyDrinkCooldownText.text = "Energy Drink Cooldown: " + energyDrinkCooldownPeriod.ToString("0");

        if (playerScript.hasRose)
            rosePicture.gameObject.SetActive(true);

        if (playerScript.currentWinTime >= playerScript.winTimeCondition && !gameOver)
        {
            winDialoguePicture.SetActive(true);
            gameWin = true;
        }
    }

    public void WinScreen()
    {
        restartButton.gameObject.SetActive(true);
        exitGameButton.gameObject.SetActive(true);
        gameWinText.gameObject.SetActive(true);
    }

    void SpawnCustomer()
    {
        if (playerScript.isGameStarted && !gameOver && !gameWin)
        {
            GameObject customerPrefab = customers[Random.Range(1,9)];
            GameObject customer = Instantiate(customerPrefab, new Vector3(RandomXPosition(), 0.51f, RandomZPosition()), Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
            StartCoroutine(DespawnCustomer(customer));
            unservedCustomers++;
        }
    }
    void SpawnEnergyDrink()
    {
        if (playerScript.isGameStarted && !gameOver && !gameWin)
        {
            GameObject drink = Instantiate(energyDrink, new Vector3(RandomXPosition(), 0.7f, RandomZPosition()), energyDrink.transform.rotation);
            StartCoroutine(DespawnEnergyDrink(drink));
        }
    }

    float RandomXPosition()
    {
        return Random.Range(-xBoundry, xBoundry);
    }
    float RandomZPosition()
    {
        return Random.Range(-zBoundry, zBoundry);
    }

    IEnumerator DespawnCustomer(GameObject customer)
    {
        yield return new WaitForSeconds(10);
        Destroy(customer);
        unservedCustomers--;
    }
    IEnumerator DespawnEnergyDrink(GameObject energyDrink)
    {
        yield return new WaitForSeconds(15);
        Destroy(energyDrink);
    }

    public IEnumerator DeathEnergyDrinkOverdose()
    {
        yield return new WaitForSeconds(60);
        energyDrinkCounter = 0;
        playerScript.hadEnergyDrink = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
