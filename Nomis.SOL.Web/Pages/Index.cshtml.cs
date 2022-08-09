namespace Nomis.SOL.Web.Pages;

using Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

public class IndexModel : PageModel
{
    private readonly ScoreCalcService _client;
    private readonly ILogger<IndexModel> _logger;

    [BindProperty]
    public string WalletAddress { get; set; }

    public WalletScoreResult Result { get; private set; }

    public bool NoData { get; private set; }

    public bool Error { get; private set; }

    public string ErrorMessage { get; private set; } = "Error while calculating ...";

    public IndexModel(ScoreCalcService client, ILogger<IndexModel> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(string address)
    {
        if (!string.IsNullOrWhiteSpace(address))
        {
            WalletAddress = address;

            try
            {
                Result = await _client.GetStatsAsync(address);
                
            }
            catch (InvalidAddressException e)
            {
                Error = true;
                ErrorMessage = e.Message;
            }
            catch (NoDataException e)
            {
                NoData = true;
                ErrorMessage = e.Message;
            }
            catch (Exception e)
            {
                Error = true;
                _logger.LogError(e, "Error calculating score for address = {Address}.", address);
            }
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        return LocalRedirect($"~/?address={WalletAddress}");
    }
}