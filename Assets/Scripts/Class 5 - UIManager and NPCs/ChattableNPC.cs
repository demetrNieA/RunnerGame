using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChattableNPC : MonoBehaviour
{
    bool triggered = false;
    bool inDialog = false;

    [SerializeField] DialogModeControlVariant dialogControlVariant;

    [SerializeField] string npcName = "NPC";
    [SerializeField] List<DialogData> dialogData = new List<DialogData>();


    // Update is called once per frame
    void Update()
    {
        if (inDialog)
        {
            RunDialogModeSequence();
        }
    }

    #region Dialog Mode Sequence
    public virtual void StartDialogModeSequence()
    {
        DialogManager.instance.TriggerDialog(npcName, dialogData);
        //MyroController.instance.AssignControlVariant(dialogControlVariant);
        MyroController.instance.SetToDialogControls(dialogControlVariant);
        triggered = true;
        inDialog = true;
    }
    public virtual void RunDialogModeSequence()
    {
        if (!DialogManager.instance.IsDialogRunning()) EndDialogModeSequence();
    }
    public virtual void EndDialogModeSequence()
    {
        inDialog = false;
        MyroController.instance.EndControlVariant();
    }
    #endregion

    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !triggered)
        {
            StartDialogModeSequence();
        }
    }
}
