using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO itemSO;
    private int quantity = 1;
    private InventoryManager inventoryManager;
    private CoinDisplay coinDisplay;
    private AbilityUIManager abilityUIManager;
    private AudioSource audioSource; // AudioSource 
    private float nextPickupTime;

    void Start()
    {
        coinDisplay = FindFirstObjectByType<CoinDisplay>();
        inventoryManager = GameObject.Find("InventoryCanvas")?.GetComponent<InventoryManager>();
        abilityUIManager = FindFirstObjectByType<AbilityUIManager>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetItemData(ItemSO itemSO_, int qty)
    {
        itemSO = itemSO_;
        quantity = qty;
    }

    private void PlayPickupSound()
    {
        if (itemSO != null && itemSO.pickupSound != null)
        {
            // Toca o som no AudioSource do prefab
            if (audioSource != null)
                audioSource.PlayOneShot(itemSO.pickupSound);
            else
                AudioSource.PlayClipAtPoint(itemSO.pickupSound, transform.position);
        }
    }

    private void TryPickup()
    {
        if (itemSO == null) return;

        // Moeda
        if (itemSO.isCurrency)
        {
            PlayPickupSound();
            coinDisplay.AddCoins(itemSO.coinValue * quantity);
            Destroy(gameObject);
            return;
        }

        // Habilidade
        if (itemSO.isAbility)
        {
            PlayPickupSound();
            itemSO.UseItem();

            if (abilityUIManager != null)
            {
                abilityUIManager.ShowAbilityUI(itemSO.sprite, itemSO.itemName, 3f);
            }

            Destroy(gameObject);
            return;
        }

        // Item normal
        if (inventoryManager != null)
        {
            int remaining = inventoryManager.AddItem(
                itemSO.itemName,
                quantity,
                itemSO.sprite,
                itemSO.itemDescription
            );

            if (remaining <= 0)
            {
                PlayPickupSound();
                Destroy(gameObject);
            }
            else
                quantity = remaining;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        TryPickup();
        nextPickupTime = Time.time;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (Time.time < nextPickupTime) return;

        TryPickup();
        nextPickupTime = Time.time;
    }
}
