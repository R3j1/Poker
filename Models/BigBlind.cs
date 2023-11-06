namespace Poker.Models
{
    public class BigBlind
    {
        public Player Player { get; set; }

        public BigBlind(Player player)
        {
            this.Player = player;
            Player.ActionsTakenLog.Add($"{player.Name} is set to be the big blind.");
        }

        internal void ContributeBigBind(int amount)
        {
            Player.Chips -= amount;
            Player.TotalBet += amount;
            Player.ActionsTakenLog.Add($"Contributed big blind of {amount} chips");
        }
    }
}
