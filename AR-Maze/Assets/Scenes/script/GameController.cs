using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Transform SceneSpot;
    public GameObject Joystick;
    public Text LockText;
    public Text PlayText;
    GameObject scene;
    bool Lock = false;
    public bool Playing;
    bool found;
    // Start is called before the first frame update

    void Start()
    {
        Lock = false;
        Playing = false;
        Joystick.SetActive(false);
        scene = GameObject.Find("Maze");
    }

    // Update is called once per frame
    void Update()
    {
        found = GameObject.Find("ImageTarget").GetComponent<DefaultTrackableEventHandler>().found;
        if (Lock == false && found)
        {
            scene.transform.position = SceneSpot.position;
            scene.transform.rotation = SceneSpot.rotation;

        }
       
    }
    void GameOver()
    {
        Joystick.SetActive(false);
        Playing = false;
    }
    public void Play()
    {
        if (Playing)
        {
            Joystick.SetActive(false);
            PlayText.text = "Play";
            Playing = false;
        }
        else
        {
            Joystick.SetActive(true);
            PlayText.text = "Restart";
            Playing = true;
        }

    }

    //called when the button is pressed
    public void LockPosition()
    {
        if (Lock)
        {
            LockText.text = "Lock Positon";
            Lock = false;
        }
        else
        {
            LockText.text = "Unlock Positon";
            Lock = true;
        }
    }
}
