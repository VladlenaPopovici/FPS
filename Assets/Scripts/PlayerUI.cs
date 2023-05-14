using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private TextMeshProUGUI _promptText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateText(string promptMessage)
    {
        _promptText.text = promptMessage;
    }
}
