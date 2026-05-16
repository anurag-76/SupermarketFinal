using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    // We assign all the renderers here through the inspector
    [SerializeField]
    private List<Renderer> renderers;

    [SerializeField]
    private Color color = Color.white;

    // Helper list to cache all the materials of this object
    private List<Material> materials;

    // Gets all the materials from each renderer
    private void Awake()
    {
        materials = new List<Material>();
        foreach (var renderer in renderers)
        {
            materials.AddRange(new List<Material>(renderer.materials));
        }
    }

    // Toggle highlight on/off
    public void ToggleHighlight(bool val)
    {
        if (val)
        {
            foreach (var material in materials)
            {
                // We need to enable the EMISSION
                material.EnableKeyword("_EMISSION");
                // before we can set the color
                material.SetColor("_EmissionColor", color);
            }
        }
        else
        {
            foreach (var material in materials)
            {
                // we can just disable the EMISSION
                // if we don't use emission color anywhere else
                material.DisableKeyword("_EMISSION");
            }
        }
    }

}
