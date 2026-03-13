using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class RebindUI : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference actionReference;
    [SerializeField] private int bindingIndex;
    [Header("UI")]
    [SerializeField] private Text actionLabel;
    [SerializeField] private Text bindingText;
    [SerializeField] private Text rebindPrompt;

    private InputActionRebindingExtensions.RebindingOperation rebindOperation;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (actionReference == null)
            return;

        var action = actionReference.action;

        if (actionLabel != null)
            actionLabel.text = action.name;

        if (bindingText != null)
            bindingText.text = action.GetBindingDisplayString(bindingIndex);
    }

    public void StartRebind()
    {
        var action = actionReference.action;
        if (action == null)
            return;

        action.Disable();

        if (rebindPrompt != null)
        {
            rebindPrompt.gameObject.SetActive(true);
            rebindPrompt.text = "Press key";
        }

        rebindOperation = action
    .PerformInteractiveRebinding(bindingIndex)
    .OnMatchWaitForAnother(0.1f)
    .OnComplete(operation =>
    {
        operation.Dispose();
        rebindOperation = null;
        action.Enable();
        if (rebindPrompt != null)
            rebindPrompt.gameObject.SetActive(false);
        UpdateUI();
    })
    .OnCancel(operation =>
    {
        operation.Dispose();
        rebindOperation = null;
        action.Enable();
        if (rebindPrompt != null)
            rebindPrompt.gameObject.SetActive(false);
    })
    .Start();
    }

    public void ResetBinding()
    {
        var action = actionReference.action;
        if (action == null)
            return;

        action.RemoveBindingOverride(bindingIndex);
        UpdateUI();
    }


}