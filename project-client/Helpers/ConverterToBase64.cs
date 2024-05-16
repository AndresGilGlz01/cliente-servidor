namespace project_client.Helpers
{
    public class ConverterToBase64
    {
        public  string ImageToBase64(IFormFile file)
        {
            // Verifica si se ha enviado un archivo
            if (file == null || file.Length == 0)
            {
                return "";
            }

            try
            {
                // Lee el contenido del archivo en un arreglo de bytes
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();

                    // Convierte el arreglo de bytes en una cadena Base64
                    var base64String = Convert.ToBase64String(fileBytes);

                    // Devuelve la cadena Base64
                    return base64String;
                }
            }
            catch (Exception ex)
            {
                return $"Error al convertir el archivo a Base64: {ex.Message}";
            }
        }
    
    }
}
