using Newtonsoft.Json;
using UnityEngine;
using Zork.Common;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    UnityInputService Input;

    [SerializeField]
    UnityOutputService Output;

    [SerializeField]
    TextMeshProUGUI LocationText;

    [SerializeField]
    TextMeshProUGUI ScoreText;

    [SerializeField]
    TextMeshProUGUI MovesText;

    private void Awake()
    {
        TextAsset gameJson = Resources.Load<TextAsset>("GameJson");
        _game = JsonConvert.DeserializeObject<Game>(gameJson.text);

        _game.Run(Input, Output);
    }

    private Game _game;
}
