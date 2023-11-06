using Poker.Models;

namespace Poker.Pages
{
    public partial class ViewDeck
    {
        private readonly List<Deck> decks = new List<Deck>()
        {
            // todo: test out different decks, e.g. some deck may have more cards than a usual deck
            new Deck()
            {
                DeckName = "Deck 1"
            },
            new Deck()
            {
                DeckName = "Deck 2"
            },
            new Deck()
            {
                DeckName = "Deck 3"
            },
        };
        private Deck selectedDeck;

        protected override void OnInitialized()
        {
            selectedDeck = decks[0];
        }

        private void ShuffleDeck()
        {
            selectedDeck.Shuffle();
        }

        private void OrderDeck()
        {
            selectedDeck.ResetDeck();
        }
    }
}