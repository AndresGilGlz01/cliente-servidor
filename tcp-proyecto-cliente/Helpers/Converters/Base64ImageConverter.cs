using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace tcp_proyecto_cliente.Helpers.Converters;

public class Base64ImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        var base64 = value as string;

        if (base64 is null) return null!;

        var bitmap = new BitmapImage();

        bitmap.BeginInit();
        bitmap.StreamSource = new MemoryStream(System.Convert.FromBase64String(base64));
        bitmap.EndInit();

        return bitmap;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}