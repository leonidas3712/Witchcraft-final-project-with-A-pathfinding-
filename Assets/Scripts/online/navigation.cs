using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class navigation : MonoBehaviour
{
    public void startServer()
    {
        SceneManager.LoadScene("serverScene");
    }
    public void startClient()
    {
        SceneManager.LoadScene("clientScene");
    }
    public void exit()
    {
        Application.Quit();
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }
}
