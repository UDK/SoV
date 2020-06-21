using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool gameIsPause = false;

    public delegate void TextAlphaUnsigned(float alpha);
    /// <summary>
    /// Евент чтобы показывать/прятать главное меню при паузе 
    /// </summary>
    static public event TextAlphaUnsigned Notify;
    public bool GameIsPause {
        get => gameIsPause;
        set
        {
            if(value == true)
            {
                Time.timeScale = 0;
                gameIsPause = value;
                Notify(1);
            }
            else
            {
                Time.timeScale = 1;
                gameIsPause = value;
                Notify(0);
            }

        }
    }
    void Update()
    {
        //Esc всегда escape и переопределения, вряд ли, будет
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameIsPause = !GameIsPause;
        }
    }
}
