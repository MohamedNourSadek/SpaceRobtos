using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoutResultUI : View
{
    #region Public Variables
    public Button backgroundClickable;
    public Button closeBottomButton;

    public TextMeshProUGUI PlayerNameText;
    public TextMeshProUGUI GoldAmountText;
    public TextMeshProUGUI IronAmountText;
    public TextMeshProUGUI OilAmountText;
    public TextMeshProUGUI TitaniumAmountText;
    public TextMeshProUGUI UraniumAmountText;

    public Image CharacterImage;
    public Slider GoldSlider;
    public Slider IronSlider;
    public Slider OilSlider;
    public Slider TitaniumSlider;
    public Slider UraniumSlider;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();

        backgroundClickable.onClick.AddListener(HideView);
        closeBottomButton.onClick.AddListener(HideView);
    }
    #endregion

    #region Public Functions
    public void ShowView(CharacterData characterData)
    {
        PlayerNameText.text = characterData.playerName;

        GoldAmountText.text = Divide(characterData.goldAmount, 1000).ToString() + "k";
        IronAmountText.text = Divide(characterData.ironAmount, 1000).ToString() + "k";
        OilAmountText.text = Divide(characterData.oilAmount, 1000).ToString() + "k";
        TitaniumAmountText.text = Divide(characterData.titaniumAmount, 1000).ToString() + "k";
        UraniumAmountText.text = Divide(characterData.uraniumAmount, 1000).ToString() + "k";

        IronSlider.maxValue = CharacterData.MAX_IRON;
        GoldSlider.maxValue = CharacterData.MAX_GOLD;
        OilSlider.maxValue = CharacterData.MAX_OIL;
        TitaniumSlider.maxValue = CharacterData.MAX_TIE;
        UraniumSlider.maxValue = CharacterData.MAX_URANIUM;

        GoldSlider.value = characterData.goldAmount;
        IronSlider.value = characterData.ironAmount;
        OilSlider.value = characterData.oilAmount;
        TitaniumSlider.value = characterData.titaniumAmount;
        UraniumSlider.value = characterData.uraniumAmount;

        CharacterImage.sprite = GameManager.instance.GetCharacterImage(characterData.characterImage);

        ShowView();
    }
    #endregion

    #region Private Functions
    private float Divide(int amount, int byAmount)
    {
        return (((float)amount) / byAmount);
    }

    #endregion

    #region CallBacks and Coroutines

    #endregion

}
