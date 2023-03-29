using System.Net;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
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

        buttonMenu.clicked += () => MenuClicked();
        buttonEndTurn.clicked += () => EndTurnClicked();

        this.PlayerLabel = root.Q<Label>("PlayerValue");
        this.RoundLabel = root.Q<Label>("RoundValue");
        this.ProvinceNameLabel = root.Q<Label>("ProvinceName");
        this.ProvinceNameLabel.visible = false;

        this.ProvinceOwnerLabel = root.Q<Label>("ProvinceOwner");
        this.ProvinceOwnerLabel.visible = false;

        this.ProvinceFootmenLabel = root.Q<Label>("FootmenLabel");
        this.ProvinceFootmenLabel.visible = false;

        this.ProvinceFootmenValue = root.Q<Label>("FootmenValue");
        this.ProvinceFootmenValue.visible = false;
    }

    private void MenuClicked()
    {
        SceneManager.LoadScene(0);
    }

    private void EndTurnClicked()
    {
        var (peviousPlayer, currentPlayer) = gameManager.EndTurn();
        if (currentPlayer != null)
        {
            this.PlayerLabel.text = currentPlayer.id.ToString();
        }
        this.RoundLabel.text = gameManager.RoundsManager.currentRound.ToString();
    }

    public void DisplayProvince(Province province)
    {
        if (province != null)
        {
            this.ProvinceNameLabel.visible = true;
            this.ProvinceNameLabel.text = province.Name;

            this.ProvinceOwnerLabel.visible = true;
            if (province.Owner != null)
                this.ProvinceOwnerLabel.text = $"Player {province.Owner.id}";
            else
                this.ProvinceOwnerLabel.text = "none";

            this.ProvinceFootmenLabel.visible = true;
            this.ProvinceFootmenValue.visible = true;
            this.ProvinceFootmenValue.text = province.FootmenCount.ToString();
        }

        else
        {
            this.ProvinceNameLabel.visible = false;
            this.ProvinceOwnerLabel.visible = false;
            this.ProvinceFootmenLabel.visible = false;
            this.ProvinceFootmenValue.visible = false;
        }

    }
}