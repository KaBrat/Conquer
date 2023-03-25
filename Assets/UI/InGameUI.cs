using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour, IProvinceDisplayer
{
    private Label PlayerLabel;
    private Label RoundLabel;
    private Label RegionNameLabel;
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
        this.RegionNameLabel = root.Q<Label>("RegionName");
        this.RegionNameLabel.visible = false;
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
        this.RoundLabel.text = gameManager.roundsManager.currentRound.ToString();
    }

    public void DisplayProvince(Province province)
    {
        if (province != null)
        {
            this.RegionNameLabel.visible = true;
            this.RegionNameLabel.text = province.Name;
        }

        else
            this.RegionNameLabel.visible = false;
    }
}