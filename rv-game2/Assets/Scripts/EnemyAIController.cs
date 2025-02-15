using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    public Transform player;  // Referência ao jogador
    
    private EnemyManager enemyManager;
    private AudioSource[] playerAudios;
    private NavMeshAgent agent;
    private Animator agentAnimator;
    private const float fireCooldown = 0.7f;
    private float fireCooldownTimer = 0f;
    private int ammo = 15;
    private int lifes = 5;
    private const float rayHeightOffset = 1.5f;
    private const float rotationSpeed = 5f;
    private const float maxDistance = 100f;
    private const float hitChance = 0.3f; 

    void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        playerAudios = GetComponents<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        agentAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (lifes > 0){
            fireCooldownTimer -= Time.deltaTime;
            HandleAI();
        }
    }

    public void TakenHit(){
        lifes--;

        if (lifes == 0){
            agentAnimator.SetTrigger("Dying");
            enemyManager.SpawnEnemy();
            Destroy(gameObject, 4f);
        }
    }

    void HandleAI()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * rayHeightOffset;
        Vector3 rayDirection = (player.position + Vector3.up * rayHeightOffset) - rayOrigin;

        Debug.DrawRay(rayOrigin, rayDirection * maxDistance, Color.red, 2f);

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, maxDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                EngagePlayer();
            }
            else
            {
                ChasePlayer();
            }
        }
    }

    void EngagePlayer()
    {
        agent.isStopped = true;
        agentAnimator.SetFloat("Walking", 0f);
        FacePlayer();

        if (ammo > 0)
        {
            if (!IsPlayingAnimation("reloading") && fireCooldownTimer <= 0f)
            {
                playerAudios[0].Play();
                agentAnimator.SetTrigger("Shooting");
                ammo--;
                fireCooldownTimer = fireCooldown;

                if (Random.value <= hitChance)
                {
                    GameController.Instance.PlayerHit();
                }
            }
        }
        else
        {
            ReloadWeapon();
        }
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        agentAnimator.SetFloat("Walking", 1f);
    }

    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void ReloadWeapon()
    {
        playerAudios[1].Play();
        playerAudios[2].Play();
        agentAnimator.SetTrigger("Reload");
        ammo = 15;
    }

    bool IsPlayingAnimation(string animationName)
    {
        return agentAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }
}
