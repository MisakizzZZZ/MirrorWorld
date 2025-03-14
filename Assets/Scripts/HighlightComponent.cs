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
        SetActivateHighlight(false);
    }

    public void SetActivateHighlight(bool value)
    {
        effect.highlighted = value;
    }

}