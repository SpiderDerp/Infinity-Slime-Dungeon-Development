using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy : MonoBehaviour
{
    [SerializeField] private int attack = 5;
    [SerializeField] private int health = 50;
    [SerializeField] private int defense = 0;

    [SerializeField] private int souls = 19;
    private Vector3 originalPosition;
    [SerializeField] private Transform player; // Assign the enemy transform in the inspector

    //[SerializeField] private Animator animator; // Assign the animator in the inspector
    [SerializeField] private float slideDuration = 0.3f; // Duration of the slide
    [SerializeField] private Camera mainCamera; // Assign the main camera in the inspector
    [SerializeField] private float shakeDuration = 0.2f; // Duration of the camera shake
    [SerializeField] private float shakeMagnitude = 0.1f; // Magnitude of the camera shake

    [SerializeField] private GameObject playerFloatingObject;

    [SerializeField] private GameObject slider;
    [SerializeField] private Renderer enemyRenderer;

    [SerializeField] private AudioSource hit2;

    // Start is called before the first frame update
    void Start()
    {
        slider.GetComponent<UnityEngine.UI.Slider>().maxValue = health;
        slider.GetComponent<UnityEngine.UI.Slider>().value = health;
        originalPosition = transform.position;     
    }

    // Update is called once per frame
    void Update()
    {   
        if (health < 0) {
            health = 0;
        }
        slider.GetComponent<UnityEngine.UI.Slider>().value = health;

        // Start fading coroutine if health is 0
        if (health == 0 && enemyRenderer.material.color.a > 0)
        {
            StartCoroutine(FadeAway());
        }
    }

    public IEnumerator Attack()
    {
        // Slide to enemy position
        yield return StartCoroutine(SlideToPosition(new Vector3(player.position.x - (0.3f * player.position.x), transform.position.y, player.position.z - 10)));

        // Play jump animation
        //animator.SetTrigger("attack");

        // Wait for the animation to finish
        //yield return new WaitForSeconds(0.7f * animator.GetCurrentAnimatorStateInfo(0).length);

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

        transform.position = targetPosition;
    }  
    private IEnumerator ShakeCameraAndFlashEnemy()
    {
        Vector3 originalCamPos = mainCamera.transform.position;
        float elapsedTime = 0;
        Renderer enemyRenderer = player.GetComponent<Renderer>();
        Color originalColor = enemyRenderer.material.color;

        hit2.Play();
        player.GetComponent<player>().DealDamage(attack);
        ShowDamage(attack.ToString());
        while (elapsedTime < shakeDuration)
        {
            float offsetX = Random.Range(-shakeMagnitude, shakeMagnitude);
            float offsetY = Random.Range(-shakeMagnitude, shakeMagnitude);
            mainCamera.transform.position = new Vector3(originalCamPos.x + offsetX, originalCamPos.y + offsetY, originalCamPos.z);

            // Flash enemy light red
            Color lightRed = new Color(1, 0, 0, 0.5f); // Adjust the alpha value to make it lighter
            enemyRenderer.material.color = lightRed;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset enemy color
        enemyRenderer.material.color = originalColor;
        mainCamera.transform.position = originalCamPos;
    }

    private IEnumerator FadeAway()
    {
        Color color = enemyRenderer.material.color;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime / 2; // Adjust the duration as needed
            enemyRenderer.material.color = color;
            yield return null;
        }
        gameObject.SetActive(false); // Optionally disable the game object after fading
    }

    public void DealDamage(int attack) {
        health -= (int) (attack * (1 - defense/(50+defense)));
    }
 
    void ShowDamage(string text) {
        if (playerFloatingObject) {
            GameObject prefab = Instantiate(playerFloatingObject, transform.position, Quaternion.identity);
            prefab.GetComponentInChildren<TextMesh>().text = text;
        }
    }

    public int GetHealth() {
        return health;
    }

    public int GetSouls() {
        return souls;
    }
}
