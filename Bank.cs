using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;

namespace Artifacts
{
    internal class Bank
    {
        private MyAccountApi _api;
        private GrandExchangeApi _exchangeApi;
        private static Configuration _config;
        private static HttpClient _httpClient;

        internal static Bank Instance => lazy.Value;

        internal static void Config(
            Configuration config,
            HttpClient httpClient
            )
        {
            _config = config;
            _httpClient = httpClient;
        }

        private static readonly Lazy<Bank> lazy =
            new(() =>
            {
                if (_config == null || _httpClient == null)
                {
                    throw new InvalidOperationException("Items not configured. Call Items.Config() before accessing the Instance.");
                }
                return new Bank(_config, _httpClient);
            });
            
        private Bank(
            Configuration config,
            HttpClient httpClient
            )
        {
            _api = new MyAccountApi(httpClient, config);
            _exchangeApi = new GrandExchangeApi(httpClient, config);
        }

        internal async Task<List<GEOrderSchema>> GetExchangeOrders(string code)
        {
            var orders = await Utils.ApiCallGet(async () =>
            {
                return await _exchangeApi.GetGeOrdersGrandexchangeOrdersGetAsync(code);
            }) as DataPageGEOrderSchema;

            return orders.Data;
        }

        internal async Task<List<SimpleItemSchema>> GetItems()
        {
            var pageNum = 1;
            var result = new List<SimpleItemSchema>();
            DataPageSimpleItemSchema page = null;

            do
            {
                var data = await Utils.ApiCallGet(async () =>
                {
                    return await _api.GetBankItemsMyBankItemsGetAsync(page: pageNum);
                });

                page = data as DataPageSimpleItemSchema;

                result.AddRange(page.Data.Where(i => i.Quantity > 0));
                pageNum++;
            } while (pageNum <= page.Pages);

            return result;
        }

        internal async Task<BankSchema> GetBankDetails()
        {
            var result = await Utils.ApiCallGet(async () => await _api.GetBankDetailsMyBankGetAsync());
            return (result as BankResponseSchema).Data;
        }

        internal async Task<List<PendingItemSchema>> GetPendingItems()
        {
            var result = await Utils.ApiCallGet(async () => await _api.GetPendingItemsMyPendingItemsGetAsync()) as DataPagePendingItemSchema;
            return result.Data;
        }
    }
}
