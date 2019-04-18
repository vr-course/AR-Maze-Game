using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinRotate : MonoBehaviour
{
    bool Playing;
    GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Playing == true && gameController.Playing == false)
        {
            this.gameObject.SetActive(true);

        }
        Playing = gameController.Playing;
        transform.Rotate(new Vector3(0,2,0));

    }
}
