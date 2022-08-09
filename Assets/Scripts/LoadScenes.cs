using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    public GameObject Umgebung;

    public bool activeMessageBubble = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

       else  if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Umgebung.SetActive(!Umgebung.activeSelf);
        }

        else if(Input.GetKeyDown(KeyCode.M))
        {
            activeMessageBubble = !activeMessageBubble;
        }
        //else if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(0).name);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(1).name);
        //}
    }
}
