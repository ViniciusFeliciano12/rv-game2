using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public Canvas pauseCanvas;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if(GameController.Instance.VerifyGameState()){
                ResumeGame();
            }
            else{
                PauseGame();
            }
        }
    }

    void PauseGame(){
        GameController.Instance.PauseResumeGame(true);
        pauseCanvas.enabled = true;
        Time.timeScale = 0.00001f;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame(){
        GameController.Instance.PauseResumeGame(false);
        pauseCanvas.enabled = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ExitGame(){
        ResumeGame();
        SceneManager.LoadScene(0);
    }
}
