using Gravitons.UI.Modal;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


//Hi. This manager is for generating popups and runtime through ModalManager.cs. If you want to to that through code,
//just use ModalManager.cs. This script is meant to contain public methods for use in unity event callbacks in the editor.

//Since unity event callbacks can only take 0 or 1 parameters, this manager script is intended to be called from multiple times
//to properly set values before calling the methods to create a popup.

//The workflow for this is meant to be:
// 1. put a unity event callback in your script to call this script from
// 2. call methods in the Set Values section to set appropriate values
// 3. at the end, call one of the create popup methods to create a popup using the currently set values.


public class PopupUIManager : MonoBehaviour
{
    [Tooltip("What the popup title is currently set to. Used by all popup creation methods")]
    public string currentTitle = "Default Title";
    [TextArea]
    [Tooltip("What the popup body text is currently set to. Used by all popup creation methods.")]
    public string currentBodyText = "Default Text Lorem Ipsum Dolor Sit Amet the fitnessgram pacer test is a multistage etc etc";
    [Tooltip("What the popup url is currently set to. Used by createPopupWithLink()")]
    public string currentURL = "www.hackertyper.com";
    [Tooltip("What the text on the url button is currently set to. Used by createPopupWithLink()")]
    public string currentURLButtonText = "Learn More";
    [Tooltip("What the confirm button text is currently set to. Used by createDefaultPopup() and createPopupWithLink()")]
    public string currentConfirmText = "Close";
    [Tooltip("What the next level button text is currently set to. Used in createNextLevelPopup()")]
    public string currentNextLevelBtnText = "Next Level";
    [Tooltip("What level is currently set to be loaded by the next level button. Used in createNextLevelPopup()")]
    public string targetSceneName = "0_Tutorial";

    ////////////
    //Set Values
    ////////////
    

    public void setTitle(string newTitle)
    {
        currentTitle = newTitle;
    }

    public void setBodyText(string newBody)
    {
        currentBodyText = newBody;
    }

    public void setURL(string newURL)
    {
        currentURL = newURL;
    }

    public void setURLButtonText(string newbtnText)
    {
        currentURLButtonText = newbtnText;
    }

    public void setConfirmText(string newConfirm)
    {
        currentConfirmText = newConfirm;
    }

    public void setNextLvlBtnText(string newBtnText)
    {
        currentNextLevelBtnText = newBtnText;
    }

    public void setTargetSceneName(string newName)
    {
        targetSceneName = newName;
    }

    //////////////////
    //Popup Management
    //////////////////
    
    public void createDefaultPopup()
    {
        ModalManager.Show(
            currentTitle,
            currentBodyText,
            new ModalButton[] { new ModalButton() { Text = currentConfirmText } }
            );
    }

    public void createPopupWithLink()
    {
        ModalManager.Show(
            currentTitle,
            currentBodyText,
            new ModalButton[] { 
                new ModalButton() { Text = currentConfirmText}, 
                new ModalButton() { Text = currentURLButtonText, Callback = goToCurrentURL} 
                }
            );
    }

    public void createNextLevelPopup()
    {
        ModalManager.Show(
            currentTitle,
            currentBodyText,
            new ModalButton[]{
                new ModalButton() { Text = currentNextLevelBtnText, Callback = loadTargetScene}
                }
            );
    }

    public void closeAllOpenPopups()
    {
        GenericModal[] openModals = transform.parent.GetComponentsInChildren<GenericModal>();

        for (int i = 0; i < openModals.Length; i++)
        {
            openModals[i].Close();
        }
    }


    ///////////
    //Callbacks
    ///////////

    private void goToCurrentURL()
    {
        Application.OpenURL(currentURL);
    }

    private void loadTargetScene()
    {
        //can we get a method for moving sequentially to next level that doesn't take a value?
        LevelManager.Instance.ChangeScene(targetSceneName);
    }
}
