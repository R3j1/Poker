using Poker.Enums;

namespace Poker.Models
{
    public class Deck
    {
        private List<Card> cards;

        public string DeckName { get; set; }

        public Deck()
        {
            InitializeDeck();
        }

        public void Shuffle()
        {
            Random random = new Random();
            cards = cards.OrderBy(card => random.Next()).ToList();
        }

        public Card DealCard()
        {
            if (cards.Count == 0)
            {
                throw new InvalidOperationException("Deck is empty. Reshuffle or create a new deck.");
            }

            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        private void InitializeDeck()
        {
            cards = new List<Card>();

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    cards.Add(new Card(suit, rank));
                }
            }
        }

        public void ResetDeck()
        {
            InitializeDeck();
        }

        public List<Card> GetCards()
        {
            return cards;
        }
    }
}