using Domain.Interfaces;
using Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PollFileRepository : IPollRepository
    {
        private readonly string _filePath = "polls.json";

        private List<Poll> ReadPolls()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Poll>>(json);
        }

        private void WritePolls(List<Poll> polls)
        {
            var json = JsonConvert.SerializeObject(polls, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public void CreatePoll(Poll poll, Action<string> logger = null)
        {
            var polls = ReadPolls();
            poll.Id = polls.Any() ? polls.Max(p => p.Id) + 1 : 1;
            poll.DateCreated = DateTime.Now;
            polls.Add(poll);
            WritePolls(polls);
            logger?.Invoke($"Poll created with title: {poll.Title}");
        }

        public IEnumerable<Poll> GetPolls()
        {
            var polls = ReadPolls();
            return polls.OrderByDescending(p => p.DateCreated);
        }

        public void Vote(int pollId, int optionNumber)
        {
            var polls = ReadPolls();
            var poll = polls.FirstOrDefault(p => p.Id == pollId);
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
                WritePolls(polls);
            }
        }
    }
}
