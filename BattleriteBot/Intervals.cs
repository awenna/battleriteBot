using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;
using System;

namespace BattleriteBot
{
    public static class Intervals
    {
        public static (ImmutableList<Signing>, DateTime?, DateTime?) 
            FindOverlap(IEnumerable<Signing> signings, int minimumRequired)
        {
            var sorted = signings.OrderBy(_ => _.Start).ToArray();
            var found = ImmutableList.Create<Signing>();

            int i = 0;
            while (i < sorted.Length && found.Count < minimumRequired)
            {
                var current = sorted[i];
                found = ImmutableList.Create(sorted[i]);

                foreach (var sort in sorted.TakeLast(sorted.Length - i - 1))
                {
                    var cEnd = current.End ?? DateTime.MaxValue;
                    var sStart = sort.Start ?? DateTime.MinValue;
                    var sEnd = sort.End ?? DateTime.MaxValue;

                    if ((cEnd - sStart).TotalMinutes < 30
                        || (sEnd - sStart).TotalMinutes < 30 ) break;
                    found = found.Add(sort);
                    if (found.Count >= minimumRequired) break;
                }
                i++;
            }
            if (found.Count < minimumRequired) return (ImmutableList.Create<Signing>(), null, null);

            var startDT = found.OrderBy(_ => _.Start ?? DateTime.MinValue).Last().Start;
            var endDT = found.OrderBy(_ => _.End ?? DateTime.MaxValue).First().End;

            var start = startDT == DateTime.MinValue ? null : startDT;
            var end = endDT == DateTime.MaxValue ? null : endDT;

            return (found, start, end);
        }
    }
}
