using System;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using dotNetShipping.Models;
using RestSharp;

namespace dotNetShipping.ShippingProviders
{
    public abstract class AbstractShippingProvider : IShippingProvider
    {
        public abstract string Name { get; }
        public Shipment Shipment { get; }
        public abstract string GetRatesUrl { get; }
        public abstract string GetAccessTokenUrl { get; }
        public abstract Task GetRates();
        
        protected void AddError(Error error)
        {
            lock (Shipment)
            {
                Shipment.Errors.Add(error);
            }
        }

        protected void AddInternalError(string error)
        {
            lock (Shipment)
            {
                Shipment.InternalErrors.Add(error);
            }
        }

        protected void AddRate(string providerCode, string name, decimal totalCharges, DateTime delivery, RateOptions options, string currencyCode)
        {
            AddRate(new Rate(Name, providerCode, name, totalCharges, delivery, options, currencyCode));
        }

        private void AddRate(Rate rate)
        {
            lock (Shipment)
            {
                if (Shipment.RateAdjusters != null)
                {
                    rate = Shipment.RateAdjusters.Aggregate(rate, (current, adjuster) => adjuster.AdjustRate(current));
                }
                Shipment.Rates.Add(rate);
            }
        }
        
        protected HttpWebRequest CreateWebRequest(string endPoint, string method, string contentType, int? length = null)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(endPoint);
            webRequest.ContentType = contentType;
            webRequest.Method = method;

            if (length.HasValue)
            {
                webRequest.ContentLength = length.Value;
            }

            return webRequest;
        }

        protected RestRequest CreateRestRequest(string endPoint, Method method, string contentType)
        {
            RestRequest request = new RestRequest(endPoint, method);
            request.AddHeader("Content-Type", contentType);
            return request;
        }
        
        protected T ExecuteRestRequest<T>(RestRequest request)
        {
            RestClient client = new RestClient();
            RestResponse<T> response = client.Execute<T>(request);
            if (response.StatusCode == HttpStatusCode.OK) return response.Data;
            
            AddInternalError($"Request failed with status code {response.StatusCode}");
            return default;

        }
    }
}