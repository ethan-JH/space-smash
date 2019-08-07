using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * script to control dropdown menu
 */
public class DropdownController : MonoBehaviour
{   
    // the panel with dropdown options
    public GameObject dropdownPanel;

    // called when script instance is loaded, intially hides panel
    private void Awake()
    {
        dropdownPanel.SetActive(false);
    }

    // activates/ deactivates dropdown when clicked
    public void ActivateDropdown()
    {
        if (dropdownPanel.activeSelf)
        {
            dropdownPanel.SetActive(false);
        } 
        else
        {
            dropdownPanel.SetActive(true);
        }
    }

}
