using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.Requests
{
    public class SearchBarRequest
    {
        // The following are info fields used in logging for search requests
        [Required] public string jwtToken { get; set; }
        [Required] public string Ip { get; set; }
        [Required] public string Url { get; set; }
    }
}
