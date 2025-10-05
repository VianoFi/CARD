using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;
using CARD.Models;

namespace CARD.Controllers
{
    public class UploadController : Controller
    {
        private readonly long _maxFileSize = 3 * 1024 * 1024; // 3 MB

        private readonly Dictionary<string, byte[]> _fileSignatures = new()
        {
            { ".jpeg", new byte[] { 0xFF, 0xD8, 0xFF } },
            { ".jpg",  new byte[] { 0xFF, 0xD8, 0xFF } },
            { ".gif",  new byte[] { 0x47, 0x49, 0x46, 0x38 } },
            { ".pdf",  new byte[] { 0x25, 0x50, 0x44, 0x46 } }
        };

        [HttpGet]
        public IActionResult UploadFile()
        {
            return View(new UploadViewModel());
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 10 * 1024 * 1024)]
        public async Task<IActionResult> UploadFile(List<IFormFile> Files)
        {
            var model = new UploadViewModel { Errors = new List<string>() };

            if (Files == null || !Files.Any())
            {
                model.SuccessMessage = " Nessun file selezionato.";
                return View(model);
            }

            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            int uploadedCount = 0;
            long totalSize = 0;

            foreach (var file in Files)
            {
                string extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                // Validazione estensione
                if (!_fileSignatures.ContainsKey(extension))
                {
                    model.Errors.Add($" {file.FileName}: estensione '{extension}' non consentita.");
                    continue;
                }

                // Validazione dimensione
                if (file.Length > _maxFileSize)
                {
                    double sizeMB = Math.Round(file.Length / (1024.0 * 1024.0), 2);
                    model.Errors.Add($"{file.FileName}: dimensione {sizeMB}MB supera il limite di 3MB.");
                    continue;
                }

                // Validazione firma
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                byte[] expectedSignature = _fileSignatures[extension];

                if (fileBytes.Length < expectedSignature.Length)
                {
                    model.Errors.Add($" {file.FileName}: file corrotto o troppo piccolo.");
                    continue;
                }

                byte[] actualSignature = fileBytes.Take(expectedSignature.Length).ToArray();

                if (!actualSignature.SequenceEqual(expectedSignature))
                {
                    model.Errors.Add($" {file.FileName}: firma non valida per {extension}.");
                    continue;
                }

                // Salvataggio 
                string safeFileName = Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadPath, safeFileName);

                await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);

                uploadedCount++;
                totalSize += file.Length;
            }

            if (uploadedCount > 0)
            {
                double totalSizeKB = Math.Round(totalSize / 1024.0, 2);
                model.SuccessMessage = $" Caricati {uploadedCount} file per un totale di {totalSizeKB} KB.";
            }
            else
            {
                model.SuccessMessage = " Nessun file è stato caricato.";
            }

            return View(model);
        }
    }
}