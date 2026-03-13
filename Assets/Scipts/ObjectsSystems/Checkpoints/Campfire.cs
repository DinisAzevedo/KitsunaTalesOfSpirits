using UnityEngine;

[System.Obsolete]
public class CampfireCheckpoint : MonoBehaviour, IInterectable
{
    private static CampfireCheckpoint activeCheckpoint = null;
    private Animator animator;
    private bool isActive = false;

    [Header("Áudio")]
    public AudioSource fireAudioSource;  // Fonte de som do fogo
    public AudioClip fireClip;           // Som do fogo (loop)
    public float soundRadius = 5f;       // Distância máxima para ouvir

    private Transform playerTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        SetInactive(); // começa apagado

        // Configura o AudioSource
        if (fireAudioSource == null)
            fireAudioSource = GetComponent<AudioSource>();

        if (fireAudioSource != null)
        {
            fireAudioSource.clip = fireClip;
            fireAudioSource.loop = true;
            fireAudioSource.playOnAwake = false;
        }

        // Procura o jogador
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerTransform = playerObj.transform;
    }

    private void Start()
    {
        // Restaura checkpoint do save
        PlayerSave playerSave = FindObjectOfType<PlayerSave>();
        if (playerSave != null && playerSave.currentCheckpoint == gameObject.name)
        {
            ActivateCheckpoint();
        }
    }

    private void Update()
    {
        if (isActive && fireAudioSource != null && playerTransform != null)
        {
            float distance = Vector2.Distance(playerTransform.position, transform.position);

            if (distance <= soundRadius)
            {
                if (!fireAudioSource.isPlaying)
                    fireAudioSource.Play();
            }
            else
            {
                if (fireAudioSource.isPlaying)
                    fireAudioSource.Stop();
            }
        }
    }

    public bool CanInteract()
    {
        return !isActive;
    }

    public void Interact()
    {
        if (isActive) return;

        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        PlayerSave playerSave = FindObjectOfType<PlayerSave>();

        if (player != null)
        {
            player.SetCheckpoint(transform.position);
            ActivateCheckpoint();

            // Auto-save
            if (playerSave != null)
            {
                playerSave.currentCheckpoint = gameObject.name;
                playerSave.SaveGame();
            }

            Debug.Log("Checkpoint ativado e jogo guardado");
        }
    }

    private void ActivateCheckpoint()
    {
        if (activeCheckpoint != null)
            activeCheckpoint.SetInactive();

        isActive = true;
        activeCheckpoint = this;

        if (animator != null)
            animator.SetBool("IsLit", true);
    }

    private void SetInactive()
    {
        isActive = false;
        if (animator != null)
            animator.SetBool("IsLit", false);

        if (fireAudioSource != null && fireAudioSource.isPlaying)
            fireAudioSource.Stop();
    }
}
