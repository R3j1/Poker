using Poker.Models;

namespace Poker.Interfaces
{
    public interface ITexasHoldemRules
    {
        void SetDealer();
        void SetSmallBinder();
        void SetBigBinder();
        void GetSmallBlind();
        void GetBigBlind();
        Player DecideWinner();
    }
}