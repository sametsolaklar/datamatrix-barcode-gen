using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Common;
using ZXing.Rendering;
using ZXing;
using ZXing.Datamatrix;
using ZXing.Datamatrix.Encoder;
using System.Reflection.Metadata.Ecma335;
using static System.Net.Mime.MediaTypeNames;
using datamatrixBarcodeGen;

namespace datamatrixBarcodeGen
{
#pragma warning disable CA1416 // Validate platform compatibility

    public class CustomBitmapRenderer : IBarcodeRenderer<Bitmap>
    {
        public SymbolShapeHint SymbolShape { get; set; } = SymbolShapeHint.FORCE_RECTANGLE;
        public BarcodeData _BarcodeData { get; set; }

        public Bitmap Render(BitMatrix matrix, BarcodeFormat format, string content, EncodingOptions options)
        {
            int width = matrix.Width;

            int height = matrix.Height;
            int desiredWidth = 450;
            int desiredHeight = 75;
            Bitmap barcodeImage = new Bitmap(desiredWidth + 20, desiredHeight + 10);
            Font fontType = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
            int scaleX = 7;  // Rectangle width
            int scaleY = 6;  // Rectangle height
            using (Graphics g = Graphics.FromImage(barcodeImage))
            {
                // white background
                g.FillRectangle(new SolidBrush(Color.White), 0, 0, desiredWidth + 20, desiredHeight + 10);
                if (_BarcodeData.TYPE_ == "FORM" || _BarcodeData.TYPE_ == "DISH")
                {
                    int xOffset = (desiredWidth + 20) - (width * scaleX * 2 / 1 + 25);  // X eksenindeki hizalama
                    if (_BarcodeData.TYPE_ == null)
                    {
                        xOffset = (desiredWidth + 20) - (width * scaleX * 2 / 1 + 50);
                    }
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            if (matrix[x, y])
                            {
                                int rectX = (x * scaleX) + xOffset;  // Dikdörtgen içindeki x koordinatı
                                int rectY = y * scaleY - 5;  // Dikdörtgen içindeki y koordinatı 5 değeri margin
                                int rectWidth = scaleX;  // Dikdörtgenin genişliği
                                int rectHeight = scaleY;  // Dikdörtgenin yüksekliği

                                g.FillRectangle(new SolidBrush(Color.Black), rectX, rectY, rectWidth, rectHeight);
                            }
                        }
                    }

                    int defaultLocation = 160;
                    if (_BarcodeData.TYPE_ == null)
                    {
                        defaultLocation -= 20;
                    }
                    // Data Yazısı
                    string data = content.Substring(0, 10);
                    Font font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    SizeF textSize = g.MeasureString(data, font);
                    float a = barcodeImage.Width - textSize.Width - 5; // X koordinatı (sağdan biraz içeri)
                    float b = desiredHeight - 10; // Y koordinatı
                    g.DrawString(data, font, brush, a, b);

                    // HASTA AD YAZISI
                    string data1 = _BarcodeData.NAME_;
                    SizeF textSize1 = g.MeasureString(data1, font);
                    float f = (barcodeImage.Width + 50) - desiredWidth + defaultLocation; // X koordinatı (sağdan biraz içeri)
                    float h = desiredHeight / 2 - textSize1.Height - 5; // Y koordinatı
                    g.DrawString(data1, font, brush, f, h);

                    // HASTA ES AD YAZISI
                    string data2 = _BarcodeData.USER_NAME;
                    SizeF textSize2 = g.MeasureString(data2, font);
                    float j = (barcodeImage.Width + 50) - desiredWidth + defaultLocation; // X koordinatı (sağdan biraz içeri)
                    float k = desiredHeight / 2; // Y koordinatı
                    g.DrawString(data2, font, brush, j, k);

                    // Tip yazısı
                    if (_BarcodeData.TYPE_ != null)
                    {
                        string Type = _BarcodeData.TYPE_;
                        if (Type.Length <= 4)
                        {
                            fontType = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);
                        }
                        else if (Type.Length > 4)
                        {
                            fontType = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold);
                        }
                        string text = _BarcodeData.TYPE_.Trim(' ');
                        //SizeF textSize1 = g.MeasureString(text, font);

                        float c = 5; // X koordinatı (sol baştan)
                        float lineHeight1 = 10;
                        float totalHeight = lineHeight1 * text.Length; // Toplam yükseklik

                        float d = (barcodeImage.Height - totalHeight) / 2 + totalHeight * 3 - lineHeight1; // Y koordinatı (en alta)

                        SizeF charsize = g.MeasureString(text, font);
                        float yOffsetk = d - (text.Length + charsize.Width);
                        g.RotateTransform(-90); // 90 derece sola dönme
                        g.TranslateTransform(c, yOffsetk, System.Drawing.Drawing2D.MatrixOrder.Append); // Dönüşüm matrisi uygulama
                        g.DrawString(text, fontType, Brushes.Black, 0, 0);

                        //for (int i = 0; i < text.Length; i++)
                        //{
                        //    string character = text[i].ToString();
                        //    SizeF charSize = g.MeasureString(character, font);
                        //    float yOffset = d - (lineHeight1 + charSize.Height) / 2; // Y hizalaması

                        //    g.RotateTransform(-90); // 90 derece sola dönme
                        //    g.TranslateTransform(c, yOffset, System.Drawing.Drawing2D.MatrixOrder.Append); // Dönüşüm matrisi uygulama
                        //    g.DrawString(character, font, Brushes.Black, 0, 0);
                        //    g.ResetTransform(); // Dönüşümü sıfırlama
                        //    d -= lineHeight1 - 2; // Bir sonraki harf için Y koordinatını azaltma
                        //}
                    }
                }
                else if (_BarcodeData.TYPE_ == "USER")
                {
                    fontType = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
                    int xOffset = (desiredWidth + 20) - (width * scaleX * 2 / 1 + 25);  // X eksenindeki hizalama
                    if (_BarcodeData.TYPE_ == null)
                    {
                        xOffset = (desiredWidth + 20) - (width * scaleX * 2 / 1 + 50);
                    }
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            if (matrix[x, y])
                            {
                                int rectX = (x * scaleX) + xOffset;  // Dikdörtgen içindeki x koordinatı
                                int rectY = y * scaleY - 5;  // Dikdörtgen içindeki y koordinatı 5 değeri margin
                                int rectWidth = scaleX;  // Dikdörtgenin genişliği
                                int rectHeight = scaleY;  // Dikdörtgenin yüksekliği

                                g.FillRectangle(new SolidBrush(Color.Black), rectX, rectY, rectWidth, rectHeight);
                            }
                        }
                    }

                    int defaultLocation = 160;
                    if (_BarcodeData.TYPE_ == null)
                    {
                        defaultLocation -= 20;
                    }
                    // Data Yazısı
                    string data = content.Substring(0, 10);
                    Font font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    SizeF textSize = g.MeasureString(data, font);
                    float a = barcodeImage.Width - textSize.Width - 5; // X koordinatı (sağdan biraz içeri)
                    float b = desiredHeight - 10; // Y koordinatı
                    g.DrawString(data, font, brush, a, b);

                    // per AD YAZISI
                    string data1 = _BarcodeData.USER_NAME;
                    SizeF textSize1 = g.MeasureString(data1, font);
                    float f = (barcodeImage.Width + 50) - desiredWidth + defaultLocation; // X koordinatı (sağdan biraz içeri)
                    float h = desiredHeight / 2 - textSize1.Height - 5; // Y koordinatı
                    g.DrawString(data1, font, brush, f, h);



                    // Tip yazısı
                    if (_BarcodeData.TYPE_ != null)
                    {
                        string text = _BarcodeData.TYPE_.Trim(' ');
                        //SizeF textSize1 = g.MeasureString(text, font);

                        float c = 5; // X koordinatı (sol baştan)
                        float lineHeight1 = 10;
                        float totalHeight = lineHeight1 * text.Length; // Toplam yükseklik

                        SizeF charsize = g.MeasureString(text, font);
                        float d = (barcodeImage.Height - totalHeight) / 2 + totalHeight + 40; // Y koordinatı (en alta)
                        float yOffsetk = d - (text.Length + charsize.Width);
                        g.RotateTransform(-90); // 90 derece sola dönme
                        g.TranslateTransform(c, yOffsetk, System.Drawing.Drawing2D.MatrixOrder.Append); // Dönüşüm matrisi uygulama
                        g.DrawString(text, fontType, Brushes.Black, 0, 0);


                        //for (int i = 0; i < text.Length; i++)
                        //{
                        //    string character = text[i].ToString();
                        //    SizeF charSize = g.MeasureString(character, font);
                        //    float yOffset = d - (lineHeight1 + charSize.Height); // Y hizalaması

                        //    g.RotateTransform(-90); // 90 derece sola dönme
                        //    g.TranslateTransform(c, yOffset, System.Drawing.Drawing2D.MatrixOrder.Append); // Dönüşüm matrisi uygulama
                        //    g.DrawString(character, fontType, Brushes.Black, 0, 0);
                        //    g.ResetTransform(); // Dönüşümü sıfırlama
                        //    d -= lineHeight1 + 5; // Bir sonraki harf için Y koordinatını azaltma
                        //}
                    }

                }
                else if (_BarcodeData.TYPE_ == "PERSONEL")
                {
                    fontType = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
                    int xOffset = (desiredWidth + 20) - (width * scaleX * 2 / 1 + 25);  // X eksenindeki hizalama
                    if (_BarcodeData.TYPE_ == null)
                    {
                        xOffset = (desiredWidth + 20) - (width * scaleX * 2 / 1 + 50);
                    }
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            if (matrix[x, y])
                            {
                                int rectX = (x * scaleX) + xOffset;  // Dikdörtgen içindeki x koordinatı
                                int rectY = y * scaleY - 5;  // Dikdörtgen içindeki y koordinatı 5 değeri margin
                                int rectWidth = scaleX;  // Dikdörtgenin genişliği
                                int rectHeight = scaleY;  // Dikdörtgenin yüksekliği

                                g.FillRectangle(new SolidBrush(Color.Black), rectX, rectY, rectWidth, rectHeight);
                            }
                        }
                    }

                    int defaultLocation = 160;
                    if (_BarcodeData.TYPE_ == null)
                    {
                        defaultLocation -= 20;
                    }
                    // Data Yazısı
                    string data = content.Substring(0, 10);
                    var l = content.LastIndexOf('-');
                    data += content.Substring(l, content.Length - l);
                    Font font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    SizeF textSize = g.MeasureString(data, font);
                    float a = barcodeImage.Width - textSize.Width - 5; // X koordinatı (sağdan biraz içeri)
                    float b = desiredHeight - 10; // Y koordinatı
                    g.DrawString(data, font, brush, a, b);

                    // per AD YAZISI
                    string data1 = _BarcodeData.USER_NAME;
                    SizeF textSize1 = g.MeasureString(data1, font);
                    float f = (barcodeImage.Width + 50) - desiredWidth + defaultLocation; // X koordinatı (sağdan biraz içeri)
                    float h = desiredHeight / 2 - textSize1.Height - 5; // Y koordinatı
                    g.DrawString(data1, font, brush, f, h);



                    // Tip yazısı
                    if (_BarcodeData.TYPE_ != null)
                    {
                        string text = _BarcodeData.TYPE_.Trim(' ');
                        //SizeF textSize1 = g.MeasureString(text, font);

                        float c = 5; // X koordinatı (sol baştan)
                        float lineHeight1 = 10;
                        float totalHeight = lineHeight1 * text.Length; // Toplam yükseklik

                        float d = (barcodeImage.Height - totalHeight) / 2 + totalHeight +barcodeImage.Height; // Y koordinatı (en alta)

                        SizeF charsize = g.MeasureString(text, font);
                        float yOffsetk = d - (text.Length + charsize.Width);
                        g.RotateTransform(-90); // 90 derece sola dönme
                        g.TranslateTransform(c, yOffsetk, System.Drawing.Drawing2D.MatrixOrder.Append); // Dönüşüm matrisi uygulama
                        g.DrawString(text, fontType, Brushes.Black, 0, 0);




                        //for (int i = 0; i < text.Length; i++)
                        //{
                        //    string character = text[i].ToString();
                        //    float yOffset = d - ((lineHeight1 + 2) + charSize.Height - lineHeight1); // Y hizalaması

                        //    g.RotateTransform(-90); // 90 derece sola dönme
                        //    g.TranslateTransform(c, yOffset, System.Drawing.Drawing2D.MatrixOrder.Append); // Dönüşüm matrisi uygulama
                        //    g.DrawString(character, fontType, Brushes.Black, 0, 0);
                        //    g.ResetTransform(); // Dönüşümü sıfırlama
                        //    d -= lineHeight1 + 2; // Bir sonraki harf için Y koordinatını azaltma
                        //}
                    }
                }
            }
            return barcodeImage;

        }
        public Bitmap Render(BitMatrix matrix, BarcodeFormat format, string content)
        {
            throw new NotImplementedException();
        }
    }
}

#pragma warning restore CA1416 // Validate platform compatibility








