using TMPro;
using UnityEngine;

public class HelperHintManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text helperHintText;

    [Header("Default Messages")]
    [SerializeField] private string idleMessage = "Choose a furniture item to begin.";
    [SerializeField] private string placementMessage = "Rotate the object, then place or cancel.";
[SerializeField] private string editMessage = "Furniture placed. Select another item to continue.";
    [SerializeField] private string noSelectionMessage = "No furniture selected.";

    public void SetIdleHint()
    {
        SetHint(idleMessage);
    }

    public void SetPlacementHint()
    {
        SetHint(placementMessage);
    }

    public void SetEditHint()
    {
        SetHint(editMessage);
    }

    public void SetNoSelectionHint()
    {
        SetHint(noSelectionMessage);
    }

    public void SetHint(string message)
    {
        if (helperHintText != null)
        {
            helperHintText.text = message;
        }
    }
}