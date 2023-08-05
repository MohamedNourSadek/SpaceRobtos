using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CreationPopUpView : View
{
    #region Public Variables
    public Image EntityImage;
    public TextMeshProUGUI EntityName;
    public TextMeshProUGUI EntityDescripition;
    public RequirementsUIManager requirements;
    public Button buildButton;
    public Button instantButton;
    public GameObject sliderParent;
    public Slider amountSlider;
    public TextMeshProUGUI amountText;

    public Action<int> onBuildPressed;
    #endregion

    #region Private Variables
    private List<requirement> cachedRequirement;
    #endregion

    #region Unity Delgates
    protected override void Start()
    {
        base.Start();

        amountSlider.onValueChanged.AddListener(OnSliderChanged);
        buildButton.onClick.AddListener(OnBuildPressed);
    }
    #endregion

    #region Public Functions
    public void ShowView(List<requirement> requirement, string entityName, string entityDescription, Action<int> OnBuild, bool ShowSlider = true)
    {
        IsRequirementsMet isRequirementsMet = GameManager.GameManager.DoIHaveResources(requirement);

        EntityImage.sprite = GameManager.DataManager.GetSprite(entityName, EntityImage);
        EntityName.text = entityName; 
        EntityDescripition.text = entityDescription;
        requirements.OrganizeRequirementsUi(isRequirementsMet);
        buildButton.interactable = isRequirementsMet.isMet;
        cachedRequirement = requirement;
        onBuildPressed += OnBuild;
        sliderParent.SetActive(ShowSlider);
        ShowView();
    }

    public override void HideView()
    {
       base.HideView();
       onBuildPressed = null;
    }
    #endregion

    #region Private Functions
    #endregion

    #region CallBacks and Coroutines
    private void OnSliderChanged(float amount)
    {
        amountText.text = ((int)amount).ToString();    
        List<requirement> newRequirements = new List<requirement>();

        foreach (var requirement in cachedRequirement)
        {
            if (requirement.requirementType == RequirementType.Building)
            {
                BuildingRequirement newRequirement = new BuildingRequirement();
                newRequirement.requirementType = RequirementType.Building;
                newRequirement.buildingType = ((BuildingRequirement) requirement).buildingType;
                newRequirement.requirementAmount = requirement.requirementAmount * ((int)amount);
                newRequirements.Add(newRequirement);
            }
            else
            {
                ResourceRequirement newRequirement = new ResourceRequirement();
                newRequirement.requirementType = RequirementType.Resource;
                newRequirement.resourceType = ((ResourceRequirement)requirement).resourceType;
                newRequirement.requirementAmount = requirement.requirementAmount * ((int)amount);
                newRequirements.Add(newRequirement);
            }
        }

        IsRequirementsMet isRequirementsMet = GameManager.GameManager.DoIHaveResources(newRequirements);
        
        buildButton.interactable = isRequirementsMet.isMet; 
        requirements.OrganizeRequirementsUi(isRequirementsMet);
    }
    private void OnBuildPressed()
    {
        onBuildPressed?.Invoke((int)amountSlider.value);
        HideView();
    }
    #endregion

}
