using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour, IProvinceDisplayer
{
    private Label PlayerLabel;
    private Label RoundLabel;
    private VisualElement ProvinceDescription;
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

        buttonTerrain.clicked += gameManager.MapManager.ShowTerrain;
        buttonProvinces.clicked += gameManager.MapManager.ShowProvinces;

        this.PlayerLabel = root.Q<Label>("PlayerValue");
        this.RoundLabel = root.Q<Label>("RoundValue");
        this.ProvinceDescription = root.Q("ProvinceDescription");
        this.ProvinceNameLabel = root.Q<Label>("ProvinceName");
        this.ProvinceOwnerLabel = root.Q<Label>("ProvinceOwner");
        this.ProvinceFootmenLabel = root.Q<Label>("FootmenLabel");
        this.ProvinceFootmenValue = root.Q<Label>("FootmenValue");

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
            this.ShowProvince(province);
        }
        else
        {
            this.HideProvinceInfo();
        }
    }

    private void ShowProvince(Province province)
    {
        this.ProvinceDescription.pickingMode = PickingMode.Position;

        this.ProvinceNameLabel.visible = true;
        this.ProvinceNameLabel.text = province.Name;
        this.ProvinceNameLabel.pickingMode = PickingMode.Position;

        this.ProvinceOwnerLabel.visible = true;
        this.ProvinceOwnerLabel.text = province.Owner != null ? $"Player {province.Owner.id}" : "none";
        this.ProvinceOwnerLabel.pickingMode = PickingMode.Position;

        ProvinceFootmenLabel.visible = true;
        this.ProvinceFootmenValue.visible = true;
        this.ProvinceFootmenValue.text = province.FootmenCount.ToString();
        this.ProvinceFootmenValue.pickingMode = PickingMode.Position;
    }

    private void HideProvinceInfo()
    {
        this.ProvinceDescription.pickingMode = PickingMode.Ignore;

        this.ProvinceNameLabel.visible = false;
        this.ProvinceNameLabel.pickingMode = PickingMode.Ignore;
        this.ProvinceOwnerLabel.visible = false;
        this.ProvinceOwnerLabel.pickingMode = PickingMode.Ignore;
        this.ProvinceFootmenLabel.visible = false;
        this.ProvinceFootmenLabel.pickingMode = PickingMode.Ignore;
        this.ProvinceFootmenValue.visible = false;
        this.ProvinceFootmenValue.pickingMode = PickingMode.Ignore;
    }
}