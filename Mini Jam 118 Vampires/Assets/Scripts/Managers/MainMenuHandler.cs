
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    private static string GAMESCENENAME = "GameViñeta";

    public void StartGame()
    {
        SceneManager.LoadScene(GAMESCENENAME);
    }
}
