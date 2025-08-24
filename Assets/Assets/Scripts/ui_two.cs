using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ui_two : MonoBehaviour
{
    [SerializeField] private AudioSource click;
    [SerializeField] private GameObject bgm;

    // Start is called before the first frame update
    void Start()
    {
        //bgm.GetComponent<MusicClass>().StopMusic();
        bgm.GetComponent<MusicClass>().PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GotoTitle() {
        click.Play();
        bgm.GetComponent<MusicClass>().StopMusic();
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
}
