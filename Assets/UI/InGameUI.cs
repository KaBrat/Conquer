using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    private Label PlayerLabel;
    private Label RoundLabel;
    public GameManager gameManager;
    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var buttonMenu = root.Q<Button>("Menu");
        var buttonEndTurn = root.Q<Button>("EndTurn");

        PlayerLabel = root.Q<Label>("PlayerValue");
        RoundLabel = root.Q<Label>("RoundValue");

        buttonMenu.clicked += () => MenuClicked();
        buttonEndTurn.clicked += () => EndTurnClicked();
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
}