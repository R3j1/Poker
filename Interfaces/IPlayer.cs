using Poker.Models;

namespace Poker.Interfaces
{
    public interface IPlayer
    {
        List<Player> GetPlayers();
        void AddPlayer(Player player);
    }
}