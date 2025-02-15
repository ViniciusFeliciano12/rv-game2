using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI ammoCanvas;

    public static GameController Instance { get; set; }
    
    private bool gameIsPaused = false;
    private int totalAmmo = 115;
    private int actualAmmo = 15;
    private int playerLife = 15;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;  
        DynamicGI.UpdateEnvironment();
        ResetAmmoCanvas();
    }

    private void ResetAmmoCanvas(){
        ammoCanvas.text = $"Ammo: {actualAmmo}/{totalAmmo}";
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
            ResetAmmoCanvas();
            return true;
        }
        return false;
    }

    public void PlayerHit(){
        playerLife--;
    }

    public bool Reload(){
        var howManyAmmo = 15 - actualAmmo;

        if (howManyAmmo > 0 && totalAmmo > 15){
            actualAmmo += howManyAmmo;
            totalAmmo -= howManyAmmo;
            ResetAmmoCanvas();
            return true;
        }
        else if(howManyAmmo > 0 && totalAmmo > 0){
            actualAmmo += totalAmmo;
            totalAmmo = 0;
            ResetAmmoCanvas();
            return true;
        }

        return false;
    }
}
