using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datamatrixBarcodeGen
{
    public static class Services
    {
        public static BarcodeData barcodeDat { get; set; }
    }



    public sealed class BarcodeData
    {
        public string NAME_ { get; set; }
        public string USER_NAME { get; set; }
        public string TYPE_ { get; set; }
        public string DATA_ { get; set; }

    }
}
