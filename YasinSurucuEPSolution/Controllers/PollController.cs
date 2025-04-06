using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YasinSurucuEPSolution.ActionFilters;

namespace YasinSurucuEPSolution.Controllers
{
    public class PollController : Controller
    {
        private readonly IPollRepository _pollRepository;

        public PollController(IPollRepository pollRepository)
        {
            _pollRepository = pollRepository;
        }

        public IActionResult Index()
        {
            var polls = _pollRepository.GetPolls();
            return View(polls);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Poll poll, [FromServices] ILogger<PollController> logger)
        {
            if (ModelState.IsValid)
            {

                _pollRepository.CreatePoll(poll, msg => logger.LogInformation(msg));
                return RedirectToAction("Index");
            }
            return View(poll);
        }

        public IActionResult Details(int id)
        {
            var poll = _pollRepository.GetPolls().FirstOrDefault(p => p.Id == id);
            if (poll == null)
            {
                return NotFound();
            }
            return View(poll);
        }

        [EnsureLoggedIn]
        public IActionResult Vote(int id)
        {
            var poll = _pollRepository.GetPolls().FirstOrDefault(p => p.Id == id);
            if (poll == null)
            {
                return NotFound();
            }
            return View(poll);
        }

        [HttpPost]
        [EnsureLoggedIn]
        public IActionResult Vote(int pollId, int selectedOption, [FromServices] ITemporaryVoteStore voteStore)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) 
            {
                return NotFound("User wan not found.");
            }

            if (voteStore.HasVoted(userId, pollId))
            {
                TempData["Error"] = "You have already voted in this poll.";
                return RedirectToAction("Details", new { id = pollId });
            }

            _pollRepository.Vote(pollId, selectedOption);

            voteStore.RecordVote(userId, pollId);

            return RedirectToAction("Details", new { id = pollId });
        }
    }
}
