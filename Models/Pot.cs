namespace Poker.Models
{
    public class Pot
    {
        public int PotTotal { get; set; }

        public void AddToPot(int amountToAdd)
        {
            PotTotal += amountToAdd;
        }
    }
}