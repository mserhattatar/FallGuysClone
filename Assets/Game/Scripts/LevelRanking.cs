using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Base;

namespace Game.Scripts
{
    public class LevelRanking
    {
        private readonly List<(string, int)> _finalRank = new();

        private readonly string _playerName;
        private readonly List<CharacterBase> _gameRank;


        protected internal LevelRanking(List<CharacterBase> characters, string playerName)
        {
            _playerName = playerName;
            _gameRank = characters;
        }

        public (string, int)[] CharacterRanking()
        {
           var gameRank = _gameRank.OrderByDescending(c => c.transform.position.z).ToList();

            var findFinishedCharacter = gameRank.Where(character => !character.gameObject.activeInHierarchy)
                .Where(c => c.transform.position.z > 30).ToArray();

            foreach (var fTransform in findFinishedCharacter)
            {
                gameRank.Remove(fTransform);
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