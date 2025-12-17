using CardDeckApi.Models;

namespace CardDeckApi.Services;

public interface IDeckService
{
    IReadOnlyList<Card> Shuffle();
    IReadOnlyList<Card> DrawFive();
    IReadOnlyList<Card> GetRemainingCards();
}

public class DeckService : IDeckService
{
    private static readonly Random _random = new();
    private Stack<Card> _deck = new();

    public DeckService()
    {
        Shuffle();
    }

    public IReadOnlyList<Card> Shuffle()
    {
        var cards = Enum.GetValues<Rank>()
            .SelectMany(rank =>
                Enum.GetValues<Suit>()
                    .Select(suit => new Card(rank, suit)))
            .ToList();

        // Fisher–Yates shuffle
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (cards[i], cards[j]) = (cards[j], cards[i]);
        }

        _deck = new Stack<Card>(cards.Reverse<Card>());
        return _deck.ToList();
    }

    public IReadOnlyList<Card> DrawFive()
    {
        if (_deck.Count < 5)
        {
            throw new InvalidOperationException("Not enough cards left in the deck");
        }

        var drawn = new List<Card>(5);
        for (int i = 0; i < 5; i++)
        {
            drawn.Add(_deck.Pop());
        }

        return drawn;
    }
    
    public IReadOnlyList<Card> GetRemainingCards()
    {
        // Stack enumerates from top → bottom
        return _deck.ToList();
    }

}
