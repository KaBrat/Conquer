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
        var buttonTerrain = root.Q<Button>("Terrain");
        var buttonStates = root.Q<Button>("States");

        buttonNewMap.clicked += () => this.mapGenerator.GenerateMap();
        buttonTerrain.clicked += () => this.mapGenerator.ShowTerrain();
        buttonStates.clicked += () => this.mapGenerator.ShowProvinces();
    }
}
