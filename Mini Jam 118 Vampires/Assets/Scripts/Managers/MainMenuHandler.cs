
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    private static string GAMESCENENAME = "Game";

    public void StartGame()
    {
        SceneManager.LoadScene(GAMESCENENAME);
    }
}
