namespace Poker.Interfaces
{
    public interface IOmahaRules
    {
        int GetStartingChips(int numberOfPlayers);
        int GetSmallBlind(int round);
        int GetBigBlind(int round);
        int GetAnte(int round);
        int GetMaxBettingRounds();
        int GetNumberOfHoleCards();
    }
}