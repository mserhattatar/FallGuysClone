using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.Base
{
    public class LevelRanking
    {
        private readonly List<(string, int)> finalRank = new();

        private readonly string playerName;
        private List<Transform> gameRank;


        protected internal LevelRanking(IEnumerable<Transform> characters, string playerName)
        {
            this.playerName = playerName;
            gameRank = characters.ToList();
        }

        public (string, int)[] CharacterRanking => FindCharacterRanking();

        private (string, int)[] FindCharacterRanking()
        {
            gameRank = gameRank.OrderByDescending(c => c.transform.position.z).ToList();

            var findFinishedCharacter = gameRank.Where(character => !character.gameObject.activeInHierarchy)
                .Where(c => c.transform.position.z > 30).ToArray();

            foreach (var fTransform in findFinishedCharacter)
            {
                gameRank.Remove(fTransform);
                finalRank.Add((fTransform.gameObject.name, finalRank.Count + 1));
            }

            var result = new List<(string, int)>(finalRank);

            foreach (var character in gameRank) result.Add((character.gameObject.name, result.Count + 1));

            var player = result.Find(c => c.Item1 == playerName);

            if (player.Item2 > 4)
                result[3] = player;

            var firstFourCharacter = result.Take(4).ToArray();

            return firstFourCharacter;
        }
    }
}