// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Npruehs.PerlinNoise
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    class Program
    {
        #region Constants

        private const string Usage = "USAGE: PerlinNoise <BitmapSize> <GridSize>";

        #endregion

        #region Public Methods and Operators

        static void Main(string[] args)
        {
            // Read arguments.
            if (args.Length < 2)
            {
                Console.WriteLine(Usage);
                Environment.Exit(1);
            }

            int bitmapSize;
            int gridSize;

            if (!int.TryParse(args[0], out bitmapSize))
            {
                Console.WriteLine(Usage);
                Environment.Exit(1);
            }

            if (!int.TryParse(args[1], out gridSize))
            {
                Console.WriteLine(Usage);
                Environment.Exit(1);
            }

            // Setup perlin noise instance.
            var perlinNoise = new PerlinNoise(bitmapSize, gridSize);

            // Create new bitmap.
            var bitmap = new Bitmap(bitmapSize, bitmapSize);

            for (int i = 0; i < bitmapSize; i++)
            {
                for (int j = 0; j < bitmapSize; j++)
                {
                    // Generate noise value.
                    var noiseValue = perlinNoise.Noise2D(i, j);

                    // Transform from [-1..+1] to [0..1].
                    var transformedNoiseValue = (noiseValue + 1) / 2;

                    // Transform to greyscale.
                    var grey = (int)Math.Floor(255 * transformedNoiseValue);

                    // Set pixel.
                    var color = Color.FromArgb(255, grey, grey, grey);
                    bitmap.SetPixel(i, j, color);
                }
            }

            // Write bitmap file.
            var bitmapFile = new FileInfo("noise.bmp");

            using (var fileStream = bitmapFile.Create())
            {
                bitmap.Save(fileStream, ImageFormat.Bmp);
            }
        }

        #endregion
    }
}