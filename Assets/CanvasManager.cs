using System;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private Transform[] allCharacters;
    [SerializeField] private TextMeshProUGUI[] rankingTexts;

    private LevelRanking levelRanking;

    public string PlayerName { get; private set; }

    private void Awake()
    {
        PlayerName = "Player";
        levelRanking = new LevelRanking(allCharacters, PlayerName);
    }

    private void LateUpdate()
    {
        //if the player not in the top three than set last ranking texts bar for player 

        var (characterNames, isPlayerInTheTopThree) = levelRanking.CharacterRanking;

        var lenght = 3;
        if (isPlayerInTheTopThree)
            lenght = 4;
        else
        {
            var playerIndex = Array.IndexOf(characterNames, PlayerName);
            rankingTexts[rankingTexts.Length - 1].text = $"{playerIndex + 1}. {PlayerName}";
        }

        for (var i = 0; i < lenght; i++)
        {
            rankingTexts[i].text = $"{i + 1}. {characterNames[i]}";
        }
    }
}