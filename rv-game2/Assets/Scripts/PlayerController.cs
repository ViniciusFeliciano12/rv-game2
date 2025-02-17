using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
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

    private bool dead = false;

    private float currentSpeed = 0f;
    private float speedSmoothVelocity = 0f;
    private readonly float speedSmoothTime = 0.2f;

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
        if (GameController.Instance.VerifyGameState() || dead)
            return;

        if (GameController.Instance.IsDead()){
            Die();
        }
    
        fireCooldownTimer -= Time.deltaTime;

        UpdateRotation();
        UpdateMovement();

        if (Input.GetMouseButton(0) && fireCooldownTimer <= 0f)
        {
            Firing();
            fireCooldownTimer = fireCooldown;
        }
        else if (Input.GetKey(KeyCode.R))
        {
            Reload();
        }
    }

    void UpdateRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        playerTransform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        if (playerCamera != null)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }
    }

    void UpdateMovement()
    {
        Vector3 movementDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            movementDirection += Vector3.forward;
        if (Input.GetKey(KeyCode.S))
            movementDirection += Vector3.back;
        if (Input.GetKey(KeyCode.A))
            movementDirection += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            movementDirection += Vector3.right;
        if (Input.GetKeyDown(KeyCode.Space) && !IsPlayingAnimation("reloading"))
            playerAnimator.SetTrigger("Jumping");

        float targetSpeed = movementDirection.magnitude > 0 ? 1f : 0f;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        playerTransform.Translate(2 * Time.deltaTime * movementDirection.normalized);

        playerAnimator.SetFloat("Walking", currentSpeed);
    }

    void ShootRaycast()
    {
        Vector3 rayOrigin = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward;
        float maxDistance = 100f;

        Debug.DrawRay(rayOrigin, rayDirection * maxDistance, Color.red);

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, maxDistance, ~LayerMask.GetMask("Player")))
        {
            Debug.Log("Acertou o objeto: " + hit.collider.name);
            if(hit.transform.CompareTag("Enemy")){
                hit.transform.GetComponent<EnemyAIController>().TakenHit();
            }else{
                GameObject bulletHole = new("bullet_hole");

                SpriteRenderer sr = bulletHole.AddComponent<SpriteRenderer>();
                sr.sprite = bulletHoleSprite;

                bulletHole.transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(bulletHole, 10f);
            }
            
        }
        else
        {
            Debug.Log("Nada foi atingido.");
        }
    }

    void Reload()
    {
        if (GameController.Instance.Reload() && !IsPlayingAnimation("jumping"))
        {
            playerAudios[1].Play();
            playerAudios[2].Play();
            playerAnimator.SetTrigger("Reload");
        }
    }

    void Die()
    {
        dead = true;
        playerAnimator.SetTrigger("Dying");
    }

    void Firing()
    {
        if (GameController.Instance.Fire() && !IsPlayingAnimation("jumping") && !IsPlayingAnimation("reloading"))
        {
            playerAudios[0].Play();
            playerAnimator.SetTrigger("Shooting");
            ShootRaycast();
        }
    }
    bool IsPlayingAnimation(string animationName)
    {
        return playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }
}
