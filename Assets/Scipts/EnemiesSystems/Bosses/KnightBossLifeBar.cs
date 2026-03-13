using UnityEngine;
using UnityEngine.UI;

public class BossLifeBar : MonoBehaviour
{
    [Header("Referências")]
    public BossHealth bossHealth;   // referência ao BossHealth
    public Image lifeBarImage;      // Image do Canvas
    public Sprite[] lifeSprites;    // sprites da barra
    
    [Header("Collider do Boss")]
    public Collider2D bossCollider;  // Collider do boss 
    
    private int lastSpriteIndex = -1;
    private CanvasGroup canvasGroup;
    private bool playerInRange = false;

    void Start()
    {
        // Se não tiver collider atribuído, tenta pegar do BossHealth
        if (bossCollider == null && bossHealth != null)
            bossCollider = bossHealth.GetComponent<Collider2D>();

        // Se o lifeBarImage tem um Canvas parent, usa CanvasGroup para fade
        if (lifeBarImage != null)
        {
            canvasGroup = lifeBarImage.GetComponentInParent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = lifeBarImage.gameObject.AddComponent<CanvasGroup>();
            
            // Começa invisível
            canvasGroup.alpha = 0f;
        }

        // Registra este script no collider do boss para receber callbacks
        if (bossCollider != null)
        {
            // Se o collider não tem um script que chama OnTrigger, adiciona este script ao boss
            BossLifeBarTrigger trigger = bossCollider.GetComponent<BossLifeBarTrigger>();
            if (trigger == null)
            {
                trigger = bossCollider.gameObject.AddComponent<BossLifeBarTrigger>();
            }
            trigger.lifeBar = this;
        }

        UpdateLifeBar();
    }

    void Update()
    {
        if (playerInRange && bossHealth != null)
            UpdateLifeBar();
    }

    public void OnPlayerEntered()
    {
        playerInRange = true;
        if (canvasGroup != null)
            canvasGroup.alpha = 1f;
    }

    public void OnPlayerExited()
    {
        playerInRange = false;
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    void UpdateLifeBar()
    {
        if (bossHealth == null) return;

        // calcula percentual de vida
        float healthPercent = (float)bossHealth.health / bossHealth.healthMax;
        int index = Mathf.RoundToInt((1f - healthPercent) * (lifeSprites.Length - 1));

        index = Mathf.Clamp(index, 0, lifeSprites.Length - 1);

        // muda sprite apenas se necessário
        if (index != lastSpriteIndex)
        {
            lifeBarImage.sprite = lifeSprites[index];
            lastSpriteIndex = index;
        }
    }
}

// Script auxiliar para detectar triggers no boss
public class BossLifeBarTrigger : MonoBehaviour
{
    public BossLifeBar lifeBar;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && lifeBar != null)
        {
            lifeBar.OnPlayerEntered();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && lifeBar != null)
        {
            lifeBar.OnPlayerExited();
        }
    }
}
