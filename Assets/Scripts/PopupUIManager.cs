using Gravitons.UI.Modal;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PopupUIManager : MonoBehaviour
{
    public string currentTitle = "Default Title";
    [TextArea]
    public string currentBodyText = "Default Text Lorem Ipsum Dolor Sit Amet the fitnessgram pacer test is a multistage etc etc";
    public string currentURL = "www.hackertyper.net";
    public string currentURLButtonText = "Learn More";
    public string currentConfirmText = "Close";
    public string currentNextLevelBtnText = "Next Level";

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
                new ModalButton() { Text = currentNextLevelBtnText}
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
}
