using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CARD.Models
{
    public class UploadViewModel
    {
        public List<IFormFile>? Files { get; set; }
        public string? SuccessMessage { get; set; }
        public List<string>? Errors { get; set; }
    }
}