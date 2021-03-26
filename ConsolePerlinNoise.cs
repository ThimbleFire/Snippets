using SharpNoise;
using SharpNoise.Builders;
using SharpNoise.Modules;
using SharpNoise.Utilities.Imaging;
using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;

    public class NoisePackage
    {
        public int Seed { get; set; }
        public int OctaveCount { get; set; }
        public double Persistence { get; set; }
        public byte Quality { get; set; }
        public double Frequency { get; set; }

        public NoisePackage()
        {
        }
    }

    internal class PerlinNoise
    {
        public static NoisePackage Generate()
        {
            // Randomly generate a package to return at the end of the method
            NoisePackage package = new NoisePackage
            {
                Seed = new Random().Next(),
                OctaveCount = new Random().Next( 3, 9 ),
                Persistence = new Random().NextDouble(),
                Quality = (byte)NoiseQuality.Best,
                Frequency = (double)new Random().Next( 1, 3 )
            };

            // The noise source - a simple Perlin noise generator will do for this sample
            Perlin noiseSource = new Perlin
            {
                Seed = package.Seed,
                OctaveCount = package.OctaveCount,
                Persistence = package.Persistence,
                Quality = (NoiseQuality)package.Quality,
                Frequency = package.Frequency
            };

            // Create a new, empty, noise map and initialize a new planar noise map builder with it
            var noiseMap = new NoiseMap();
            var noiseMapBuilder = new PlaneNoiseMapBuilder
            {
                DestNoiseMap = noiseMap,
                SourceModule = noiseSource
            };

            // Set the size of the noise map
            noiseMapBuilder.SetDestSize( Program.MapWidth, Program.MapHeight );

            // Set the bounds of the noise mpa builder
            // These are the coordinates in the noise source from which noise values will be sampled
            noiseMapBuilder.SetBounds( -3, 3, -2, 2 );

            // Build the noise map - samples values from the noise module above,
            // using the bounds coordinates we have passed in the builder
            noiseMapBuilder.Build();

            // Create a new image and image renderer
            var image = new Image();
            var renderer = new ImageRenderer
            {
                SourceNoiseMap = noiseMap,
                DestinationImage = image
            };

            // The renderer needs to know how to map noise values to colors.
            // In this case, we use one of the predefined gradients, specifically the terrain gradient,
            // which maps lower noise values to blues and greens and higher values to brouns and whites.
            // This simulates the look of a map with water, grass and vegetation, dirt and mountains.
            renderer.BuildGrayscaleGradient();

            // Before rendering the image, we could set various parameters on the renderer,
            // such as the position and color of the light source.
            // But we aren't going to bother for this sample.

            // Finally, render the image
            renderer.Render();

            // Finally, save the rendered image as a PNG in the current directory
            using ( var fs = File.OpenWrite( "NoiseMapGrayscale.png" ) )
            {
                image.SaveGdiBitmap( fs, ImageFormat.Png );
            }

            renderer.BuildTerrainGradient();

            renderer.Render();

            using ( var fs = File.OpenWrite( "NoiseMapColor.png" ) )
            {
                image.SaveGdiBitmap( fs, ImageFormat.Png );
            }

            Process photoViewer = new Process();
            photoViewer.StartInfo.FileName = "NoiseMapColor.png";
            photoViewer.Start();

            Console.WriteLine(
                "\n      Frequency: " + noiseSource.Frequency +
                "\n      Lacunarity: " + noiseSource.Lacunarity +
                "\n      Octaves: " + noiseSource.OctaveCount +
                "\n      Persistence: " + noiseSource.Persistence +
                "\n      Seed: " + noiseSource.Seed + "\n" );

            return package;
        }

        public static NoiseMap GetMap( NoisePackage package )
        {
            Perlin noiseSource = new Perlin
            {
                Seed = package.Seed,
                OctaveCount = package.OctaveCount,
                Persistence = package.Persistence,
                Quality = (NoiseQuality)package.Quality,
                Frequency = package.Frequency
            };

            // Create a new, empty, noise map and initialize a new planar noise map builder with it
            var noiseMap = new NoiseMap();
            var noiseMapBuilder = new PlaneNoiseMapBuilder
            {
                DestNoiseMap = noiseMap,
                SourceModule = noiseSource
            };

            // Set the size of the noise map
            noiseMapBuilder.SetDestSize( Program.MapWidth, Program.MapHeight );

            // Set the bounds of the noise mpa builder
            // These are the coordinates in the noise source from which noise values will be sampled
            noiseMapBuilder.SetBounds( -3, 3, -2, 2 );

            // Build the noise map - samples values from the noise module above,
            // using the bounds coordinates we have passed in the builder
            noiseMapBuilder.Build();

            return noiseMap;
        }
    }
