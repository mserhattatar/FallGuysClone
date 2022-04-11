using System.Linq;
using UnityEngine;

public class LevelRanking
{
    private Transform[] characters;
    private readonly string playerName;

    public (string[], bool) CharacterRanking => FindCharacterRanking();


    protected internal LevelRanking(Transform[] characters, string playerName)
    {
        this.characters = characters;
        this.playerName = playerName;
    }

    private (string[], bool) FindCharacterRanking()
    {
        var result = new string[characters.Length];
        var isPlayerInThree = false;

        characters = characters.OrderByDescending(character => character.position.z).ToArray();

        for (var i = 0; i < characters.Length; i++)
        {
            result[i] = characters[i].name;

            if (i <= 3 && result[i] == playerName)
            {
                isPlayerInThree = true;
            }
        }

        return (result, isPlayerInThree);
    }
}