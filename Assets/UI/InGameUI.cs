using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    public Label PlayerLabel;
    public Label RoundLabel;

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
        gameManager.EndTurn();
        var currentPlayer = gameManager.GetCurrentPlayer();
        this.PlayerLabel.text = currentPlayer.id.ToString();
        this.RoundLabel.text = gameManager.Round.ToString();
    }
}