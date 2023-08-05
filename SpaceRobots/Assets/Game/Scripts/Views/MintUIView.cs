using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MintUIView : ResourceStructureView
{
    protected override void Awake()
    {
        base.Awake();

        if (GameManager.DataManager.characterData.buildingsLevels.ContainsKey(MyID) == false)
        {
            toBeBuilt = BuildingType.Gold;
            ShowOnBuild = false;
            OnBuildingBuilt();
        }
    }
}
