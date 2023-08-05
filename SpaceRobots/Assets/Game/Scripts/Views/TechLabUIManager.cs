using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechLabUIManager : BuildingButton
{
    #region Public Variables
    #endregion

    #region Private Variables
    protected override void Start()
    {
        base.Start();
    }
    #endregion

    #region Unity Delgates

    #endregion

    #region Public Functions
    protected override void LoadAndApplyData()
    {
        base.LoadAndApplyData();

        levelHandler.SetLevelsData(GetUpgradeInfo());
    }

    #endregion

    #region Private Functions
    private List<List<requirement>> GetUpgradeInfo()
    {
        var upgradeListRequirements = new List<List<requirement>>();

        for (int i = 0; i < 30; i++)
        {
            upgradeListRequirements.Add(
                new List<requirement>()
                {
                    new BuildingRequirement()
                    {
                        requirementType = RequirementType.Building,
                        buildingType = BuildingType.TechLab,
                        requirementAmount = i+1,
                    },
                    new ResourceRequirement()
                    {
                        requirementType = RequirementType.Resource,
                        resourceType = ResourceType.Iron,
                        requirementAmount = 6 + i
                    },
                    new ResourceRequirement()
                    {
                        requirementType = RequirementType.Resource,
                        resourceType = ResourceType.Oil,
                        requirementAmount = 4 + i
                    },
                    new ResourceRequirement()
                    {
                        requirementType = RequirementType.Resource,
                        resourceType = ResourceType.Titanium,
                        requirementAmount = 3 + i
                    }
                }
            );
        }

        return upgradeListRequirements;
    }
    #endregion

    #region CallBacks and Coroutines

    #endregion

}
