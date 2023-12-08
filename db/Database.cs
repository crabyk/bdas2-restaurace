using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace BDAS2_Restaurace.DB
{
    public static class Database
    {
        public static OracleConnection Connect()
        {
            string connectionString =
                "User Id=st67264;Password=Dy6ngKrXuS;Data Source=(DESCRIPTION=" +
                "(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521))" +
                "(CONNECT_DATA=(SID=BDAS)));";
            return new OracleConnection(connectionString);
        }
    }

    public static class DbUtils
    {
        public static BitmapImage ConvertToBitmapFromBlob(OracleBlob imgBlob)
        {
            Byte[] byteArr = new Byte[imgBlob.Length];
            int i = imgBlob.Read(byteArr, 0, Convert.ToInt32(imgBlob.Length));
            MemoryStream memStream = new MemoryStream(byteArr);
            var image = Image.FromStream(memStream);

            using (var memory = new MemoryStream())
            {
                image.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public static OracleBlob ConvertToBlobFromBitmap(BitmapImage image, OracleConnection conn)
        {
            using (var memoryStream = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(memoryStream);

                byte[] byteArr = memoryStream.ToArray();

                OracleBlob oracleBlob = new OracleBlob(conn);
                oracleBlob.Write(byteArr);

                return oracleBlob;
            }
        }
    }
}
