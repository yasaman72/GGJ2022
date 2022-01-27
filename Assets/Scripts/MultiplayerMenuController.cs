using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiplayerMenuController : MonoBehaviour
{
    [SerializeField] private TMP_InputField codeInputField;
    [SerializeField] private TextMeshProUGUI hostCode;

    public void OnJoinBtnClicked()
    {
        Debug.Log("clicked on join btn");
       // get the entered code: codeInputField.text
    }

    public void OnHostBtnClicked()
    {
        Debug.Log("clicked on host btn");

        // set the host code: hostCode.text = "";
    }

    // this one is called when finished editing the text for the host code
    public void OnEnteredHostCode()
    {
        Debug.Log("entered host code");
    }

    public void OnShowMultiplayerPanel()
    {
        hostCode.text = "---------";
        codeInputField.text = string.Empty;
    }

    public void OnHideMultiplayerPanel()
    {
    }
}
