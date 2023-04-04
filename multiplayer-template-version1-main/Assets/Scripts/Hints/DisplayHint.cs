using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHint : MonoBehaviour
{
    public Text Textfield;

    public void SetText(string text)
    {
        Textfield.text = text;
    }
}
