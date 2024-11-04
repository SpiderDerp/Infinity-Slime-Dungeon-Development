using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class game : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject WinPopup;
    [SerializeField] private GameObject LosePopup;

    [SerializeField] private AudioSource click;

    [SerializeField] private AudioSource victory;
    [SerializeField] private AudioSource defeat;

    [SerializeField] private GameObject bgm;

    private bool gameStart = false;
    private bool playerTurn = true;

    // Start is called before the first frame update
    void Start()
    {
        startButton.interactable = true;
        //bgm.GetComponent<MusicClass>().PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart)
        {
            startButton.interactable = false;
        }
    }

    public void StartGame()
    {
        click.Play();
        playerTurn = true;
        gameStart = true;
        startButton.interactable = false;
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (gameStart)
        {
            int playerHealth = player.GetComponent<player>().GetHealth();
            int enemyHealth = enemy.GetComponent<enemy>().GetHealth();

            if (playerHealth <= 0 || enemyHealth <= 0)
            {
                //Debug.Log("Game Over");
                gameStart = false;
                //startButton.interactable = true;
                if (enemyHealth <= 0) {
                    WinGame();
                } else {
                    LoseGame();
                }
                yield break;
            }

            if (playerTurn)
            {
                yield return StartCoroutine(PlayerAttack());
            }
            else
            {
                yield return StartCoroutine(EnemyAttack());
            }

            playerTurn = !playerTurn;
        }
    }

    private IEnumerator PlayerAttack()
    {
        yield return player.GetComponent<player>().Attack();
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator EnemyAttack()
    {
        yield return enemy.GetComponent<enemy>().Attack();
        yield return new WaitForSeconds(0.1f);
    }

    public void WinGame()
    {
        player.GetComponent<player>().SetSouls(player.GetComponent<player>().GetSouls() + enemy.GetComponent<enemy>().GetSouls());  // Add enemy souls to player
        PlayerPrefs.SetInt("souls", player.GetComponent<player>().GetSouls());
        if (SceneManager.GetActiveScene().name == "Room15") {
            PlayerPrefs.SetInt("gameComplete", 1);
        }
        victory.Play();
        StartCoroutine(SlideDownWinPopup());
    }

    public void LoseGame()
    {
        player.GetComponent<player>().SetSouls(0);  // Lose Souls
        player.GetComponent<player>().SetAttack(0);  // Reset player attack level
        player.GetComponent<player>().SetDefense(0);  // Reset player defense level
        player.GetComponent<player>().SetHealth(0);  // Reset player health level
        PlayerPrefs.SetInt("attack", player.GetComponent<player>().GetAttackLevel());
        PlayerPrefs.SetInt("health", player.GetComponent<player>().GetHealthLevel());
        PlayerPrefs.SetInt("defense", player.GetComponent<player>().GetDefenseLevel());
        PlayerPrefs.SetInt("souls", player.GetComponent<player>().GetSouls());
        defeat.Play();
        StartCoroutine(SlideDownLosePopup());
    }

    private IEnumerator SlideDownWinPopup()
    {
        Vector3 startPosition = WinPopup.transform.position;
        Vector3 endPosition = new Vector3(0, 0, startPosition.z);
        float duration = 1.0f; // Duration of the slide down
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = 1f - Mathf.Pow(2f, -10f * t); // Exponential decay
            WinPopup.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        WinPopup.transform.position = endPosition;
    }

    private IEnumerator SlideDownLosePopup()
    {
        Vector3 startPosition = LosePopup.transform.position;
        Vector3 endPosition = new Vector3(0, 0, startPosition.z);
        float duration = 1.0f; // Duration of the slide down
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = 1f - Mathf.Pow(2f, -10f * t); // Exponential decay
            LosePopup.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        LosePopup.transform.position = endPosition;
    }
}
