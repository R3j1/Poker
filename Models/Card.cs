using Poker.Enums;

namespace Poker.Models
{
    public class Card
    {
        public Suit CardSuit { get; private set; }
        public Rank CardRank { get; private set; }
        public string CardImage { get; private set; }

        public Card(Suit suit, Rank rank)
        {
            CardSuit = suit;
            CardRank = rank;
            CardImage = GetCardImage(suit, rank);
        }

        public override string ToString()
        {
            return $"{CardRank} of {CardSuit}";
        }

        private string GetCardImage(Suit suit, Rank rank)
        {
            string rankString = ((int)rank).ToString();

            switch(rankString)
            {
                case "11":
                    rankString = "jack";
                    break;

                case "12":
                    rankString = "queen";
                    break;

                case "13":
                    rankString = "king";
                    break;

                case "14":
                    rankString = "ace";
                    break;
            }

            return $"/Images/{$"{rankString}_of_{suit.ToString().ToLower()}.png"}"; 
        }
    }
}