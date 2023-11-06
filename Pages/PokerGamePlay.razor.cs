using Microsoft.AspNetCore.Components;
using Poker.Enums;
using Poker.Models;

namespace Poker.Pages
{
    public partial class PokerGamePlay
    {
        private Player sessionPlayer = new Player("");
        private string raiseAmount = "";
        private bool isRaiseButtonDisabled = true;
        private bool sessionPlayersTurn;

        protected override async Task OnInitializedAsync()
        {
            gameplay.GamePlayActive = true;
            sessionPlayer = gameplay.GetPlayers().FirstOrDefault(player => player.SessionActivePlayer == true);
            await SetupInitialPlayerRoles();
            await RunPokerGame();
        }

        private async Task SetupInitialPlayerRoles()
        {
            await ShowStatusMessage("Setting Up Dealer");
            gameplay.SetDealer();

            await ShowStatusMessage("Setting Up Small Blind");
            gameplay.SetSmallBinder();

            await ShowStatusMessage("Setting Up Big Blind");
            gameplay.SetBigBinder();
        }

        private async Task InitialSetup()
        {
            await ShowStatusMessage("Getting chip(s) from small blind");
            gameplay.GetSmallBlind();

            await ShowStatusMessage("Getting chips from big blind");
            gameplay.GetBigBlind();

            await ShowStatusMessage("Dealer shuffling cards");
            gameplay.ShuffleDeck();

            await ShowStatusMessage("Dealer dealing cards");
            gameplay.DealPlayerCards();
        }

        private async Task GameInProgressProcesses()
        {
            int bettingRounds = 4;

            for (int i = 1; i <= bettingRounds; i++)
            {
                await ShowStatusMessage($"Started betting round {i}");
                await ManageBettingRound();
                gameplay.ManageBettingRoundEnded();

                switch (i)
                {
                    case 1:
                        await ShowStatusMessage("Dealer Getting Flop Cards");
                        gameplay.GetFlopCards();
                        break;
                    case 2:
                        await ShowStatusMessage("Dealer Getting Turn Card");
                        gameplay.GetTurnCard();
                        break;
                    case 3:
                        await ShowStatusMessage("Dealer Getting River Card");
                        gameplay.GetRiverCard();
                        break;
                    default:
                        break;
                }
            }
        }
        private async Task GamePlayEndedProcesses()
        {
            await ShowStatusMessage("Evaluating showdown winner");
            Player winner = gameplay.DecideWinner();
            await ShowStatusMessage($"The winner is {winner.Name}!");

            await ShowStatusMessage($"Distributing pot loot to {winner.Name}");
            gameplay.GivePotLootToWinner(winner);

            await ShowStatusMessage("Revealing active players cards");
            gameplay.RevealPlayerCards = true;

            await ShowStatusMessage("Updating players stats");
            UpdatePlayersStats();

            await ShowStatusMessage("Reached end of game session");
            gameplay.GamePlayActive = false;
        }

        private async Task RunPokerGame()
        {
            await InitialSetup();
            await GameInProgressProcesses();
            await GamePlayEndedProcesses();
        }

        private async Task ShowStatusMessage(string message)
        {
            int delay = 2500;
            gameplay.GameStatusPrompt = message;
            StateHasChanged();
            await Task.Delay(delay);
        }

        private async Task PlayAgain()
        {
            gameplay.GamePlayActive = true;
            await ShowStatusMessage("Setting up for new game");
            gameplay.ResetGameSettings();

            await ShowStatusMessage("Updating player roles");
            gameplay.UpdatePlayerRoles();

            await RunPokerGame();
        }

        private string GetCurrentPlayersFirstCard()
        {
            if (gameplay.GetPlayers().Count <= 0 || gameplay.GetPlayers()[0].Hand.GetCards().Count <= 0)
            {
                return string.Empty;
            }
            else
            {
                return sessionPlayer.Hand.GetCards()[0].CardImage;
            }
        }

        private string GetCurrentPlayersSecondCard()
        {
            if (gameplay.GetPlayers().Count <= 0 || gameplay.GetPlayers()[0].Hand.GetCards().Count <= 0)
            {
                return string.Empty;
            }
            else
            {
                return sessionPlayer.Hand.GetCards()[1].CardImage;
            }
        }

        private void UpdatePlayersStats()
        {
            foreach (Player player in players.GetPlayers())
            {
                Player? gamePlayer = gameplay.GetPlayers().FirstOrDefault(p => p.Name == player.Name);

                if (gamePlayer != null)
                {
                    player.Chips = gamePlayer.Chips;
                }
            }
        }

        private async Task ManageBettingRound()
        {
            foreach (Player player in gameplay.GetPlayers())
            {
                // todo: if there is only 1 player left in the round they are automatically the winner
                if(player.CurrentAction != PlayerAction.Fold && player.ParticipatedInRound != true)
                {
                    if (player.SessionActivePlayer)
                    {
                        sessionPlayersTurn = true;
                        await ShowStatusMessage("It is your turn to make a move");
                        while(sessionPlayersTurn)
                        {
                            // Wait for player to make a move
                            await Task.Delay(100);
                        }
                    }
                    else
                    {
                        // simulate moves
                        Random random = new Random();
                        PlayerAction action = (PlayerAction)random.Next(0, Enum.GetNames(typeof(PlayerAction)).Length);

                        // todo: remove fold option from condition, initially put here to make it not possible for testing gameplay easier
                        // todo: learn more about poker to find out more about the different actions
                        while (action == PlayerAction.Bet || action == PlayerAction.None || action == PlayerAction.Fold)
                        {
                            action = (PlayerAction)random.Next(0, Enum.GetNames(typeof(PlayerAction)).Length);
                        }

                        // reduce number of random raises as too many picked randomly
                        if (action == PlayerAction.Raise && gameplay.GetCurrentBetAmount() >= 250)
                        {
                            action = PlayerAction.Check;
                        }

                        int amount = 0;

                        switch(action)
                        {
                            case PlayerAction.Check:
                                if(gameplay.GetCurrentBetAmount() > 0)
                                {
                                    action = PlayerAction.Call;
                                    await ShowStatusMessage($"{player.Name} Called, updating pot");
                                }
                                break;

                            case PlayerAction.Call:
                                if(gameplay.GetCurrentBetAmount() == 0)
                                {
                                    action = PlayerAction.Check;
                                    await ShowStatusMessage($"{player.Name} checked");
                                }
                                break;

                            case PlayerAction.Raise:
                                int max = player.Chips - gameplay.GetCurrentBetAmount();
                                amount = random.Next(1, max >= 100 ? 100 : max);
                                await ShowStatusMessage($"{player.Name} raised {amount} chips, updating pot");
                                break;
                        }

                        gameplay.ManageBettingRound(action, player, amount);
                    }
                }
            }

            if(gameplay.GetPlayers().Any(player => !player.ParticipatedInRound && player.CurrentAction != PlayerAction.Fold))
            {
                await ManageBettingRound();
            }
        }

        private void ValidateInput(ChangeEventArgs e)
        {
            raiseAmount = e.Value.ToString();
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            if (int.TryParse(raiseAmount, out int parsedBetAmount) && parsedBetAmount > 0)
            {
                if (parsedBetAmount > sessionPlayer.Chips)
                {
                    isRaiseButtonDisabled = true;
                }
                else
                {
                    isRaiseButtonDisabled = false;
                }
            }
            else
            {
                isRaiseButtonDisabled = true;
            }
        }

        private async Task Fold()
        {
            await ShowStatusMessage($"{sessionPlayer.Name} has folded");
            gameplay.ManageBettingRound(PlayerAction.Fold, sessionPlayer);
            sessionPlayersTurn = false;
        }

        private async Task Check()
        {
            await ShowStatusMessage($"{sessionPlayer.Name} has checked");
            gameplay.ManageBettingRound(PlayerAction.Check, sessionPlayer);
            sessionPlayersTurn = false;
        }

        private async Task Call()
        {
            await ShowStatusMessage($"{sessionPlayer.Name} has Called, Updating Pot");
            gameplay.ManageBettingRound(PlayerAction.Call, sessionPlayer);
            sessionPlayersTurn = false;
        }

        private async Task Raise()
        {
            await ShowStatusMessage($"{sessionPlayer.Name} has raised {raiseAmount} chips");
            // todo: ensure they have enough chips, if not show message to user and also cleanup
            gameplay.ManageBettingRound(PlayerAction.Raise, sessionPlayer, int.Parse(raiseAmount));
            sessionPlayersTurn = false;
        }
    }
}