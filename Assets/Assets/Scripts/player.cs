using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    [SerializeField] private int attacklevel = 0;
    [SerializeField] private int healthlevel = 0;
    [SerializeField] private int defenselevel = 0;
    private int attack;
    private int health;
    private int defense;
    private int souls = 0;
    private Vector3 originalPosition;
    [SerializeField] private Transform enemy; // Assign the enemy transform in the inspector
    [SerializeField] private Animator animator; // Assign the animator in the inspector
    [SerializeField] private float slideDuration = 0.3f; // Duration of the slide
    [SerializeField] private Camera mainCamera; // Assign the main camera in the inspector
    [SerializeField] private float shakeDuration = 0.2f; // Duration of the camera shake
    [SerializeField] private float shakeMagnitude = 0.1f; // Magnitude of the camera shake

    [SerializeField] private GameObject enemyFloatingObject;

    [SerializeField] private GameObject slider;
    [SerializeField] private Renderer playerRenderer;

    [SerializeField] private AudioSource hit1;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        attack = 10 + 5 * attacklevel;
        health = 100 + 20 * healthlevel;
        defense = 5 + 3 * defenselevel;
        slider.GetComponent<Slider>().maxValue = health;
        slider.GetComponent<Slider>().value = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0) {
            health = 0;
        }
        //make slider value match health
        slider.GetComponent<Slider>().value = health;

        // Start fading coroutine if health is 0
        if (health == 0 && playerRenderer.material.color.a > 0)
        {
            StartCoroutine(FadeAway());
        }
    }

    void OnDisable() {
        PlayerPrefs.SetInt("attack", attacklevel);
        PlayerPrefs.SetInt("health", healthlevel);
        PlayerPrefs.SetInt("defense", defenselevel);
        PlayerPrefs.SetInt("souls", souls);
    }

    void OnEnable() {
        attacklevel = PlayerPrefs.GetInt("attack");
        healthlevel = PlayerPrefs.GetInt("health");
        defenselevel = PlayerPrefs.GetInt("defense");
        souls = PlayerPrefs.GetInt("souls");
    }

    public IEnumerator Attack()
    {
        // Slide to enemy position
        yield return StartCoroutine(SlideToPosition(new Vector3(enemy.position.x - (0.3f * enemy.position.x), transform.position.y, enemy.position.z - 10)));

        // Play jump animation
        animator.SetTrigger("attack");

        // Wait for the animation to finish
        yield return new WaitForSeconds(0.7f * animator.GetCurrentAnimatorStateInfo(0).length);

        // Shake the camera and flash the enemy
        yield return StartCoroutine(ShakeCameraAndFlashEnemy());

        // Slide back to original position
        yield return StartCoroutine(SlideToPosition(originalPosition));
    }

    private IEnumerator SlideToPosition(Vector3 targetPosition)
    {
        float elapsedTime = 0;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < slideDuration)
        {
            float t = 1 - Mathf.Exp(-5 * elapsedTime / slideDuration); // Exponential decay
            transform.position = new Vector3(
                Mathf.Lerp(startingPosition.x, targetPosition.x, t),
                startingPosition.y,
                Mathf.Lerp(startingPosition.z, targetPosition.z, t)
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(targetPosition.x, startingPosition.y, targetPosition.z);
    }

    private IEnumerator ShakeCameraAndFlashEnemy()
    {
        Vector3 originalCamPos = mainCamera.transform.position;
        float elapsedTime = 0;
        Renderer enemyRenderer = enemy.GetComponent<Renderer>();
        Color originalColor = enemyRenderer.material.color;

        hit1.Play();
        enemy.GetComponent<enemy>().DealDamage(attack);
        ShowDamage(attack.ToString());
        while (elapsedTime < shakeDuration)
        {
            float offsetX = Random.Range(-shakeMagnitude, shakeMagnitude);
            float offsetY = Random.Range(-shakeMagnitude, shakeMagnitude);
            mainCamera.transform.position = new Vector3(originalCamPos.x + offsetX, originalCamPos.y + offsetY, originalCamPos.z);

            // Flash enemy red
            enemyRenderer.material.color = Color.red;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset enemy color
        enemyRenderer.material.color = originalColor;
        mainCamera.transform.position = originalCamPos;
    }

    void ShowDamage(string text) {
        if (enemyFloatingObject) {
            GameObject prefab = Instantiate(enemyFloatingObject, transform.position, Quaternion.identity);
            prefab.GetComponentInChildren<TextMesh>().text = text;
        }
    }

    public void DealDamage(int attack) {
        health -= (int) (attack * (1 - defense/(100+defense)));
    }

    public void AddSouls(int amount) {
        souls += amount;
    }

    public int GetHealth() {
        return health;
    }

    public int GetSouls() {
        return souls;
    }

    public void SetSouls(int amount) {
        souls = amount;
    }

    public void SetHealth(int amount) {
        healthlevel = amount;
    }

    public void SetAttack(int amount) {
        attacklevel = amount;
    }

    public void SetDefense(int amount) {
        defenselevel = amount;
    }

    public int GetAttackLevel() {
        return attacklevel;
    }

    public int GetHealthLevel() {
        return healthlevel;
    }

    public int GetDefenseLevel() {
        return defenselevel;
    }


    private IEnumerator FadeAway()
    {
        Color color = playerRenderer.material.color;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime / 2; // Adjust the duration as needed
            playerRenderer.material.color = color;
            yield return null;
        }
        gameObject.SetActive(false); // Optionally disable the game object after fading
    }
}
