using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI ammoCanvas;
    public TextMeshProUGUI lifeCanvas;
    public TextMeshProUGUI scoreCanvas;


    public static GameController Instance { get; set; }
    
    private bool gameIsPaused = false;
    private int totalAmmo = 115;
    private int actualAmmo = 15;
    private int playerLife = 15;

    private int score = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;  
        DynamicGI.UpdateEnvironment();
        ResetCanvas();
    }

    private void ResetCanvas(){
        ammoCanvas.text = $"Ammo: {actualAmmo}/{totalAmmo}";
        lifeCanvas.text = $"Life: {playerLife}/15";
        scoreCanvas.text = $"{score}";
    }

    public void UpdateScore(){
        score++;
        ResetCanvas();
    }

    public void PauseResumeGame(bool state){
        gameIsPaused = state;
    }

    public bool VerifyGameState(){
        return gameIsPaused;
    }

    public bool Fire(){
        if (actualAmmo > 0){
            actualAmmo--;
            ResetCanvas();
            return true;
        }
        return false;
    }

    public void PlayerHit(){
        playerLife--;
        ResetCanvas();
    }
    
    public bool IsDead(){
        return playerLife <= 0;
    }

    public bool Reload(){
        var howManyAmmo = 15 - actualAmmo;
        bool value = false;
        if (howManyAmmo > 0 && totalAmmo > 15){
            actualAmmo += howManyAmmo;
            totalAmmo -= howManyAmmo;
            value = true;
        }
        else if(howManyAmmo > 0 && totalAmmo > 0){
            actualAmmo += totalAmmo;
            totalAmmo = 0;
            value = true;
        }

        ResetCanvas();
        return value;
    }
}
