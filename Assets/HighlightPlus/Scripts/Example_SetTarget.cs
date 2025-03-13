using UnityEngine;
using HighlightPlus;

public class Example_SetTarget : MonoBehaviour
{
    // Reference to the HighlightEffect component
    public HighlightEffect highlightEffect;
    
    // Optional: GameObject to highlight when the script starts
    public Transform initialTarget;
    
    // Optional: Array of renderers to highlight (for SetTargets method)
    public Renderer[] specificRenderers;

    private void Start()
    {
        // Make sure we have a reference to a HighlightEffect component
        if (highlightEffect == null)
        {
            // Try to get the component from this GameObject
            highlightEffect = GetComponent<HighlightEffect>();
            
            // If still null, create one
            if (highlightEffect == null)
            {
                highlightEffect = gameObject.AddComponent<HighlightEffect>();
            }
        }
        
        // If an initial target was specified, highlight it
        if (initialTarget != null)
        {
            SetTargetAndHighlight(initialTarget);
        }
    }
    
    /// <summary>
    /// Sets a target transform and enables highlighting
    /// </summary>
    public void SetTargetAndHighlight(Transform targetTransform)
    {
        // Set the target
        highlightEffect.SetTarget(targetTransform);
        
        // Enable highlighting
        highlightEffect.highlighted = true;
    }
    
    /// <summary>
    /// Sets a target transform with specific renderers and enables highlighting
    /// </summary>
    public void SetTargetsAndHighlight(Transform targetTransform, Renderer[] renderers)
    {
        // Set the targets
        highlightEffect.SetTargets(targetTransform, renderers);
        
        // Enable highlighting
        highlightEffect.highlighted = true;
    }
    
    /// <summary>
    /// Example method that could be called from a UI button or other event
    /// </summary>
    public void HighlightGameObject(GameObject targetObject)
    {
        if (targetObject != null)
        {
            SetTargetAndHighlight(targetObject.transform);
        }
    }
    
    /// <summary>
    /// Example method to stop highlighting
    /// </summary>
    public void StopHighlighting()
    {
        highlightEffect.highlighted = false;
    }
} 