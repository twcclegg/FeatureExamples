using System.Collections.Generic;

namespace FeatureExamples{
    public class Random    {
        public List<double> Data { get; init; }
    }

    public class Result
    {
        public Random Random { get; init; }
    }

    public record RandomData
    {
        public Result Result { get; init; }
    }
}
