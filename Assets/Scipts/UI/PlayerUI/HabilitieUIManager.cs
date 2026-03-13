using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIManager : MonoBehaviour
{
    public GameObject abilitiesPanel;       // Painel que contém a UI
    public Image abilityPortrait;           // Imagem da habilidade
    public TMPro.TMP_Text abilityName;      // Nome da habilidade 

    // Mostra os objetos durante X segundos
    public void ShowAbilityUI(Sprite portrait, string name, float duration)
    {
        StartCoroutine(ShowAbilityCoroutine(portrait, name, duration));
    }

    private IEnumerator ShowAbilityCoroutine(Sprite portrait, string name, float duration)
    {
        // Encontrar objetos
        if (abilitiesPanel == null) abilitiesPanel = GameObject.Find("AbilitiesPanel");
        if (abilityPortrait == null) abilityPortrait = GameObject.Find("HabilitiePortrait")?.GetComponent<Image>();
        if (abilityName == null) abilityName = GameObject.Find("HabilitieName")?.GetComponent<TMPro.TMP_Text>();

        // Atualizar UI
        if (abilityPortrait != null) abilityPortrait.sprite = portrait;
        if (abilityName != null) abilityName.text = "Nova Habilidade Desbloqueada: " + name;

        abilitiesPanel.SetActive(true);
        abilityPortrait.gameObject.SetActive(true);
        abilityName.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        abilitiesPanel.SetActive(false);
        abilityPortrait.gameObject.SetActive(false);
        abilityName.gameObject.SetActive(false);
    }
}
