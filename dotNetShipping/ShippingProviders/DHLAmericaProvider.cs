using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using dotNetShipping.Models;
using RestSharp;

namespace dotNetShipping.ShippingProviders
{
    /// <summary>
    ///     Provides rates for DHL Americas.
    ///     See https://docs.api.dhlecs.com/#intro
    /// </summary>
    public class DHLAmericaProvider : AbstractShippingProvider
    {
        public override string Name => "DHL Americas";
        
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _pickupId;
        private readonly string _distributionCenter;
        private readonly bool _useProduction;
        
        public override string GetRatesUrl => _useProduction ? "https://api.dhlecs.com/auth/v4/accesstoken" : "https://api-sandbox.dhlecs.com/auth/v4/accesstoken";
        public override string GetAccessTokenUrl => _useProduction ? "https://api.dhlecs.com/shipping/v4/products" : "https://api-sandbox.dhlecs.com/shipping/v4/products";
        
        public DHLAmericaProvider(string clientId, string clientSecret, string pickupId, string distributionCenter, bool useProduction)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _pickupId = pickupId;
            _distributionCenter = distributionCenter;
            this._useProduction = useProduction;
        }
        
        public override Task GetRates()
        {
            string accessToken = RequestAccessToken();
            
            return Task.CompletedTask;
        }

        private string RequestAccessToken()
        {
            RestRequest request = CreateRestRequest(GetAccessTokenUrl, Method.Post, "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", $"client_credentials&client_id={_clientId}&client_secret={_clientSecret}");
            DHLAccessTokenResponse response =  ExecuteRestRequest<DHLAccessTokenResponse>(request);
            return response.AccessToken ?? string.Empty;
        }
    }
    
    public abstract class DHLAccessTokenResponse
    {
        [JsonPropertyName("access_token")] public string AccessToken { get; set; }
        [JsonPropertyName("client_id")] public string ClientId { get; set; }
        [JsonPropertyName("token_type")] public string TokenType { get; set; }
        [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }
    }
}