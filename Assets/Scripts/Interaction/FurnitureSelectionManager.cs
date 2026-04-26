using TMPro;
using UnityEngine;

public class FurnitureSelectionManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text selectedItemText;

    [Header("Optional")]
    [SerializeField] private HelperHintManager helperHintManager;

    public GameObject CurrentSelectedObject { get; private set; }

    public void SelectObject(GameObject target)
    {
        CurrentSelectedObject = target;
        UpdateSelectedLabel();

        if (helperHintManager != null && CurrentSelectedObject != null)
        {
            helperHintManager.SetEditHint();
        }
    }

    public void ClearSelection()
    {
        CurrentSelectedObject = null;
        UpdateSelectedLabel();

        if (helperHintManager != null)
        {
            helperHintManager.SetNoSelectionHint();
        }
    }

    public FurnitureInstance GetCurrentFurnitureInstance()
    {
        if (CurrentSelectedObject == null) return null;
        return CurrentSelectedObject.GetComponent<FurnitureInstance>();
    }

    private void UpdateSelectedLabel()
    {
        if (selectedItemText == null) return;

        if (CurrentSelectedObject == null)
        {
            selectedItemText.text = "Selected: None";
            return;
        }

        FurnitureInstance instance = CurrentSelectedObject.GetComponent<FurnitureInstance>();
        if (instance != null && !string.IsNullOrWhiteSpace(instance.displayName))
        {
            selectedItemText.text = $"Selected: {instance.displayName}";
        }
        else
        {
            selectedItemText.text = $"Selected: {CurrentSelectedObject.name}";
        }
    }
}