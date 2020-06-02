using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool gameIsPause = false;
    public bool GameIsPause {
        get => gameIsPause;
        set
        {
            if(value == true)
            {
                Time.timeScale = 0;
                gameIsPause = value;
            }
            else
            {
                Time.timeScale = 1;
                gameIsPause = value;
            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Esc всегда escape и переопределения, вряд ли, будет
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameIsPause = !GameIsPause;
        }
    }
}
