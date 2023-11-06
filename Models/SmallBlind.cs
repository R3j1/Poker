namespace Poker.Models
{
    public class SmallBlind
    {
        public Player Player { get; set; }

        public SmallBlind(Player player)
        {
            this.Player = player;
            Player.ActionsTakenLog.Add($"{player.Name} is set to be the small blind.");
        }

        internal void ContributeSmallBind(int amount)
        {
            Player.Chips -= amount;
            Player.TotalBet += amount;
            Player.ActionsTakenLog.Add($"Contributed small blind of {amount} chip(s)");
        }
    }
}