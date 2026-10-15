
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject gameplay;
    public GameObject menuEnvironment;
    public GameObject menuUI;
    public AudioSource menuMusic;
    public AudioSource ambientMusic;

    public void StartGame()
    {
        if (gameplay != null)
        {
            gameplay.SetActive(true);
        }

        if (menuEnvironment != null)
        {
            Destroy(menuEnvironment);
        }

        if (menuMusic != null)
        {
            menuMusic.Stop();
        }

        if (ambientMusic != null)
        {
            ambientMusic.Play();
        }

        if (menuUI != null)
        {
            menuUI.SetActive(false);
        }
    }
}
