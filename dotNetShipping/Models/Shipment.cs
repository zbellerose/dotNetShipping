using System.Collections.Generic;

namespace dotNetShipping.Models
{
    public class Shipment
    {
        public ICollection<IRateAdjuster> RateAdjusters { get; set; }
        /// <summary>
        ///     Shipment rates
        /// </summary>
        public List<Rate> Rates { get; } = new List<Rate>();
        /// <summary>
        ///     Errors returned by service provider (e.g. 'Wrong postal code')
        /// </summary>
        public List<Error> Errors { get; } = new List<Error>();

        /// <summary>
        ///     Internal library errors during interaction with service provider
        ///     (e.g. SoapException was trown)
        /// </summary>
        public List<string> InternalErrors { get; } = new List<string>();
    }
}