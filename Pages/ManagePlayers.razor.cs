using Poker.Models;

namespace Poker.Pages
{
    public partial class ManagePlayers
    {
        private string newPlayerName;
        private Player editPlayer;
        private int newPlayerChips = 10000;

        private void AddPlayer()
        {
            if (!string.IsNullOrWhiteSpace(newPlayerName) && newPlayerChips > 0)
            {
                players.AddPlayer(new Player(newPlayerName) { Chips = newPlayerChips });
                newPlayerName = string.Empty;
                newPlayerChips = 10000;
            }
        }

        private void EditPlayer(Player player)
        {
            editPlayer = player;
        }

        private void SaveEditedPlayer()
        {
            Player player = players.GetPlayers().FirstOrDefault(p => p.Name == editPlayer.Name);
            if(player != null)
            {
                player.Name = editPlayer.Name;
                player.Chips = editPlayer.Chips;
            }
            editPlayer = null;
        }

        private void DeletePlayer(Player player)
        {
            players.GetPlayers().Remove(player);
        }

        private void CancelEdit()
        {
            editPlayer = null;
        }
    }
}