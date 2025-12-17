using CardDeckApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CardDeckApi.Controllers;

[ApiController]
[Route("api/deck")]
public class DeckController : ControllerBase
{
    private readonly IDeckService _deckService;

    public DeckController(IDeckService deckService)
    {
        _deckService = deckService;
    }

    /// <summary>
    /// Shuffles and resets the deck
    /// </summary>
    [HttpPost("new_deck")]
    public IActionResult NewDeck()
    {
        var deck = _deckService.NewDeck();
        return Ok(new
        {
            count = deck.Count,
            cards = deck
        });
    }

    /// <summary>
    /// Shuffles and resets the deck
    /// </summary>
    [HttpPost("shuffle")]
    public IActionResult Shuffle()
    {
        var deck = _deckService.Shuffle();
        return Ok(deck);
    }

    /// <summary>
    /// Draws 5 cards from the top of the deck
    /// </summary>
    [HttpPost("draw_five")]
    public IActionResult DrawFive()
    {
        Shuffle();

        try
        {
            var result = _deckService.DrawFive();
            return Ok(new{
                result.drawn,
                remaining = result.cards,
                result.count
                });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// Returns the remaining cards in the deck without shuffling
    /// </summary>
    [HttpGet("remaining")]
    public IActionResult GetRemaining()
    {
        var remaining = _deckService.GetRemainingCards();
        return Ok(new
        {
            count = remaining.Count,
            cards = remaining
        });
    }

}
