using System.Text;

namespace TXTextControl.Esign {
   public class SignatureStamp {
      public static byte[] Create(
         Size size,
         string name,
         float fontSize,
         string signatureSVG,
         string documentID = "",
         Color? color = null,
         Color? highlightColor = null,
         int offsetX = 70,
         int offsetY = 20,
         int highlights = 10) {

         // default colors
         color = color ?? Color.LightGray;
         highlightColor = highlightColor ?? Color.Blue;

         // write svg header
         string sSvgImage = "<?xml version=\"1.0\" encoding=\"utf-8\"?><svg version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\" viewBox=\"0 0 " + size.Width.ToString() + " " + size.Height.ToString() + "\" xml:space=\"preserve\">";

         int countOffsetX = -offsetX;
         int countOffsetY = 0;
         int countX = 0;
         int countY = 0;

         // generate 
         int[][] positions = CreateRandomPositions(size, offsetX, countOffsetY, offsetY, 10);

         // loop through all rows
         do {

            string textColor;

            // loop through all columns
            do {

               // change highlight color based on random positions
               if (Array.Exists(positions, element => (element[0] == countX) && (element[1] == countY)))
                  textColor = HexConverter(highlightColor);
               else
                  textColor = HexConverter(color);

               // render the text
               sSvgImage += "<g><text style=\"font-size: " + fontSize.ToString() + "px; font-family: Arial; fill: " + textColor + ";\" transform=\"matrix(1 -0.17 0.17 1 " + countOffsetX.ToString() + " " + countOffsetY.ToString() + ")\">" + name + "</text><text style=\"font-size: " + fontSize.ToString() + "px; font-family: Arial; fill: " + textColor + ";\" transform=\"matrix(1 -0.17 0.17 1 " + (countOffsetX + 30).ToString() + " " + (countOffsetY + 0.4).ToString() + ")\">" + name + "</text></g>";
               
               // increase offset
               countOffsetX += offsetX;
               countX++;

            } while (countOffsetX < size.Width + offsetX);

            countOffsetY += offsetY;
            countOffsetX = -offsetX;

            countY++; countX = 0;

         } while (countOffsetY < size.Height + offsetY);

         // do all required calculations in Signature object
         var signature = new Signature(signatureSVG, size);
         
         // center the signature in stamp
         var posX = (size.Width - (signature.Size.Width * signature.ScalingFactor)) / 2;
         var posY = (size.Height - (signature.Size.Height * signature.ScalingFactor)) / 2;

         sSvgImage += "<g transform=\"scale(" + signature.ScalingFactor + ") translate(" + posX + " " + posY + ")\">";
         
         // render the signature
         sSvgImage += signatureSVG;
         sSvgImage += "</g>";

         // adding document ID
         sSvgImage += "<g transform=\"translate(10 20)\"><text style=\"font-family: Arial; fill: #000;\">Document ID: " + documentID + "</text></g>";

         // close the SVG
         sSvgImage += "</svg>";

         // return the SVG as a byte array
         return Encoding.ASCII.GetBytes(sSvgImage);
      }

      private static int[][] CreateRandomPositions(Size size, int offsetX, int countOffsetY, int offsetY, int count = 5) {

         List<int[]> positions = new List<int[]>();

         Random rnd = new Random();

         for (int i = 0; i <= count; i++) {

            int xRandom = rnd.Next(0, size.Width + offsetX) / offsetX;
            int yRandom = rnd.Next(countOffsetY, size.Height + offsetY) / offsetY;

            positions.Add(new int[] { xRandom, yRandom });
         }

         return positions.ToArray();
      }

      private static string HexConverter(System.Drawing.Color? c) {
         return "#" + c.Value.R.ToString("X2") + c.Value.G.ToString("X2") + c.Value.B.ToString("X2");
      }
   }
}
