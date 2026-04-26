using TMPro;
using UnityEngine;

public class HelperHintManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text helperHintText;

    [Header("Default Messages")]
    [SerializeField] private string idleMessage = "Choose a furniture item to begin.";
    [SerializeField] private string placementMessage = "Preview the furniture and confirm placement.";
    [SerializeField] private string editMessage = "Use the controls to rotate, scale, or recolor the selected object.";
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