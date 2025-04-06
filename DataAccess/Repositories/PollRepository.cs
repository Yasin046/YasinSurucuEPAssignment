using DataAccess.DataContext;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PollRepository : IPollRepository
    {
        private readonly PollDbContext _context;

        public PollRepository(PollDbContext context)
        {
            _context = context;
        }


        public void CreatePoll(Poll poll, Action<string> logger = null)
        {
            poll.DateCreated = DateTime.Now;
            _context.Polls.Add(poll);
            _context.SaveChanges();
            logger?.Invoke($"Poll created with title: {poll.Title}");
        }

        public IEnumerable<Poll> GetPolls()
        {
            return _context.Polls.OrderByDescending(p => p.DateCreated).ToList();
        }

        public void Vote(int pollId, int optionNumber)
        {
            var poll = _context.Polls.FirstOrDefault(p => p.Id == pollId);
            if (poll != null)
            {
                switch (optionNumber)
                {
                    case 1:
                        poll.Option1VotesCount++;
                        break;
                    case 2:
                        poll.Option2VotesCount++;
                        break;
                    case 3:
                        poll.Option3VotesCount++;
                        break;
                    default:
                        break;
                }
                _context.SaveChanges();
            }
        }
    }
}
