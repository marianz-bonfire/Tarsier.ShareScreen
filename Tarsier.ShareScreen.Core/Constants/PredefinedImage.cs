using System.Drawing;
using System.Windows.Forms;

namespace Tarsier.ShareScreen.Core.Constants
{
    public class PredefinedImage
    {
        public static Bitmap Default() {
            var screenSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Bitmap image = new Bitmap(screenSize.Width, screenSize.Height);
            using (Graphics g = Graphics.FromImage(image)) {
                g.Clear(Color.Black);
            }

            // Create a font and a brush for the text
            Font font = new Font("Arial", 36, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.White);

            // Create a string format to center the text
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            // Draw the text on the image
            using (Graphics g = Graphics.FromImage(image)) {
                g.DrawString("Initializing camera...", font, brush, new RectangleF(0, 0, screenSize.Width, screenSize.Height), format);
            }

            // Save the image as a JPEG file
            //image.Save("image.jpg", ImageFormat.Jpeg);
            Bitmap defaultImage = image;
            // Dispose the resources
            image.Dispose();
            font.Dispose();
            brush.Dispose();
            return defaultImage;
        }
    }
}