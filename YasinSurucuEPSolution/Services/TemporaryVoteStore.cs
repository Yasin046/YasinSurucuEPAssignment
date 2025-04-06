using Domain.Interfaces;
using System.Collections.Concurrent;

namespace YasinSurucuEPSolution.Services
{
    public class TemporaryVoteStore : ITemporaryVoteStore
    {
        private readonly ConcurrentDictionary<string, HashSet<int>> _voteRecords = new ConcurrentDictionary<string, HashSet<int>>();

        public bool HasVoted(string userId, int pollId)
        {
            if (_voteRecords.TryGetValue(userId, out var polls))
            {
                return polls.Contains(pollId);
            }
            return false;
        }

        public void RecordVote(string userId, int pollId)
        {
            var polls = _voteRecords.GetOrAdd(userId, new HashSet<int>());
            polls.Add(pollId);
        }
    }
}
