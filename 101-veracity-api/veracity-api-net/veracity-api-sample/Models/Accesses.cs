using System.Collections.Generic;

namespace veracity_api_sample.Models
{
    /// <summary>
    /// Json structure for Accesses result
    /// </summary>
    public class Accesses
    {
        public List<Access> Results { get; set; }
        public int Page { get; set; }
        public int ResultsPerPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }
}