using System;
using System.IO;
using Tesseract;
using ImageMagick;
using System.Collections.Generic;
using System.Linq;

namespace OCR_Tesseract
{
    internal class Program
    {
        private static readonly string filePath = "C:\\Users\\jeff1\\source\\repos\\OCR Tesseract\\bin\\Debug\\net6.0\\test.pdf";//@"./test.pdf";

        static void Main()
        {
            using TesseractEngine engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);

            foreach ((string text, int pageIndex) in ConvertPDFToImage(filePath, engine).Select((text, index) => (text, index)))
            {
                File.WriteAllText($"test{pageIndex}.txt", text);
                Console.WriteLine($"Done, {pageIndex}");
            }

            Console.WriteLine("Done!");
        }

        private static IEnumerable<string> ConvertPDFToImage(string filePath, TesseractEngine ocrEngine)
        {
            string tmpFile = Path.ChangeExtension(Path.Combine(Directory.GetCurrentDirectory(), Path.GetTempFileName()), "png"); 

            MagickReadSettings settings = new MagickReadSettings
            {
                Density = new Density(140),
                ColorType = ColorType.Grayscale
            };

            using MagickImageCollection images = new MagickImageCollection();
            // Find a way to read one at a time?
            images.Read(filePath, settings);

            foreach (IMagickImage<ushort> image in images)
            {
                // Keep in memory and do one page at a time?
                image.Write(tmpFile);

                // Output here
                using (Pix pixImage = Pix.LoadFromFile("temp.png"))
                {
                    using (Page page = ocrEngine.Process(pixImage))
                    {
                        // Output here
                        yield return page.GetText();
                    }
                }
            }

            File.Delete(tmpFile);
        }
    }
}