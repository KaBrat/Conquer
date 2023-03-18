using UnityEngine;
using UnityEngine.UIElements;

public class MenuUI : MonoBehaviour
{
    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var buttonQuit = root.Q<Button>("Quit");

        buttonQuit.clicked += () => Quit();
    }

    private void Quit()
    {
        Application.Quit();
    }
}
