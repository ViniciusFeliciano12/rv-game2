using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Canvas initialMenuCanvas;
    public Canvas difficultyCanvas;
    public DifficultyGame difficultyScript;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartGame(){
        audioSource.Play();
        initialMenuCanvas.enabled = false;
        difficultyCanvas.enabled = true;
    }

    public void SelectDifficulty(int difficulty){
        difficultyScript.difficultyData.Difficulty = (Difficulty)difficulty;
        SceneManager.LoadScene(1);
    }

    public void ExitGame(){
        audioSource.Play();
        Application.Quit();
    }
}
