using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuideAudioDelay : MonoBehaviour
{
    AudioSource audio;

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        audio = GetComponent<AudioSource>();
//        Debug.Log(SceneManager.GetActiveScene().name);

        if (currentScene.name == "Basic - LS")
        {
            audio.PlayDelayed(2.4f);
        }
    }
        // Update is called once per frame
        void Update () {
		
	}
}
