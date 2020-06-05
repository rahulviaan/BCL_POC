using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            ////creating a image object
            //System.Drawing.Image bitmap = (System.Drawing.Image)Bitmap.FromFile(Server.MapPath("onam.jpg")); // set image 
            //                                                                                                 //draw the image object using a Graphics object
            //Graphics graphicsImage = Graphics.FromImage(bitmap);
            ////Set the alignment based on the coordinates   
            //StringFormat stringformat = new StringFormat();
            //stringformat.Alignment = StringAlignment.Far;
            //stringformat.LineAlignment = StringAlignment.Far;
            //StringFormat stringformat2 = new StringFormat();
            //stringformat2.Alignment = StringAlignment.Center;
            //stringformat2.LineAlignment = StringAlignment.Center;
            ////Set the font color/format/size etc..  
            //Color StringColor = System.Drawing.ColorTranslator.FromHtml("#933eea");//direct color adding
            //Color StringColor2 = System.Drawing.ColorTranslator.FromHtml("#e80c88");//customise color adding
            //string Str_TextOnImage = "Happy";//Your Text On Image
            //string Str_TextOnImage2 = "Onam";//Your Text On Image
            //graphicsImage.DrawString(Str_TextOnImage, new Font("arial", 40,
            //FontStyle.Regular), new SolidBrush(StringColor), new Point(268, 245),
            //stringformat); Response.ContentType = "image/jpeg";
            //graphicsImage.DrawString(Str_TextOnImage2, new Font("Edwardian Script ITC", 111,
            //FontStyle.Bold), new SolidBrush(StringColor2), new Point(145, 255),
            //stringformat2); Response.ContentType = "image/jpeg";
            //bitmap.Save(Response.OutputStream, ImageFormat.Jpeg);
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection fc, HttpPostedFileBase file)
        {

            var Branch = fc["Branch"].ToString();
            var Image_url = file.ToString();
            var Name = fc["Name"].ToString();
            var Designation = fc["Designation"].ToString();

            var fileName = Path.GetFileName(file.FileName);

            var ext = Path.GetExtension(file.FileName);

            string name = Path.GetFileNameWithoutExtension(fileName);

            Guid guid = Guid.NewGuid();
            string myfile = guid.ToString() + "_" + ext;
            var path = Path.Combine(Server.MapPath("~/Content/images"), myfile);
            var bitmappath = Path.Combine(Server.MapPath("~/Content/images/Bitmap"), myfile);
            var captionbPath = Server.MapPath("~/Content/images/Caption.png");
            var captionedSavedPath = Path.Combine(Server.MapPath("~/Content/images/Captioned"), myfile);
            file.SaveAs(path);

            Image bitmap = (System.Drawing.Image)Bitmap.FromFile(captionbPath);

            StringFormat stringformat = new StringFormat();
            stringformat.Alignment = StringAlignment.Far;
            stringformat.LineAlignment = StringAlignment.Far;
            StringFormat stringformat2 = new StringFormat();
            stringformat2.Alignment = StringAlignment.Center;
            stringformat2.LineAlignment = StringAlignment.Center;
            //Set the font color/format/size etc..  
            Color StringColor = System.Drawing.ColorTranslator.FromHtml("#933eea");//direct color adding
            Color StringColor2 = System.Drawing.ColorTranslator.FromHtml("#e80c88");//customise color adding
            PointF NameLocation = new PointF(50f, 10f);
            PointF DesignationLocation = new PointF(50f, 50f);
            PointF Company = new PointF(50f,90f);
            PointF BranchLocation = new PointF(50f,130f);
            string Str_TextOnImage = Name;
            string Str_TextOnImage2 = Branch;
            string Str_TextOnImage3 = Designation;
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                using (Font arialFont = new Font("Arial", 30))
                {
                    graphics.DrawString(Name, arialFont, Brushes.Black, NameLocation);                    
                    graphics.DrawString(Designation, arialFont, Brushes.Black, DesignationLocation);
                    graphics.DrawString("Bajaj Capital Ltd.", arialFont, Brushes.Black, Company);
                    graphics.DrawString(Branch, arialFont, Brushes.Black, BranchLocation);
                }
            }
            Response.ContentType = "image/jpeg";
            // bitmap.Save(Response.OutputStream, ImageFormat.Jpeg);
            bitmap.Save(captionedSavedPath);//save the image file
            //byte[] buffer1 = getResizedImage(path, 200, 200);
            //Image image1 = GetImage(buffer1);
            //bitmap.Save(bitmappath);//save the image file

            Image image1 = Bitmap.FromFile(path);
            var width = image1.Width;
            var height = image1.Height;
            byte[] buffer2 = getResizedImage(captionedSavedPath, width, 100);
            Image image2 = GetImage(buffer2);

            //MergeTwoImages(image1, image2).Save(bitmappath);
            Bitmap captionedbitmap = MergeTwoImages(image1, image2);
            captionedbitmap.Save(Response.OutputStream, ImageFormat.Jpeg);
            captionedbitmap.Save(bitmappath);
            return View();

            /*End*/

        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public Image GetImage(byte[] imagebyte)
        {
            using (var ms = new MemoryStream(imagebyte))
            {
                return Image.FromStream(ms);
            }
        }
        public static Bitmap MergeTwoImages(Image firstImage, Image secondImage)
        {
            if (firstImage == null)
            {
                throw new ArgumentNullException("firstImage");
            }

            if (secondImage == null)
            {
                throw new ArgumentNullException("secondImage");
            }

            int outputImageWidth = firstImage.Width > secondImage.Width ? firstImage.Width : secondImage.Width;

            int outputImageHeight = firstImage.Height + secondImage.Height + 1;

            Bitmap outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(outputImage))
            {
                graphics.DrawImage(firstImage, new Rectangle(new Point(), firstImage.Size),
                    new Rectangle(new Point(), firstImage.Size), GraphicsUnit.Pixel);
                graphics.DrawImage(secondImage, new Rectangle(new Point(0, firstImage.Height + 1), secondImage.Size),
                    new Rectangle(new Point(), secondImage.Size), GraphicsUnit.Pixel);
            }

            return outputImage;
        }
        byte[] getResizedImage(String path, int width, int height)
        {
            Bitmap imgIn = new Bitmap(path);
            double y = imgIn.Height;
            double x = imgIn.Width;

            double factor = 1;
            if (width > 0)
            {
                factor = width / x;
            }
            else if (height > 0)
            {
                factor = height / y;
            }
            System.IO.MemoryStream outStream = new System.IO.MemoryStream();
            Bitmap imgOut = new Bitmap((int)(x * factor), (int)(y * factor));

            // Set DPI of image (xDpi, yDpi)
            imgOut.SetResolution(72, 72);

            Graphics g = Graphics.FromImage(imgOut);
            g.Clear(Color.White);
            g.DrawImage(imgIn, new Rectangle(0, 0, (int)(factor * x), (int)(factor * y)),
              new Rectangle(0, 0, (int)x, (int)y), GraphicsUnit.Pixel);

            imgOut.Save(outStream, getImageFormat(path));
            return outStream.ToArray();
        }
        string getContentType(String path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp": return "Image/bmp";
                case ".gif": return "Image/gif";
                case ".jpg": return "Image/jpeg";
                case ".png": return "Image/png";
                default: break;
            }
            return "";
        }

        ImageFormat getImageFormat(String path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp": return ImageFormat.Bmp;
                case ".gif": return ImageFormat.Gif;
                case ".jpg": return ImageFormat.Jpeg;
                case ".png": return ImageFormat.Png;
                default: break;
            }
            return ImageFormat.Jpeg;
        }
    }
}