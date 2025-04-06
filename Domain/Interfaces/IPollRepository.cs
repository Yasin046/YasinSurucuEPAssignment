using Domain.Models;

namespace Domain.Interfaces
{
    public interface IPollRepository
    {
        void CreatePoll(Poll poll, Action<string> logger = null);
        IEnumerable<Poll> GetPolls();
        void Vote(int pollId, int optionNumber);
    }
}
