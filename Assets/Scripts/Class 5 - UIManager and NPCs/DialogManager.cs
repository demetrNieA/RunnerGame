using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    [SerializeField] GameObject dialogBox;
    [SerializeField] TextMeshProUGUI dialogBodyText;
    [SerializeField] TextMeshProUGUI dialogNameText;
    [SerializeField] GameObject yesNoButtons;

    List<DialogData> savedDialogDataList = new List<DialogData>();
    bool dialogRunning = false;
    bool dialogProgressedThisFrame = false;
    int dialogProgressionCount = 0;

    bool dialogYesNoValue = false;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        HideDialog();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogRunning) RunDialog();
    }

    #region Standard Dialog Run
    public void TriggerDialog(string npcName, List<DialogData> dialogDatas)
    {
        if(dialogDatas == null)
        {
            Debug.LogError("Attempted to send a null Dialog Data List to DialogManager.TriggerDialog");
            return;
        }

        // Turn the dialog box parent GameObject on
        dialogBox.SetActive(true);
        // Set the NPC name
        dialogNameText.text = npcName;
        // Restart the counter for the list index of information to pull from
        dialogProgressionCount = 0;

        savedDialogDataList = dialogDatas;
        NextDialog();

        // Set the dialog logic to run
        dialogRunning = true;
    }
    void RunDialog()
    {
        if (ProgressDialogButtonPressed() && !dialogProgressedThisFrame)
        {

            dialogProgressedThisFrame = true;
            NextDialog();
        } else
        {
            dialogProgressedThisFrame = false;
        }
    }
    void NextDialog()
    {
        if (dialogProgressionCount >= savedDialogDataList.Count)
        {
            EndDialog();
        }
        else
        {
            // Set the first body text
            dialogBodyText.text = savedDialogDataList[dialogProgressionCount].dialogText;

            // If we have audio, play it
            if (savedDialogDataList[dialogProgressionCount].dialogAudio != null) 
               AudioManager.instance.PlayVoice(savedDialogDataList[dialogProgressionCount].dialogAudio);

            // Increment the Dialog Data List index
            dialogProgressionCount++;
        }
    }
    void EndDialog()
    {
        dialogRunning = false;
        dialogBox.SetActive(false);
    }
    void HideDialog()
    {
        dialogBox.SetActive(false);
        if(yesNoButtons != null) yesNoButtons.SetActive(false);
    }
    #endregion

    #region YesNo Dialog Functionality
    public void TriggerYesNoDialog(string npcName, DialogData dialogData)
    {
        if (yesNoButtons != null) yesNoButtons.SetActive(true);
        dialogBox.SetActive(true);
        dialogBodyText.text = dialogData.dialogText;
        dialogNameText.text = npcName;
    }
    public void YesButtonClicked()
    {
        dialogYesNoValue = true;
    }
    public void NoButtonClicked()
    {
        dialogYesNoValue = false;
    }
    #endregion

    #region Utility
    bool ProgressDialogButtonPressed()
    {
        return (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E));
    }

    public bool IsDialogRunning()
    {
        return dialogRunning;
    }

    public bool GetDialogYesNoValue()
    {
        return dialogYesNoValue;
    }
    #endregion
}

[Serializable]
public class DialogData
{
    public string dialogText = "";
    public AudioClip dialogAudio;
}