using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequirementsUIManager : MonoBehaviour
{
    public List<RequirementUI> requirementsUI;
 
    public void OrganizeRequirementsUi(IsRequirementsMet isRequirementsMet)
    {
        foreach (RequirementUI requirementUi in requirementsUI)
            Destroy(requirementUi.gameObject);

        requirementsUI.Clear();

        var prefab = Resources.Load<GameObject>("Prefabs/ResourceElement");
        RequirementUI requirementUiObject = prefab.GetComponent<RequirementUI>();

        foreach (IsRequirementMet requirement in isRequirementsMet.isRequirementsMet)
        {
            RequirementUI requirementUI = Instantiate(requirementUiObject.gameObject, this.transform).GetComponent<RequirementUI>();

            requirementUI.amountIHave.text = requirement.amountIHave.ToString();
            requirementUI.requirementAmount.text = requirement.amountRequired.ToString();
            requirementUI.requirementName.text = requirement.name.ToString();
            requirementUI.amountIHave.color = requirement.isMet ? Color.green : Color.red;

            requirementsUI.Add(requirementUI);
        }
    }
}
