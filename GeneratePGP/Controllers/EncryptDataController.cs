using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace GeneratePGP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EncryptDataController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public EncryptDataController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet("download-pgp")]
        public IActionResult GetEncryptedCsv()
        {
            // Contoh data CSV
            var csv = "Nama,NIK,Paket,Harga\nAgus,1607150104941002,15 Mbps,170000";
            var csvBytes = Encoding.UTF8.GetBytes(csv);

            // Lokasi public key
            //var publicKeyPath = Path.Combine(_env.WebRootPath, "pgp-keys", "public_key_dummy.asc");
            var publicKeyPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pgp-keys", "public_key_mcm.asc");


            // Enkripsi
            var pgpBytes = Helper.Encrypt(csvBytes, publicKeyPath);

            return File(pgpBytes, "application/octet-stream", "data.csv.pgp");
        }
    }
}
