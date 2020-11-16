using System;
using System.Text.Json.Serialization;
using PhoneNumbers;
using Telephony.PhoneNumberUtil;

namespace FeatureExamples
{
    internal record Call
    {
        public Call(PhoneNumber number, double minutes) => (Number, Minutes) = (number, minutes);

        [JsonConverter(typeof(PhoneNumberTextConverter))]
        public PhoneNumber Number { get; }

        public double Minutes { get; }

        public TimeSpan Duration => TimeSpan.FromMinutes(Minutes);
        public double Cost()
            => Duration.TotalMinutes * (Number, Duration) switch
            {
                var (_, d) when d.Ticks is < 0 => throw new InvalidOperationException(),
                var (num, _) when num.CountryCode == 1 =>
                        Helper.Util.Format(num, PhoneNumberFormat.NATIONAL).Substring(3)
                    switch
                    {
                        "900" => 1,
                        "876" or "658" => 1.5,
                        _ => .001
                    },
                var (num, d) when num.CountryCode == 52 && d > TimeSpan.FromHours(1) => .01,
                ({CountryCode: 261}, _) => int.MaxValue,
                (_, _) => .4
            };

        public override string ToString() =>
            $"To {Number} for {Duration.TotalMinutes} minutes";
    }
}
