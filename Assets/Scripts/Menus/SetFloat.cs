using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFloat : MonoBehaviour {

    public TMPro.TMP_Text valueText;
    
	void Awake ()
    {
        valueText = GetComponent<TMPro.TMP_Text>();
	}
	
    public void Set (float value)
    {
        if (valueText == null) { Debug.LogWarning("yep"); }
        valueText.text = value.ToString("N2");
    }
}
