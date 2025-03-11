using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Xml.Schema;

namespace LopushokApp.DB
{
    public partial class Product
    {
        public BitmapImage ProductImage
        {
            get
            {
                if (Image != null && Image.Length > 0)
                {
                    return LoadImageFromBytes(Image);
                }
                return LoadDefaultImage();
            }
        }

        private BitmapImage LoadImageFromBytes(byte[] imageData)
        {
            using (var byteStream = new MemoryStream(imageData))
            {
                return CreateBitmapImage(byteStream);
            }
        }

        private BitmapImage LoadDefaultImage()
        {
            try
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image", "products", "picture.png");
                return new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
                return null;
            }
        }

        private BitmapImage CreateBitmapImage(Stream stream)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = stream;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        }

        public string Materials => string.Join(" ", ProductMaterial.Select(pm => pm.Material.Name).ToArray());

        public double TotalCost
        {
            get
            {
                double totalCost = (double)ProductMaterial.Sum(pm => (pm.Count_Mat_Fot_One ?? 0) * pm.Material.Price);
                return totalCost > 0 ? totalCost : (double) Min_Price_For_Agent;
            }
        }

        public SolidColorBrush BackgroundColor
        {
            get
            {
                var lastSale = ProductSale?.LastOrDefault()?.SaleDate;
                if (lastSale == null || lastSale.Value.Month != DateTime.Now.Month || lastSale.Value.Year != DateTime.Now.Year)
                {
                    return new SolidColorBrush(Colors.LightCoral);
                }
                return new SolidColorBrush(Colors.White);
            }
        }
    }
}
