using Poker.Enums;

namespace Poker.Models
{
    public class Hand
    {
        private List<Card> cards;

        public Hand()
        {
            cards = new List<Card>();
        }

        public int CardCount => cards.Count;

        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        public void Clear()
        {
            cards.Clear();
        }

        private class CardComparer : IComparer<Card>
        {
            public int Compare(Card x, Card y)
            {
                return x.CardRank.CompareTo(y.CardRank);
            }
        }

        public HandRank EvaluateHand()
        {
            // Sort the cards by rank (for straights and pairs)
            cards.Sort(new CardComparer());

            bool isFlush = IsFlush();
            bool isStraight = IsStraight();

            // Check for specific hand ranks
            if (isFlush && isStraight)
            {
                return HandRank.StraightFlush;
            }
            if (HasFourOfAKind())
            {
                return HandRank.FourOfAKind;
            }
            if (HasFullHouse())
            {
                return HandRank.FullHouse;
            }
            if (isFlush)
            {
                return HandRank.Flush;
            }
            if (isStraight)
            {
                return HandRank.Straight;
            }
            if (HasThreeOfAKind())
            {
                return HandRank.ThreeOfAKind;
            }
            if (HasTwoPair())
            {
                return HandRank.TwoPair;
            }
            if (HasOnePair())
            {
                return HandRank.OnePair;
            }

            return HandRank.HighCard; // Default: no hand found
        }

        private bool IsFlush()
        {
            var firstSuit = cards.First().CardSuit;
            return cards.All(card => card.CardSuit == firstSuit);
        }

        private bool IsStraight()
        {
            for (int i = 1; i < cards.Count; i++)
            {
                if (cards[i].CardRank != cards[i - 1].CardRank + 1)
                {
                    return false;
                }
            }
            return true;
        }

        private bool HasFourOfAKind()
        {
            for (int i = 0; i <= CardCount - 4; i++)
            {
                if (cards[i].CardRank == cards[i + 3].CardRank)
                {
                    return true;
                }
            }
            return false;
        }

        private bool HasFullHouse()
        {
            return (HasThreeOfAKind() && HasOnePair());
        }

        private bool HasThreeOfAKind()
        {
            for (int i = 0; i <= CardCount - 3; i++)
            {
                if (cards[i].CardRank == cards[i + 2].CardRank)
                {
                    return true;
                }
            }
            return false;
        }

        private bool HasTwoPair()
        {
            int pairCount = 0;
            for (int i = 0; i < CardCount - 1; i++)
            {
                if (cards[i].CardRank == cards[i + 1].CardRank)
                {
                    pairCount++;
                    i++;
                }
            }
            return pairCount == 2;
        }

        private bool HasOnePair()
        {
            for (int i = 0; i < CardCount - 1; i++)
            {
                if (cards[i].CardRank == cards[i + 1].CardRank)
                {
                    return true;
                }
            }
            return false;
        }


        public List<Card> GetCards()
        {
            return new List<Card>(cards);
        }
    }
}