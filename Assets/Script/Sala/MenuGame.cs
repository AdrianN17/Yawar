using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGame : MonoBehaviour
{
    bool activar;
    public GameObject Pause;
    void Start()
    {
        Pause.SetActive(false);
    }


    public void pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            activar = !activar;
            Pause.SetActive(activar);
            //Time.timeScale = (activar) ? 0 : 1f;
        }
    }

    void Update()
    {
        pause();
    }

    public void Volver_Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
