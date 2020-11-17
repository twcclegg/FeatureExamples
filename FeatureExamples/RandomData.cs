using System.Collections.Generic;

namespace FeatureExamples{
    public class Random    {
        public List<double>? Data { get; init; }
    }

    public sealed record Result(Random? Random);

    public sealed record RandomData(Result? Result);
}
