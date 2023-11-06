using Poker.Enums;
using PokerConsoleApplication.Interfaces;

namespace Poker.Models
{
    public class Player : IPlayerActions
    {
        // todo: add Id property
        public string Name { get; set; }
        public Hand Hand { get; set; }
        public int Chips { get; set; }
        public bool ParticipatedInRound { get; set; }
        public int GamesWon { get; set; }
        public int GamesLost { get; set; }
        public DateTime PlayerCreated { get; set; } = DateTime.Now;
        public PlayerAction CurrentAction { get; set; }
        public int TotalBet { get; set; }
        public bool SessionActivePlayer { get; set; }
        public List<string> ActionsTakenLog { get; set; } = new List<string>();

        public Player(string name, int startingChips = 10000)
        {
            Name = name;
            Chips = startingChips;
            CurrentAction = PlayerAction.None;
            Hand = new Hand();
            TotalBet = 0;
        }

        public void Fold()
        {
            CurrentAction = PlayerAction.Fold;
            ActionsTakenLog.Add($"{Name} folds.");
        }

        public void Check()
        {
            CurrentAction = PlayerAction.Check;
            ActionsTakenLog.Add($"{Name} checks.");
        }

        public void Call(int currentBet)
        {
            if (Chips >= currentBet)
            {
                CurrentAction = PlayerAction.Call;
                Chips -= currentBet;
                TotalBet += currentBet;
                ActionsTakenLog.Add($"{Name} called {currentBet} chips.");
            }
            else
            {
                ActionsTakenLog.Add($"{Name} attempted to call but doesn't have enough chips.");
                CurrentAction = PlayerAction.Fold;
            }
        }

        public void Raise(int currentBet, int raiseAmount)
        {
            int totalAmountToRaise = currentBet + raiseAmount;

            if (Chips >= totalAmountToRaise)
            {
                CurrentAction = PlayerAction.Raise;
                Chips -= totalAmountToRaise;
                TotalBet += totalAmountToRaise;
                ActionsTakenLog.Add($"{Name} raised by {raiseAmount} chips.");
            }
            else
            {
                ActionsTakenLog.Add($"{Name} attempted to raise but doesn't have enough chips.");
            }
        }
    }
}