using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Sprite bulletHoleSprite; 

    private Animator playerAnimator;
    private Transform playerTransform;
    private Camera playerCamera;
    private AudioSource[] playerAudios;

    private readonly float rotationSpeed = 2f;
    private readonly float verticalLookLimit = 80f;
    private readonly float fireCooldown = 0.7f;
    private float verticalRotation = 0f;
    private float fireCooldownTimer = 0f;

   

    void Start()
    {
        playerTransform = GetComponent<Transform>();
        playerAnimator = GetComponent<Animator>();
        playerAudios = GetComponents<AudioSource>();

        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (GameController.Instance.VerifyGameState()){
            return;
        }

        fireCooldownTimer -= Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        playerTransform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;
        verticalRotation -= mouseY;  
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        if (playerCamera != null)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }

        Vector3 movementDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)){
            movementDirection += Vector3.forward;   
            Running();
        }
        if (Input.GetKey(KeyCode.S)){
            movementDirection += Vector3.back;
            Running();
        }
        if (Input.GetKey(KeyCode.A)){
            movementDirection += Vector3.left;
            Running();
        } 
        if (Input.GetKey(KeyCode.D)){
            movementDirection += Vector3.right;
            Running();
        }

        if(movementDirection == Vector3.zero){
            Idle();
        }

        playerTransform.Translate(2 * Time.deltaTime * movementDirection.normalized); 
        
        if (Input.GetMouseButton(0) && fireCooldownTimer <= 0f) 
        {
            Firing();
            fireCooldownTimer = fireCooldown; 
        }
        else if(Input.GetKey(KeyCode.R)){
            Reload();
        }
    }

    void ShootRaycast()
    {
        Vector3 rayOrigin = playerCamera.transform.position;
        
        Vector3 rayDirection = playerCamera.transform.forward;
        
        float maxDistance = 100f;

        Debug.DrawRay(rayOrigin, rayDirection * maxDistance, Color.red);

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, maxDistance, ~LayerMask.GetMask("Ignore Raycast")))
        {
            Debug.Log("Acertou o objeto: " + hit.collider.name);
            GameObject bulletHole = new("bullet_hole");

            SpriteRenderer sr = bulletHole.AddComponent<SpriteRenderer>();
            sr.sprite = bulletHoleSprite;

            bulletHole.transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(hit.normal));

            Destroy(bulletHole, 10f);
        }
        else
        {
            Debug.Log("Nada foi atingido.");
        }
    }

    void Idle()
    {
        playerAnimator.SetBool("Running", false);
        playerAnimator.SetBool("Firing", false);
    }

    void Reload(){
        if(GameController.Instance.Reload()){
            playerAudios[1].Play();
            playerAudios[2].Play();
        }
    }

    void Running()
    {
        playerAnimator.SetBool("Running", true);
        playerAnimator.SetBool("Firing", false);
    }

    void Firing()
    {
        if (GameController.Instance.Fire()){
            playerAnimator.SetBool("Running", false);
            playerAnimator.SetBool("Firing", true);
            playerAudios[0].Play();
            ShootRaycast();
        } 
    }
}
