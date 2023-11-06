namespace Poker.Models
{
    public class Dealer
    {
        public Deck Deck { get; set; } = new Deck();
        public Player Player { get; set; }

        public Dealer(Player player)
        {
            this.Player = player;
            Player.ActionsTakenLog.Add($"{player.Name} is set to be the dealer.");
        }
    }
}
