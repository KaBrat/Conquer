using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var buttonStart = root.Q<Button>("Start");
        var buttonQuit = root.Q<Button>("Quit");

        buttonStart.clicked += () => StartClicked();
        buttonQuit.clicked += () => QuitClicked();
    }

    private void StartClicked()
    {
        SceneManager.LoadScene(1);
    }

    private void QuitClicked()
    {
        Application.Quit();
    }
}
