using Azure.Storage.Blobs;

/* es para que se suban cosas al alamacenamiento de azure tipo nube para que se conecte al contenedor y mi cuenta
ya asu funcionando regresa el archivo subido en forma de link para el deste del visual */
namespace spotify.Services
{
    public class AzureBlobService
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public AzureBlobService(IConfiguration config)
        {
            _connectionString = config["AzureStorage:ConnectionString"];
            _containerName = config["AzureStorage:Container"];
        }

        public async Task<string> SubirArchivoAsync(IFormFile archivo)
        {
            var cliente = new BlobContainerClient(_connectionString, _containerName);
            await cliente.CreateIfNotExistsAsync();

            var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);

            var blob = cliente.GetBlobClient(nombreArchivo);

            using (var stream = archivo.OpenReadStream())
            {
                await blob.UploadAsync(stream, overwrite: true);
            }

            return blob.Uri.ToString();
        }
    }
}