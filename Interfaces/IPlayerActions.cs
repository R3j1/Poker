using Poker.Enums;

namespace PokerConsoleApplication.Interfaces
{
    public interface IPlayerActions
    {
        bool ParticipatedInRound { get; set; }
        PlayerAction CurrentAction { get; set; }
        List<string> ActionsTakenLog { get; set; }
        void Fold();
        void Check();
        void Call(int currentBet);
        void Raise(int currentBet, int raiseAmount);
    }
}