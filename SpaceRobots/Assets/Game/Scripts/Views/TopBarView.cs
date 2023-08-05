using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBarView : View
{
    #region Public Variables
    public Button GemButton;
    public Button VIPButton;

    public TextMeshProUGUI PlayerNameText;
    public TextMeshProUGUI PowerNumberText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI VIPLevelText;
    public TextMeshProUGUI GemAmountText;
    public TextMeshProUGUI GoldAmountText;
    public TextMeshProUGUI IronAmountText;
    public TextMeshProUGUI OilAmountText;
    public TextMeshProUGUI TitaniumAmountText;
    public TextMeshProUGUI UraniumAmountText;

    public Image CharacterImage;
    public Slider LevelSlider;
    public Slider VIPLevelSlider;
    public Slider EnergySlider;
    public Slider GoldSlider;
    public Slider IronSlider;
    public Slider OilSlider;
    public Slider TitaniumSlider;
    public Slider UraniumSlider;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Delgates

    #endregion

    #region Public Functions
    public override void ShowView()
    {
        base.ShowView();
    }
    #endregion

    #region Private Functions
    public void RefreshPlayerInformation(CharacterData characterData)
    {
        PlayerNameText.text = characterData.playerName;
        PowerNumberText.text = characterData.powerNumber.ToString();
        LevelText.text = "Lv" + (characterData.xp/CharacterData.MAX_XP).ToString();
        VIPLevelText.text = "(Lv." + characterData.GetVIPLevel().ToString() + ")";
        GemAmountText.text = characterData.gemAmount.ToString();

        GoldAmountText.text = Divide(characterData.goldAmount,1000).ToString() + "k";
        IronAmountText.text = Divide(characterData.ironAmount, 1000).ToString() + "k";
        OilAmountText.text = Divide(characterData.oilAmount, 1000).ToString() + "k";
        TitaniumAmountText.text = Divide(characterData.titaniumAmount, 1000).ToString() + "k";
        UraniumAmountText.text = Divide(characterData.uraniumAmount, 1000).ToString() + "k";

        LevelSlider.maxValue = CharacterData.MAX_XP;
        VIPLevelSlider.maxValue = CharacterData.MAX_VIP_POINTS;
        EnergySlider.maxValue = CharacterData.MAX_ENERGY;
        IronSlider.maxValue = CharacterData.MAX_IRON;
        GoldSlider.maxValue = CharacterData.MAX_GOLD;
        OilSlider.maxValue = CharacterData.MAX_OIL;
        TitaniumSlider.maxValue = CharacterData.MAX_TIE;
        UraniumSlider.maxValue = CharacterData.MAX_URANIUM;

        LevelSlider.value = characterData.xp - ((characterData.xp / CharacterData.MAX_XP) * CharacterData.MAX_XP);
        VIPLevelSlider.value = characterData.vipPoints - ((characterData.vipPoints / CharacterData.MAX_VIP_POINTS) * CharacterData.MAX_VIP_POINTS);
        EnergySlider.value = characterData.energyAmount;
        GoldSlider.value = characterData.goldAmount;
        IronSlider.value = characterData.ironAmount;
        OilSlider.value = characterData.oilAmount;
        TitaniumSlider.value = characterData.titaniumAmount;
        UraniumSlider.value = characterData.uraniumAmount;

        CharacterImage.sprite = GameManager.instance.GetCharacterImage(characterData.characterImage);

        GemButton.onClick.AddListener(View.GetViewGlobally<GEMShopUIView>().ShowView);
        VIPButton.onClick.AddListener(View.GetViewGlobally<VIPUIManager>().ShowView);
    }

    private float Divide(int amount, int byAmount)
    {
        return (((float)amount) / byAmount);
    }
    #endregion

    #region CallBacks and Coroutines

    #endregion

}
