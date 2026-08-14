using UnityEngine;
public class BriefcaseChoiceManager : MonoBehaviour
{
    [Header("Story")]
    public WomanTrigger womanTrigger;
    [Header("Choice Group")]
    public GameObject choiceGroup;
    private bool choiceMade = false;
    public bool ChoseYes { get; private set; }
    public bool ChoseNo { get; private set; }

    private void OnEnable()
    {
        // 每次选择界面重新出现时允许重新选择
        choiceMade = false;
        ChoseYes = false;
        ChoseNo = false;
        Debug.Log("Briefcase Choice Manager ENABLED and ready.");
    }

    // =====================================================
    // YES
    // =====================================================
    public void ChooseYes()
    {
        Debug.Log(">>> YES BUTTON CLICK RECEIVED <<<");
        if (choiceMade)
        {
            Debug.LogWarning("YES ignored because choiceMade is already TRUE.");
            return;
        }
        choiceMade = true;
        ChoseYes = true;
        if (womanTrigger != null)
        {
            Debug.Log("Sending YES to WomanTrigger.");
            womanTrigger.TriggerBriefcaseYesChoice();
        }
        else
        {
            Debug.LogError("WomanTrigger is NOT assigned!");
        }
        HideChoices();
    }

    // =====================================================
    // NO
    // =====================================================
    public void ChooseNo()
    {
        Debug.Log(">>> NO BUTTON CLICK RECEIVED <<<");
        if (choiceMade)
        {
            Debug.LogWarning("NO ignored because choiceMade is already TRUE.");
            return;
        }
        choiceMade = true;
        ChoseNo = true;
        if (womanTrigger != null)
        {
            Debug.Log("Sending NO to WomanTrigger.");
            womanTrigger.TriggerBriefcaseNoChoice();
        }
        else
        {
            Debug.LogError("WomanTrigger is NOT assigned!");
        }
        HideChoices();
    }

    // =====================================================
    // Hide Choices
    // =====================================================
    private void HideChoices()
    {
        if (choiceGroup != null)
        {
            choiceGroup.SetActive(false);
        }
        else
        {
            Debug.LogWarning(
                "Choice Group is empty. Choices were not hidden."
            );
        }
    }
}