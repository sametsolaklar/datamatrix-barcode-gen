using datamatrixBarcodeGen;
using System.Drawing;
using ZXing.Datamatrix.Encoder;
using ZXing.Rendering;
using ZXing;

BarcodeData barcodeDat = new BarcodeData();
barcodeDat.DATA_ = "2021001267-22052023-121";//barcode data
barcodeDat.TYPE_ = "DISH";//Barcode type 
barcodeDat.USER_NAME = "Samet";
barcodeDat.NAME_ = barcodeDat.USER_NAME;


IBarcodeRenderer<Bitmap> renderer = new CustomBitmapRenderer { SymbolShape = SymbolShapeHint.FORCE_RECTANGLE, _BarcodeData = barcodeDat };
// custom render sınıfı 2 parametre  : SymbolShape=Barkodun yazılacağı şekil Kare Dikdörtgen vs // _BarcodeData = barkodun içeriği ve Tipi = DISH-FORM-PERSONEL-USER-OTHER
#pragma warning disable CA1416 // Validate platform compatibility
BarcodeWriter<Bitmap> writer = new BarcodeWriter<Bitmap>
{
    Format = BarcodeFormat.DATA_MATRIX,
    Options = new ZXing.Common.EncodingOptions
    {
        Width = 2,//width ratio
        Height = 1,//height ratio
        Margin = 2
    },
    Renderer = renderer
};

Bitmap barkod = writer.Write(barcodeDat.DATA_);
barkod.Save("datamatrix_barkod.png");

#pragma warning restore CA1416 // Validate platform compatibility