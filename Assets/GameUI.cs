using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var buttonAttack = root.Q<Button>("AttackButton");

        buttonAttack.clicked += () => DoSomething();
    }

    private void DoSomething()
    {
        Debug.Log("Test");
    }
}
