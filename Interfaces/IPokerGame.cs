using Poker.Enums;
using Poker.Models;

namespace Poker.Interfaces
{
    public interface IPokerGame : ITexasHoldemRules
    {
        string GameStatusPrompt { get; set; }
        void AddPlayer(Player player);
        List<Player> GetPlayers();
        void ShuffleDeck();
        void DealPlayerCards();
        void UpdateCurrentBet(int newBetAmount);
        int GetCurrentBetAmount();
        int GetPotTotal();
        void AddToPot(int amount);
        void ManageBettingRound(PlayerAction playerAction, Player player, int amount = 0);
        void ManageBettingRoundEnded();
        void RestartBetting();
        void GetFlopCards();
        List<Card> GetCommunityCards();
        void GetTurnCard();
        void GetRiverCard();
        bool RevealPlayerCards { get; set; }
        void GivePotLootToWinner(Player player);
        void ResetGameSettings();
        void UpdatePlayerRoles();
        bool GamePlayActive { get; set; }
    }
}