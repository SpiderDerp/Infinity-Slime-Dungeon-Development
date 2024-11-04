using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ui : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;

    [SerializeField] private TMP_Text attacklevel;
    [SerializeField] private TMP_Text healthlevel;
    [SerializeField] private TMP_Text defenselevel;

    [SerializeField] private TMP_Text souls;
    [SerializeField] private TMP_Text souls2;
    [SerializeField] private TMP_Text attackupgradecost;
    [SerializeField] private TMP_Text healthupgradecost;
    [SerializeField] private TMP_Text defenseupgradecost;

    [SerializeField] private GameObject shopUI;

    [SerializeField] private GameObject WinPopup;

    [SerializeField] private AudioSource click;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Update UI
        if (player.GetComponent<player>().GetAttackLevel() >= 15) {
            attacklevel.text = "LVL MAX";
        } else {
            attacklevel.text = "LVL " + player.GetComponent<player>().GetAttackLevel().ToString();
        }
        if (player.GetComponent<player>().GetHealthLevel() >= 15) {
            healthlevel.text = "LVL MAX";
        } else {
            healthlevel.text = "LVL " + player.GetComponent<player>().GetHealthLevel().ToString();
        }
        if (player.GetComponent<player>().GetDefenseLevel() >= 10) {
            defenselevel.text = "LVL MAX";
        } else {
            defenselevel.text = "LVL " + player.GetComponent<player>().GetDefenseLevel().ToString();
        }
        
        souls.text = player.GetComponent<player>().GetSouls().ToString();
        souls2.text = player.GetComponent<player>().GetSouls().ToString();

        switch (player.GetComponent<player>().GetAttackLevel()) {
            case 0: case 1: case 2: case 3:
                attackupgradecost.text = "20";
                break;
            case 4: case 5: case 6:
                attackupgradecost.text = "30";
                break;
            case 7: case 8: case 9:
                attackupgradecost.text = "50";
                break;
            case 10: case 11: case 12:
                attackupgradecost.text = "70";
                break;
            case 13: case 14:
                attackupgradecost.text = "100";
                break;
            case 15:
                attackupgradecost.text = "-";
                break;
            default:
                attackupgradecost.text = "-";
                break;
        }

        switch (player.GetComponent<player>().GetHealthLevel()) {
            case 0: case 1: case 2: case 3:
                healthupgradecost.text = "30";
                break;
            case 4: case 5: case 6:
                healthupgradecost.text = "40";
                break;
            case 7: case 8: case 9:
                healthupgradecost.text = "60";
                break;
            case 10: case 11: case 12:
                healthupgradecost.text = "80";
                break;
            case 13: case 14:
                healthupgradecost.text = "120";
                break;
            case 15:
                healthupgradecost.text = "-";
                break;
            default:
                healthupgradecost.text = "-";
                break;
            
            
        }

        switch (player.GetComponent<player>().GetDefenseLevel()) {
            case 0: case 1: case 2: case 3:
                defenseupgradecost.text = "10";
                break;
            case 4: case 5: case 6:
                defenseupgradecost.text = "20";
                break;
            case 7: case 8: case 9:
                defenseupgradecost.text = "30";
                break;
            case 10:
                defenseupgradecost.text = "-";
                break;  
            default:
                defenseupgradecost.text = "-";
                break;
        }


        
    }

    public void UpgradeAttack() {
        int souls = player.GetComponent<player>().GetSouls();
        int cost = int.Parse(attackupgradecost.text);
        if (souls >= cost && player.GetComponent<player>().GetAttackLevel() < 15) {
            click.Play();
            player.GetComponent<player>().SetAttack(player.GetComponent<player>().GetAttackLevel() + 1);
            player.GetComponent<player>().SetSouls(souls - cost);
        }

    }

    public void UpgradeHealth() {
        int souls = player.GetComponent<player>().GetSouls();
        int cost = int.Parse(healthupgradecost.text);
        if (souls >= cost && player.GetComponent<player>().GetHealthLevel() < 15) {
            click.Play();
            player.GetComponent<player>().SetHealth(player.GetComponent<player>().GetHealthLevel() + 1);
            player.GetComponent<player>().SetSouls(souls - cost);
        }
    }

    public void UpgradeDefense() {
        int souls = player.GetComponent<player>().GetSouls();
        int cost = int.Parse(defenseupgradecost.text);
        if (souls >= cost && player.GetComponent<player>().GetDefenseLevel() < 10) {
            click.Play();
            player.GetComponent<player>().SetDefense(player.GetComponent<player>().GetDefenseLevel() + 1);
            player.GetComponent<player>().SetSouls(souls - cost);
        }
    }

    public void OpenShop() {
        // Open shop UI
        click.Play();
        StartCoroutine(SlideUpWinPopup());
        StartCoroutine(SlideDownShop());
    }


    private IEnumerator SlideUpWinPopup()
    {
        Vector3 startPosition = WinPopup.transform.position;
        Vector3 endPosition = new Vector3(0, 784, startPosition.z);
        float duration = 1.0f; // Duration of the slide up
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
    
    private IEnumerator SlideDownShop()
    {
        Vector3 startPosition = shopUI.transform.position;
        Vector3 endPosition = new Vector3(0, 0, startPosition.z);
        float duration = 1.0f; // Duration of the slide down
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = 1f - Mathf.Pow(2f, -10f * t); // Exponential decay
            shopUI.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        shopUI.transform.position = endPosition;
    }

    public void GotoTitle() {
        click.Play();
        //unload current scene
        SceneManager.LoadScene("Title");
    }

    public void NextRoom() {
        string scenename = SceneManager.GetActiveScene().name;
        click.Play();

        switch (scenename) {
            case "Title":
                SceneManager.LoadScene("Room1");
                //set all stats to 0 and save

                PlayerPrefs.SetInt("attack", 0);
                PlayerPrefs.SetInt("health", 0);
                PlayerPrefs.SetInt("defense", 0);
                PlayerPrefs.SetInt("souls", 0);
                break;
            case "Room1":
                SceneManager.LoadScene("Room2");
                break;
            case "Room2":
                SceneManager.LoadScene("Room3");
                break;
            case "Room3":
                SceneManager.LoadScene("Room4");
                break;
            case "Room4":
                SceneManager.LoadScene("Room5");
                break;
            case "Room5":
                SceneManager.LoadScene("Room6");
                break;
            case "Room6":
                SceneManager.LoadScene("Room7");
                break;
            case "Room7":
                SceneManager.LoadScene("Room8");
                break;
            case "Room8":
                SceneManager.LoadScene("Room9");
                break;
            case "Room9":
                SceneManager.LoadScene("Room10");
                break;
            case "Room10":
                SceneManager.LoadScene("Room11");
                break;
            case "Room11":
                SceneManager.LoadScene("Room12");
                break;
            case "Room12":
                SceneManager.LoadScene("Room13");
                break;
            case "Room13":
                SceneManager.LoadScene("Room14");
                break;
            case "Room14":
                SceneManager.LoadScene("Room15");
                break;
            case "Room15":
                SceneManager.LoadScene("Finale");
                break;

        }
    }

    public void ReturntoRoom1() {
        click.Play();
        //unload current scene
        SceneManager.LoadScene("Room1");
    }
}
