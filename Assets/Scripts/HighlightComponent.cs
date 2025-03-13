using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightComponent : MonoBehaviour
{
    private HighlightEffect effect;

    void Awake()
    {
        if(GetComponent<HighlightEffect>()==null)
        {
            this.gameObject.AddComponent<HighlightEffect>();
        }
        effect = GetComponent<HighlightEffect>();
    }

 
    private void Start()
    {
        DeactivateHighlight();
    }

    public void ActivateHighlight()
    {
        effect.highlighted = true;
    }

    public void DeactivateHighlight()
    {
        effect.highlighted = false;
    }
}