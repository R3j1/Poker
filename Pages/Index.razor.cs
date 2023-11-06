using Microsoft.AspNetCore.Components;

namespace Poker.Pages
{
    public partial class Index
    {
        private string selectedPlayer = "";

        protected override void OnInitialized()
        {
            if(gameplay.GetPlayers().Count == 0 )
            {
                gameplay.AddPlayer(players.GetPlayers().FirstOrDefault(p => p.SessionActivePlayer));
            }
        }

        private void UpdateSelectedPlayer(ChangeEventArgs e)
        {
            selectedPlayer = e.Value.ToString();
        }

        private void AddPlayer()
        {
            var player = players.GetPlayers().FirstOrDefault(p => p.Name == selectedPlayer);
            if (player != null)
            {
                gameplay.AddPlayer(player);
                selectedPlayer = "";
            }
        }

        private void NavigateToGamePlayPage()
        {
            navManager.NavigateTo("/PokerGamePlay");
        }
    }
}