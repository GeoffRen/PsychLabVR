using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class audioDelay : MonoBehaviour
{
    AudioSource audio;

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        audio = GetComponent<AudioSource>();

        //starts 1 second prior to "Play on Awake" option in Audio Source
        //audio.PlayDelayed(-1);

        //This needs to be updated if scene name is changed
        switch (currentScene.name)
        {
            case ("Mark-2"):
            case ("Mark-2-Complete"):
            case ("WSAP"):
                audio.PlayDelayed(1);
                break;

            case ("Mark-3"):
            case ("Mark-3-Complete"):
                audio.PlayDelayed(2.3f);
                break;

            case ("Mark-4"):
            case ("Mark-4-Complete"):
                audio.PlayDelayed(0.7f);
                break;

            case ("Mark-5"):
            case ("Mark-5-Complete"):
                audio.PlayDelayed(1.3f);
                break;

            default:
                audio.Play();
                break;
        }
    }

    void Update()
    {

    }
}