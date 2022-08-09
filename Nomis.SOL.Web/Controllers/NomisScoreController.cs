namespace Nomis.SOL.Web.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Client;
using Services;


[Route("api/[controller]")]
[ApiController]
public class NomisScoreController : ControllerBase
{
    private readonly ScoreCalcService _scoreCalc;
    private readonly ILogger<NomisScoreController> _logger;

    public NomisScoreController(ScoreCalcService scoreCalc, ILogger<NomisScoreController> logger)
    {
        _scoreCalc = scoreCalc;
        _logger = logger;
    }

    /// <summary>
    /// Get Nomis Score for given wallet address.
    /// </summary>
    /// <param name="address">Solana wallet address to get Nomis Score.</param>
    /// <returns>An NomisScore value and corresponding statistical data.</returns>
    /// <remarks>
    /// Sample request:
    ///     GET /api/NomisScore?address=TNTsj7LaigtZZZpyupRUGTQhvkoNFjkN3QgQxEhvriKo
    /// </remarks>
    /// <response code="200">Returns Nomis Score and stats.</response>
    /// <response code="400">Address not valid.</response>
    /// <response code="404">No data found.</response>
    /// <response code="500">Unknown internal error.</response>
    [HttpGet]
    [ProducesResponseType(typeof(WalletScoreResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public async Task<IActionResult> GetAsync([Required] string address)
    {
        try
        {
            var result = await _scoreCalc.GetStatsAsync(address);
            return Ok(result);
        }
        catch (InvalidAddressException)
        {
            return BadRequest();
        }
        catch (NoDataException)
        {
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error calculating score for address = {Address}.", address);
            return StatusCode(500);
        }

    }
}