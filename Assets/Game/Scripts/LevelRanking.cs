using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts
{
    public class LevelRanking
    {
        private readonly List<(string, int)> _finalRank = new();

        private readonly string _playerName;
        private List<Transform> _gameRank;


        protected internal LevelRanking(IEnumerable<Transform> characters, string playerName)
        {
            _playerName = playerName;
            _gameRank = characters.ToList();
        }

        public (string, int)[] CharacterRanking => FindCharacterRanking();

        private (string, int)[] FindCharacterRanking()
        {
            _gameRank = _gameRank.OrderByDescending(c => c.transform.position.z).ToList();

            var findFinishedCharacter = _gameRank.Where(character => !character.gameObject.activeInHierarchy)
                .Where(c => c.transform.position.z > 30).ToArray();

            foreach (var fTransform in findFinishedCharacter)
            {
                _gameRank.Remove(fTransform);
                _finalRank.Add((fTransform.gameObject.name, _finalRank.Count + 1));
            }

            var result = new List<(string, int)>(_finalRank);

            foreach (var character in _gameRank) result.Add((character.gameObject.name, result.Count + 1));

            var player = result.Find(c => c.Item1 == _playerName);

            if (player.Item2 > 4)
                result[3] = player;

            var firstFourCharacter = result.Take(4).ToArray();

            return firstFourCharacter;
        }
    }
}