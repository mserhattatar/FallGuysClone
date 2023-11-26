using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Base;

namespace Game.Scripts
{
    public class LevelRanking
    {
        private readonly string _playerName;
        private readonly List<CharacterBase> _gameRank;


        protected internal LevelRanking(List<CharacterBase> characters, string playerName)
        {
            _playerName = playerName;
            _gameRank = characters;
        }

        /// <summary>
        /// </summary>
        /// <returns> string value is character rank name, int value is character rank index</returns>
        public (string, int)[] CharacterRanking()
        {
            List<(string, int)> rankResult = _gameRank
                .Where(x => x.CharacterGameCompletionInfo.Item1)
                .OrderBy(x => x.CharacterGameCompletionInfo.Item2)
                .Take(4)
                .Select(characterBase => (characterBase.CharacterName, characterBase.CharacterGameCompletionInfo.Item2))
                .ToList();

            if (rankResult.Count >= 4 && rankResult.Any(x => x.Item1 == _playerName))
                return rankResult.ToArray();

            var raceRanking =
                new List<CharacterBase>(_gameRank.Where(x => !x.CharacterGameCompletionInfo.Item1)
                    .OrderByDescending(c => c.transform.position.z));

            var resultCount = rankResult.Count;
            for (int i = 0; i < raceRanking.Count; i++)
            {
                var character = raceRanking[i];
                character.CharacterGameCompletionInfo =
                    new ValueTuple<bool, int>(character.CharacterGameCompletionInfo.Item1, resultCount + 1 + i);

                rankResult.Add((character.CharacterName, character.CharacterGameCompletionInfo.Item2));
            }

            var playerRaceIndex = rankResult.FirstOrDefault(x => x.Item1 == _playerName).Item2;


            if (playerRaceIndex <= 4) return rankResult.Take(4).ToArray();

            rankResult[3] = new ValueTuple<string, int>(_playerName, playerRaceIndex);

            return rankResult.ToArray();
        }
    }
}