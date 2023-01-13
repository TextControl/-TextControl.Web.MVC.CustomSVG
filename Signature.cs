using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TXTextControl.Esign {
    internal class Signature {

      private Size m_size;
      private Size m_distinationSize;
      private string m_signatureSVG;
      private float m_scalingFactor = 1.0f;

      public Signature(string signatureSVG, Size destinationSize) {
         m_signatureSVG = signatureSVG;
         m_distinationSize = destinationSize;
         m_size = GetSignatureSize();
         m_scalingFactor = CalculateScaling();
      }

      public Size Size { get { return m_size; } }
      public float ScalingFactor {  get { return m_scalingFactor; } }   

      private Size GetSignatureSize() {
         XmlDocument input = new XmlDocument();
         input.LoadXml(m_signatureSVG);
         XmlNodeList childNodes = input.GetElementsByTagName("svg");

         return new Size(
            int.Parse(childNodes[0].Attributes["width"].Value),
            int.Parse(childNodes[0].Attributes["height"].Value));
      }

      private float CalculateScaling() {

         float scalingFactor = 1;

         if (m_size.Width > m_distinationSize.Width) {
            scalingFactor = (float)m_distinationSize.Width / (float)m_size.Width;
         }

         return scalingFactor;
      }
   }

  
}
