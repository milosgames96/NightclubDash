using System.Collections;
using UnityEngine;

public class GirlScript : MonoBehaviour
{
    public Animator girlAnimator;
    private GameManager gameManagerScript;
    public GameObject aura;
    public Aura auraScript;
    public ClubSound clubSound;
    public float speed = 1;
    public float girlSize = 1f;
    readonly float enrageIncrement = 1.15f;
    public float currentEnrageTime = 0f;
    public float requiredEnrageTime = 50f;
    public int EnrageStack { get; set; }

    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        currentEnrageTime = 0f;
        EnrageStack = 0;
    }

    void Update()
    {
        FallowPlayer();

        aura.transform.position = transform.position - new Vector3(0, 0.47f, 0);

        if (currentEnrageTime >= requiredEnrageTime)
        {
            Enrage();
            auraScript.Enrage();
            currentEnrageTime = 0f;
        }

        currentEnrageTime += 1 * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rose"))
        {
            DeEnrage();
            auraScript.DeEnrage();
            Destroy(other.gameObject);
        }
    }
    private void FallowPlayer()
    {
        if (!gameManagerScript.gameOver)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.LookAt(GameObject.Find("Player").transform);
        }
    }

    void Enrage()
    {
        transform.localScale *= enrageIncrement;
        ++EnrageStack;
        girlAnimator.SetBool("isYelling", true);
        StartCoroutine(FinishYelling());
    }

    void DeEnrage()
    {
        transform.localScale /= enrageIncrement;
        --EnrageStack;
        currentEnrageTime = 0f;
    }

    IEnumerator FinishYelling()
    {
        yield return new WaitForSeconds(2);
        girlAnimator.SetBool("isYelling", false);
    }
}

