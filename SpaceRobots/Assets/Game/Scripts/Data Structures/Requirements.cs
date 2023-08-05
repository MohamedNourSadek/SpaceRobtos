using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class requirement
{
    public RequirementType requirementType;
    public int requirementAmount;
}
[System.Serializable]
public class ResourceRequirement : requirement
{
    public ResourceType resourceType;
}
[System.Serializable]
public class BuildingRequirement : requirement
{
    public BuildingType buildingType;
}

[System.Serializable]
public class IsRequirementMet
{
    public bool isMet;
    public System.Enum name;
    public int amountIHave;
    public int amountRequired;
}

[System.Serializable]
public class IsRequirementsMet
{
    public bool isMet;
    public List<IsRequirementMet> isRequirementsMet = new List<IsRequirementMet>();
}


public enum RequirementType
{
    Building, Resource
}
public enum BuildingType
{
    CommandCenter, RoboticsCenter, TechLab, EngineeringLab, Storage, Iron,
    Gold, Oil, Titanium, Uranium, ResourcesPlatform
}
public enum ResourceType
{
    Gold, Iron, Oil, Titanium, Uranium
}

public enum RobotsNames
{
    F1Hunter
}
