using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MGUIScript : MonoBehaviour
{
    public MapGenerator mapGenerator;

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var buttonNewMap = root.Q<Button>("NewMap");

        buttonNewMap.clicked += () => NewMapClicked();
    }

    private void NewMapClicked()
    {
        mapGenerator.GenerateMap();
    }
}
