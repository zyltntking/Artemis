namespace Artemis.Test.ImageSharpConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dirPath = @"C:\Users\zyltn\Desktop\images\8";

            var targetPath = @"C:\Users\zyltn\Desktop\images\8-R";

            var coverPath = @"C:\Users\zyltn\Desktop\images\CoverV2.png";

            var dirInfo = Directory.GetFiles(dirPath);

            Console.WriteLine($"检测到{dirInfo.Length}张图片");

            Console.WriteLine("开始处理图片....");

            for (var i = 0; i < dirInfo.Length; i++)
            {
                var imagePath = dirInfo[i];
                var fileName = Path.GetFileName(imagePath);
                Console.WriteLine($"处理第{i+1}图片:{fileName}...");

                using var image = Image.Load(imagePath);

                Console.WriteLine("加载覆盖...");
                var cover = Image.Load(coverPath);

                Console.WriteLine("计算水印位置...");
                var position = new Point(0 , image.Height - cover.Height);

                Console.WriteLine("复制图片...");
                var clone = image.Clone(process => process.DrawImage(cover, position, 1));

                clone.Save(Path.Combine(targetPath, fileName));
                Console.WriteLine("保存成功...");
                Console.WriteLine("===================================================");
                Thread.Sleep(10);
            }


            //var imagePath = @"C:\Users\zyltn\Desktop\images\Sample.png";

            //var coverPath = @"C:\Users\zyltn\Desktop\images\Cover.png";

            //using var image = Image.Load(imagePath);

            //var cover = Image.Load(coverPath);

            //var position = new Point(0, image.Height - cover.Height);

            //var clone = image.Clone(process => process.DrawImage(cover, position, 1));

            //clone.Save(@"C:\Users\zyltn\Desktop\images\Done.png");

            //using (var image = await Image.LoadAsync(imagePath))
            //{
            //    var cover = await Image.LoadAsync(coverPath);
            //    var size = new Size(cover.Width, cover.Height);
            //    var position = new Point(0, 0);
            //    image.Mutate(x => x.DrawImage(cover, size, position, 1));
            //    image.Save(@"C:\Users\zyltn\Desktop\images\Sample2.png");
            //}

            Console.WriteLine("Hello, World!");
        }

        
    }
}