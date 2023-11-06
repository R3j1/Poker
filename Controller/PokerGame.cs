using Poker.Enums;
using Poker.Interfaces;
using Poker.Models;

namespace Poker.Controller
{
    public class PokerGame : IPokerGame
    {
        private List<Player> players;
        private Dealer dealer;
        private SmallBlind smallBlind;
        private BigBlind bigBlind;
        private Pot pot = new Pot();
        private int smallBlindAmount = 1;
        private int bigBlindAmount = 3;
        private int currentBet;
        private List<Card> communityCards = new List<Card>();

        public bool RevealPlayerCards { get; set; }
        public string GameStatusPrompt { get; set; } = "Let The Game Begin";
        public bool GamePlayActive { get; set; }

        public PokerGame()
        {
            players = new List<Player>();
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public List<Player> GetPlayers()
        {
            return players;
        }

        public void ShuffleDeck()
        {
            dealer.Deck.Shuffle();
            dealer.Player.ActionsTakenLog.Add("shuffled cards");
        }

        public void DealPlayerCards()
        {
            foreach (Player player in players)
            {
                for (int i = 0; i < 2; i++)
                {
                    Card card = dealer.Deck.DealCard();
                    player.Hand.AddCard(card);
                }
            }
            dealer.Player.ActionsTakenLog.Add("dealt player cards");
        }

        public void GetFlopCards()
        {
            for (int i = 0; i < 3; i++)
            {
                communityCards.Add(dealer.Deck.DealCard());
            }
            dealer.Player.ActionsTakenLog.Add("Got flop cards");
        }

        public List<Card> GetCommunityCards()
        {
            return communityCards;
        }

        public void GetTurnCard()
        {
            communityCards.Add(dealer.Deck.DealCard());
            dealer.Player.ActionsTakenLog.Add("Got turn cards");
        }

        public void GetRiverCard()
        {
            communityCards.Add(dealer.Deck.DealCard());
            dealer.Player.ActionsTakenLog.Add("Got river cards");
        }

        public void GivePotLootToWinner(Player player)
        {
            // todo: if draw split the pot loot between the players
            player.Chips += pot.PotTotal;
            pot.PotTotal = 0;
        }

        public void SetDealer()
        {
            dealer = new Dealer(players[0]);
        }

        public void SetSmallBinder()
        {
            smallBlind = new SmallBlind(players[1]);
        }

        public void SetBigBinder()
        {
            bigBlind = new BigBlind(players[2]);
        }

        public void GetSmallBlind()
        {
            smallBlind.ContributeSmallBind(smallBlindAmount);
            pot.AddToPot(smallBlindAmount);
        }

        public void GetBigBlind()
        {
            bigBlind.ContributeBigBind(bigBlindAmount);
            pot.AddToPot(bigBlindAmount);
            currentBet = bigBlindAmount;
        }

        public void UpdateCurrentBet(int newBetAmount)
        {
            currentBet = newBetAmount;
        }

        public int GetCurrentBetAmount()
        {
            return currentBet;
        }

        public void JoinPlayerCardsToCommunityCards()
        {
            foreach (Player p in players)
            {
                foreach (Card c in communityCards)
                {
                    p.Hand.AddCard(c);
                }
            }
        }

        public Player DecideWinner()
        {
            List<HandRank> ranks = new List<HandRank>();

            for (int i = 0; i < players.Count; i++)
            {
                HandRank playerRank = players[i].CurrentAction != PlayerAction.Fold ? players[i].Hand.EvaluateHand() : HandRank.HighCard;
                ranks.Add(playerRank);
            }

            // Find the highest rank among all players
            HandRank highestRank = ranks.Max();

            // Find the players with the highest rank
            List<Player> winningPlayers = players.Where((player, index) => ranks[index] == highestRank).ToList();

            if (winningPlayers.Count == 1)
            {
                winningPlayers[0].ActionsTakenLog.Add($"{winningPlayers[0].Name} is the winner with {highestRank}.");
            }
            else
            {
                foreach (Player winningPlayer in winningPlayers)
                {
                    winningPlayer.ActionsTakenLog.Add($"{winningPlayer.Name} wins in a tied");
                }
            }
            return winningPlayers[0];
        }

        public int GetPotTotal()
        {
            return pot.PotTotal;
        }

        public void ManageBettingRound(PlayerAction action, Player player, int amount = 0)
        {
            player.ParticipatedInRound = true;
            switch (action)
            {
                case PlayerAction.Fold:
                    player.Fold();
                    break;

                case PlayerAction.Check:
                    player.Check();
                    break;

                case PlayerAction.Call:
                    int currentBet = GetCurrentBetAmount();
                    player.Call(currentBet);
                    AddToPot(currentBet);
                    break;

                case PlayerAction.Raise:
                    player.Raise(GetCurrentBetAmount(), amount);
                    int total = GetCurrentBetAmount() + amount;
                    UpdateCurrentBet(total);
                    AddToPot(total);
                    RestartBetting();
                    break;
            }
        }

        public void ManageBettingRoundEnded()
        {
            foreach (Player player in players)
            {
                player.ParticipatedInRound = false;
            }
            UpdateCurrentBet(0);
        }

        public void RestartBetting()
        {
            // someone raised so all other players who have not folded need to call or fold
            foreach (Player player in players)
            {
                if (player.CurrentAction != PlayerAction.Fold)
                {
                    player.ParticipatedInRound = false;
                }
            }
        }

        public void ResetGameSettings()
        {
            RevealPlayerCards = false;
            dealer.Deck.ResetDeck();
            communityCards.Clear();
            foreach (Player player in players)
            {
                player.Hand.Clear();
                player.ActionsTakenLog.Clear();
                player.CurrentAction = PlayerAction.None;
                player.TotalBet = 0;
            }
            UpdateCurrentBet(0);
        }

        public void UpdatePlayerRoles()
        {
            Player lastPlayer = players[players.Count - 1];

            for (int i = players.Count - 1; i >= 1; i--)
            {
                players[i] = players[i - 1];
            }

            players[0] = lastPlayer;

            dealer = new Dealer(players[0]);
            smallBlind = new SmallBlind(players[1]);
            bigBlind = new BigBlind(players[2]);
        }

        public void AddToPot(int amount)
        {
            pot.PotTotal += amount;
        }
    }
}