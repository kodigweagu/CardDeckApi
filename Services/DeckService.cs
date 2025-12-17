using CardDeckApi.Models;

namespace CardDeckApi.Services;

public interface IDeckService
{
    IReadOnlyList<Card> Shuffle();
    (int count, IReadOnlyList<Card> cards, IReadOnlyList<Card> drawn) DrawFive();
    IReadOnlyList<Card> GetRemainingCards();
    IReadOnlyList<Card> NewDeck();
}

public class DeckService : IDeckService
{
    private static readonly Random _random = new();
    private Stack<Card> _deck = new();

    public DeckService()
    {
        NewDeck();
    }

    public IReadOnlyList<Card> NewDeck()
    {
        var cards = Enum.GetValues<Rank>()
            .SelectMany(rank =>
                Enum.GetValues<Suit>()
                    .Select(suit => new Card(rank, suit)))
            .ToList();

        _deck = new Stack<Card>(cards);
        return [.. _deck];
    }

    public IReadOnlyList<Card> Shuffle()
    {
        var cards = _deck.ToList();

        // Fisher–Yates shuffle
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (cards[i], cards[j]) = (cards[j], cards[i]);
        }

        _deck = new Stack<Card>(cards.Reverse<Card>());
        return [.. _deck];
    }

    public (int count, IReadOnlyList<Card> cards, IReadOnlyList<Card> drawn) DrawFive()
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

        return (
            _deck.Count,
            [.._deck],
            drawn
        );
    }
    
    public IReadOnlyList<Card> GetRemainingCards()
    {
        // Stack enumerates from top → bottom
        return [.. _deck];
    }

}
