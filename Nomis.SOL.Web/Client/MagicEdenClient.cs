using Nomis.SOL.Web.Client.DTO;

namespace Nomis.SOL.Web.Client
{
    public class MagicEdenClient
    {
        private const int ItemsFetchLimit = 500;

        private readonly HttpClient _client;

        public MagicEdenClient(Uri baseUri)
        {
            _client = new()
            {
                BaseAddress = baseUri
            };
        }

        private async Task<MagicEdenWalletActivity[]> GetWalletActivityList(string address, int? offset = null)
        {
            var request =
                $"/v2/wallets/{address}/activities?offset={offset ?? 0}&limit={ItemsFetchLimit}";

            var response = await _client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            var transactionsData = await response.Content.ReadFromJsonAsync<MagicEdenWalletActivity[]>();
            return transactionsData;
        }

        public async Task<IEnumerable<MagicEdenWalletActivity>> GetWalletActivitiesData(string address)
        {
            var result = new List<MagicEdenWalletActivity>();
            var transactionsData = await GetWalletActivityList(address);
            result.AddRange(transactionsData);
            var offset = 0;
            while (transactionsData.Length >= ItemsFetchLimit)
            {
                offset += ItemsFetchLimit;
                transactionsData = await GetWalletActivityList(address, offset: offset);
                result.AddRange(transactionsData);
            }

            return result;
        }
    }
}