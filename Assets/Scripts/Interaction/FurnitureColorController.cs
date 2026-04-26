using UnityEngine;
using UnityEngine.UI;

public class FurnitureColorController : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button buttonColorWhite;
    [SerializeField] private Button buttonColorBlack;
    [SerializeField] private Button buttonColorBlue;
    [SerializeField] private Button buttonColorGreen;

    [Header("Managers")]
    [SerializeField] private FurnitureSelectionManager selectionManager;

    private void Start()
    {
        if (buttonColorWhite != null)
            buttonColorWhite.onClick.AddListener(() => ApplyColor(Color.white));

        if (buttonColorBlack != null)
            buttonColorBlack.onClick.AddListener(() => ApplyColor(Color.black));

        if (buttonColorBlue != null)
            buttonColorBlue.onClick.AddListener(() => ApplyColor(Color.blue));

        if (buttonColorGreen != null)
            buttonColorGreen.onClick.AddListener(() => ApplyColor(Color.green));
    }

    public void ApplyColor(Color color)
    {
        GameObject target = GetSelectedObject();
        if (target == null) return;

        Renderer[] renderers = target.GetComponentsInChildren<Renderer>(true);

        foreach (Renderer rendererComponent in renderers)
        {
            Material[] materials = rendererComponent.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i] != null && materials[i].HasProperty("_Color"))
                {
                    materials[i].color = color;
                }
            }
        }
    }

    private GameObject GetSelectedObject()
    {
        if (selectionManager == null)
        {
            Debug.LogWarning("FurnitureColorController: SelectionManager not assigned.");
            return null;
        }

        return selectionManager.CurrentSelectedObject;
    }
}