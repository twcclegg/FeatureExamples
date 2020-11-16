using System;

namespace FeatureExamples
{
    public struct Point
    {
        public Point(double x, double y) => (X, Y) = (x, y);
        public double X { get; set; }
        public double Y { get; set; }
        public readonly double Distance => Math.Sqrt(X * X + Y * Y);

        public override string ToString() =>
            $"({X}, {Y}) is {Distance} from the origin";
    }
}
