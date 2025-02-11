using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartGame(){
        audioSource.Play();
        SceneManager.LoadScene(1);
    }

    public void ExitGame(){
        audioSource.Play();
        Application.Quit();
    }
}
