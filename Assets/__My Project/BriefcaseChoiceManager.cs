using UnityEngine;
using UnityEngine;
public class BriefcaseChoiceManager : MonoBehaviour
{
    [Header("Story")]
    public WomanTrigger womanTrigger;
    private bool choiceMade = false;
    public bool ChoseYes { get; private set; } = false;
    public bool ChoseNo { get; private set; } = false;

    public void ChooseYes()
    {
        if (choiceMade)
            return;
        choiceMade = true;
        ChoseYes = true;
        Debug.Log(
            "Player chose YES."
        );
        // 隐藏整个 YES / NO 选择组
        gameObject.SetActive(false);
        if (womanTrigger != null)
        {
            womanTrigger.TriggerBriefcaseYesChoice();
        }
    }

    public void ChooseNo()
    {
        if (choiceMade)
            return;
        choiceMade = true;
        ChoseNo = true;
        Debug.Log(
            "Player chose NO."
        );
        // 隐藏整个 YES / NO 选择组
        gameObject.SetActive(false);
        if (womanTrigger != null)
        {
            womanTrigger.TriggerBriefcaseNoChoice();
        }
    }
}