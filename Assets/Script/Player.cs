using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Animator playerAnimator;
    public Animator girlAnimator;
    private AudioSource playerAudioSource;
    public AudioClip fastPouringSound;
    public AudioClip slowPouringSound;
    private NavMeshAgent playerNavMeshAgent;
    public LayerMask whatCanBeClickedOn;
    public GameManager gameManagerScript;
    public GameObject customer;
    public GameObject energyDrink;
    public GameObject girl;
    public GameObject girlAura;
    public GameObject door;
    public GameObject rose;
    public GameObject WURST;
    [SerializeField] private GameObject castBar;
    [SerializeField] private CastBar[] CastBars;
    [SerializeField] private Image castBarImage;
    [SerializeField] private Text castBarTimer;
    public int customersServed = 0;
    private float drinkPouringCastTime = 2;
    public float currentWinTime = 0f;
    public float winTimeCondition = 600f;
    public bool hasRose = false;
    private bool resetRose = true;
    private bool isNearCustomer = false;
    private bool canMove = true;
    public bool isGameStarted = false;
    public bool hadEnergyDrink = true;
    private bool hasEnergyDrink = false;
    private bool areDoorClosed;
    private bool isPouringDrink = false;

    void Start()
    {
        playerNavMeshAgent = GetComponent<NavMeshAgent>();
        playerAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        SetSpeedBasedOnEnergyDrink();
        PlayerMovement();

        if (customersServed == 0)
            hasRose = false;
        else
        {
            if (customersServed % 10 == 0 && resetRose)
            {
                hasRose = true;
                resetRose = false;
            }
        }

        ServeCustomer();
        GiveRose();
        CloseDoor();

        if (areDoorClosed)
            currentWinTime += 1 * Time.deltaTime;


        if (hasEnergyDrink)
            playerAnimator.SetBool("hasEnergyDrink", true);
        else
            playerAnimator.SetBool("hasEnergyDrink", false);

        GameWinScreen();
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Customer"))
        {
            isNearCustomer = true;
            customer = other.gameObject;
        }

        if (other.CompareTag("EnergyDrink"))
        {
            energyDrink = other.gameObject;
            Destroy(energyDrink);
            drinkPouringCastTime = 0.5f;
            hasEnergyDrink = true;
            gameManagerScript.energyDrinkCounter++;

            if (gameManagerScript.energyDrinkCounter == 1)
                gameManagerScript.energyDrinkCooldownPeriod = 60f;
            
            if (hadEnergyDrink)
            {
                hadEnergyDrink = false;
                StartCoroutine(gameManagerScript.DeathEnergyDrinkOverdose());
            }

            StartCoroutine(EnergyDrinkComeDown());
        }

        if (other.CompareTag("Girl"))
        {
            gameManagerScript.gameOver = true;
            playerAnimator.SetBool("isDead", true);
            girlAnimator.SetBool("isGameOver", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TriggerStart"))
        {
            isGameStarted = true;
            other.enabled = false;
            areDoorClosed = true;
            StartCoroutine(GirlSpawn());
        }

        if (other.CompareTag("Customer"))
        {
            isNearCustomer = false;
            customer = null;
        }

        if (other.CompareTag("EnergyDrink"))
            energyDrink = null;

        if (other.CompareTag("TriggerEnd") && gameManagerScript.gameWin == true)
            gameManagerScript.WinScreen();
    }

    private void SetSpeedBasedOnEnergyDrink()
    {
        if (hasEnergyDrink)
            playerNavMeshAgent.speed = 5;
        else
            playerNavMeshAgent.speed = 3.5f;
    }

    private void PlayerMovement()
    {
        if (Input.GetMouseButton(0) && canMove)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            playerAnimator.SetBool("isWalking", true);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, whatCanBeClickedOn))
            {
                if (!gameManagerScript.gameOver)
                {
                    playerNavMeshAgent.SetDestination(hitInfo.point);
                }
            }
        }
        else
        {
            playerAnimator.SetBool("isWalking", false);
        }
    }

    private void GiveRose()
    {
        if (Input.GetKeyDown(KeyCode.R) && hasRose)
        {
            Instantiate(rose, gameObject.transform.position, rose.transform.rotation);
            hasRose = false;
            gameManagerScript.rosePicture.gameObject.SetActive(false);

        }
    }

    private void ServeCustomer()
    {
        if (Input.GetKeyDown(KeyCode.E) && isNearCustomer)
        {
            if (!isPouringDrink)
            {
                isPouringDrink = true;
                StartCoroutine(ServeCustomerCorutine(drinkPouringCastTime, customer));
            }
            canMove = false;
            playerNavMeshAgent.isStopped = true;
        }
    }

    IEnumerator ServeCustomerCorutine(float pouringTime, GameObject customer)
    {
        if (hasEnergyDrink)
            PouringDrink(pouringTime, fastPouringSound);
        else
            PouringDrink(pouringTime, slowPouringSound);

        castBar.SetActive(true);

        yield return new WaitForSeconds(pouringTime);

        Destroy(customer);
        gameManagerScript.unservedCustomers--;
        customersServed++;
        canMove = true;
        isPouringDrink = false;
        isNearCustomer = false;
        playerNavMeshAgent.isStopped = false;

        castBar.SetActive(false);
    }

    private void PouringDrink(float pouringTime, AudioClip pouringSound)
    {
        StartCoroutine(CastProgress(pouringTime));
        playerAudioSource.PlayOneShot(pouringSound);
    }

    IEnumerator CastProgress(float pouringTime)
    {
        float timePassed = Time.deltaTime;
        float rate = 1.0f / pouringTime;
        float progress = 0.0f;

        while (progress <= 1.0)
        {
            castBarImage.fillAmount = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;

            castBarTimer.text = (pouringTime - timePassed).ToString("F1");

            yield return null;
        }
    }

    IEnumerator EnergyDrinkComeDown()
    {
        yield return new WaitForSeconds(10);
        hasEnergyDrink = false;
        drinkPouringCastTime = 2;
    }

    IEnumerator GirlSpawn()
    {
        yield return new WaitForSeconds(5);
        girl.SetActive(true);
        girlAura.SetActive(true);
    }
    
    private void CloseDoor()
    {
        if (areDoorClosed && door.transform.position.x < 0.75f)
        {
            Vector3 doorsEndPosition = new Vector3(0.153f, 0, -2.875f);
            door.transform.Translate(-doorsEndPosition * Time.deltaTime);
        }
    }

    private void GameWinScreen()
    {
        if (currentWinTime >= winTimeCondition && !gameManagerScript.gameOver)
        {
            girl.SetActive(false);
            girlAura.SetActive(false);
            door.SetActive(false);
            WURST.SetActive(true);
        }
    }
}


