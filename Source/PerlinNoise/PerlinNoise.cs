// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerlinNoise.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Npruehs.PerlinNoise
{
    using System;

    internal class PerlinNoise
    {
        #region Fields

        /// <summary>
        ///   Pseudo-random gradients of unit length for all grid points.
        /// </summary>
        private readonly Vector2[,] gradients;

        /// <summary>
        ///   Height and width of each grid cell, in pixels.
        /// </summary>
        private readonly int gridCellSize;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new perlin noise instance for filling a bitmap
        ///   of the specified size, broken down into a smaller grid of the
        ///   passed size.
        /// </summary>
        /// <param name="bitmapSize">
        ///   Size of the bitmap to generate.
        /// </param>
        /// <param name="gridSize">
        ///   Size of the grid imposed on the bitmap.
        /// </param>
        public PerlinNoise(int bitmapSize, int gridSize)
        {
            // Store cell size.
            this.gridCellSize = bitmapSize / gridSize;

            // Compute pseudo-random gradients.
            var random = new Random();

            this.gradients = new Vector2[gridSize + 1,gridSize + 1];

            for (int i = 0; i < gridSize + 1; i++)
            {
                for (int j = 0; j < gridSize + 1; j++)
                {
                    this.gradients[i, j] = this.RandomVector(random);
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Returns the value of the two-dimensional noise function at the specified position.
        /// </summary>
        public double Noise2D(int x, int y)
        {
            // Transform position from pixel space into grid space.
            Vector2 p = new Vector2((double)x / this.gridCellSize, (double)y / this.gridCellSize);

            // Get surrounding grid points.
            var x0 = x / this.gridCellSize;
            var x1 = x0 + 1;
            var y0 = y / this.gridCellSize;
            var y1 = y0 + 1;

            Vector2 bottomLeft = new Vector2(x0, y0);
            Vector2 bottomRight = new Vector2(x1, y0);
            Vector2 topLeft = new Vector2(x0, y1);
            Vector2 topRight = new Vector2(x1, y1);

            // Get vectors from grid points to (x, y).
            Vector2 fromBottomLeft = p - bottomLeft;
            Vector2 fromBottomRight = p - bottomRight;
            Vector2 fromTopLeft = p - topLeft;
            Vector2 fromTopRight = p - topRight;

            // Compute influence of the gradients.
            double s = this.gradients[x0, y0].Dot(fromBottomLeft);
            double t = this.gradients[x1, y0].Dot(fromBottomRight);
            double u = this.gradients[x0, y1].Dot(fromTopLeft);
            double v = this.gradients[x1, y1].Dot(fromTopRight);

            // Ease and combine to noise value.
            double sX = this.Ease(p.X - x0);
            double a = this.Interpolate(s, t, sX);
            double b = this.Interpolate(u, v, sX);

            double sY = this.Ease(p.Y - y0);
            double z = this.Interpolate(a, b, sY);

            return z;
        }

        #endregion

        #region Methods

        private double Ease(double p)
        {
            return (3 * Math.Pow(p, 2) - 2 * Math.Pow(p, 3));
        }

        private double Interpolate(double a, double b, double t)
        {
            return b * t + a * (1 - t);
        }

        private Vector2 RandomVector(Random random)
        {
            var v = new Vector2(random.NextDouble() * 2 - 1, random.NextDouble() * 2 - 1);
            return v.Normalize();
        }

        #endregion
    }
}