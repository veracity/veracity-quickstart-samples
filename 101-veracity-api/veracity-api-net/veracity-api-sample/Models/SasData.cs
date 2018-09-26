namespace veracity_api_sample.Models
{
    /// <summary>
    /// Json structure for SAS token info data
    /// </summary>
    public class SasData
    {
        public string SasKey { get; set; }
        public string SasUri { get; set; }
        public string FillKey { get; set; }
        public string SasKeyExpiryTimeUTC { get; set; }
        public bool IsKeyExpired { get; set; }
        public bool AutoRefreshed { get; set; }
    }
}