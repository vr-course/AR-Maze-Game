using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject Joystick;
    GameObject scene;
    public Text PlayText;
    public bool Playing;
    // Start is called before the first frame update


    void Start()
    {
        Playing = false;
        Joystick.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

       
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

}
