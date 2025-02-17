using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    protected DifficultyGame difficultyScript;

    public TextMeshProUGUI ammoCanvas;
    public TextMeshProUGUI lifeCanvas;
    public TextMeshProUGUI scoreCanvas;
    public static GameController Instance { get; set; }
    
    private bool gameIsPaused = false;
    private int totalAmmo = 115;
    private int actualAmmo = 15;
    private int playerLife = 15;
    private int playerMaxLife = 15;
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
        InitializeGame();
    }

    void InitializeGame(){
        switch(GetDifficulty()){
            case Difficulty.Easy: 
                playerLife = 20;
                playerMaxLife = 20;
                totalAmmo = 200;
                break;
            case Difficulty.Medium: 
                playerLife = 15;
                playerMaxLife = 15;
                totalAmmo = 125;
                break;
            case Difficulty.Hard: 
                playerLife = 10;
                playerMaxLife = 10;
                totalAmmo = 100;
                break;
            default: break;
        }

        ResetCanvas();
    }

    public Difficulty GetDifficulty(){
        return difficultyScript.difficultyData.Difficulty;
    }

    private void ResetCanvas(){
        ammoCanvas.text = $"Ammo: {actualAmmo}/{totalAmmo}";
        lifeCanvas.text = $"Life: {playerLife}/{playerMaxLife}";
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
