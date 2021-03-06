﻿using System;
using System.Drawing;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;
using Caliburn.Micro;
using Interfaces;

namespace Services
{
    public class ImageService : IImageService
    {
        private readonly HaarObjectDetector _harHaarObjectDetector;
        private readonly FaceHaarCascade _cascade;
        private readonly IEventAggregator _eventAggregator;

        public ImageService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _cascade = new FaceHaarCascade();
            _harHaarObjectDetector = new HaarObjectDetector(_cascade, 30);
            SetupDetector();
        }

        private void SetupDetector()
        {
            _harHaarObjectDetector.SearchMode = ObjectDetectorSearchMode.Single;
            _harHaarObjectDetector.ScalingFactor = 1.5f;
            _harHaarObjectDetector.ScalingMode = ObjectDetectorScalingMode.GreaterToSmaller;
            _harHaarObjectDetector.UseParallelProcessing = true;
            _harHaarObjectDetector.Suppression = 2;
        }

        public bool ContainsPerson(Bitmap bitmap)
        {
            if(bitmap == null)
                return false;

            try
            {
                Rectangle[] rectangles = ContainsPersonReturnMarker(bitmap);
                bool result = rectangles.Length > 0;

                return result;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return false;
        }

        public Rectangle[] ContainsPersonReturnMarker(Bitmap bitmap)
        {
            return _harHaarObjectDetector.ProcessFrame(bitmap);
        }
    }
}
