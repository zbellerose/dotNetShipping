﻿namespace dotNetShipping.Models
{
    public class RateOptions
    {
        /// <summary>
        /// Saturday Delivery indicator. This flag will be set only if Saturday delivery was requested
        /// by the ShipmentOptions.SaturdayDelivery value.
        /// </summary>
        public bool SaturdayDelivery { get; set; }
    }
}