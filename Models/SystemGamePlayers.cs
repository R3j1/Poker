using Poker.Interfaces;

namespace Poker.Models
{
    public class SystemGamePlayers : IPlayer
    {
        List<Player> players = new List<Player>();

        public SystemGamePlayers()
        {
            // todo: load these from db
            players.Add(new Player("Bob"));
            players.Add(new Player("Bill"));
            players.Add(new Player("Ben"));
            players.Add(new Player("Tom"));
            players.Add(new Player("Jerry"));
            players.Add(new Player("Jo"));
            players.Add(new Player("Rej") { SessionActivePlayer = true });
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public List<Player> GetPlayers()
        {
            return players;
        }
    }
}