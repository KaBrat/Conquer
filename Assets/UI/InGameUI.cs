using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour, IProvinceDisplayer
{
    private Label PlayerLabel;
    private Label RoundLabel;
    private Label ProvinceNameLabel;
    private Label ProvinceOwnerLabel;
    private Label ProvinceFootmenLabel;
    private Label ProvinceFootmenValue;
    public GameManager gameManager;

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var buttonMenu = root.Q<Button>("Menu");
        var buttonEndTurn = root.Q<Button>("EndTurn");

        var buttonTerrain = root.Q<Button>("Terrain");
        var buttonProvinces = root.Q<Button>("Provinces");

        buttonMenu.clicked += () => SceneManager.LoadScene(0);
        buttonEndTurn.clicked += EndTurnClicked;

        buttonTerrain.clicked += gameManager.ProvincesMap.ShowTerrain;
        buttonProvinces.clicked += gameManager.ProvincesMap.ShowProvinces;

        PlayerLabel = root.Q<Label>("PlayerValue");
        RoundLabel = root.Q<Label>("RoundValue");
        ProvinceNameLabel = root.Q<Label>("ProvinceName");
        ProvinceOwnerLabel = root.Q<Label>("ProvinceOwner");
        ProvinceFootmenLabel = root.Q<Label>("FootmenLabel");
        ProvinceFootmenValue = root.Q<Label>("FootmenValue");

        HideProvinceInfo();
    }

    private void EndTurnClicked()
    {
        var (previousPlayer, currentPlayer) = gameManager.EndTurn();
        if (currentPlayer != null)
        {
            PlayerLabel.text = currentPlayer.id.ToString();
        }
        RoundLabel.text = gameManager.RoundsManager.currentRound.ToString();
    }

    public void DisplayProvince(Province province)
    {
        if (province != null)
        {
            ProvinceNameLabel.visible = true;
            ProvinceNameLabel.text = province.Name;

            ProvinceOwnerLabel.visible = true;
            ProvinceOwnerLabel.text = province.Owner != null ? $"Player {province.Owner.id}" : "none";

            ProvinceFootmenLabel.visible = true;
            ProvinceFootmenValue.visible = true;
            ProvinceFootmenValue.text = province.FootmenCount.ToString();
        }
        else
        {
            HideProvinceInfo();
        }
    }

    private void HideProvinceInfo()
    {
        ProvinceNameLabel.visible = false;
        ProvinceOwnerLabel.visible = false;
        ProvinceFootmenLabel.visible = false;
        ProvinceFootmenValue.visible = false;
    }
}