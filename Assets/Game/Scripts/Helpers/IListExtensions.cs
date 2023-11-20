using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Helpers
{
    public static class ListExtensions
    {
        /// <summary>
        ///     Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = Random.Range(i, count);
                (ts[i], ts[r]) = (ts[r], ts[i]);
            }
        }
    }
}