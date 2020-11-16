using System;
using System.Text.Json;
using System.Threading.Tasks;
using FeatureExamples;
using PhoneNumbers;

GrowingList<object?>? list = null;
list ??= new();
var call = JsonSerializer.Deserialize<Call>("{\"Minutes\": 13,\"Number\": \"+12124571397\"}")!;

list.Add(call.Cost());
list.Add(call.ToString());
list.Add(await GetPi(100,100));
if (list is not null)
    Console.WriteLine(list);

static async Task<double> GetPi(int count, int size)
{
    var inside = 0;
    var r = Points.GetRandomPoints(count, size);
    await foreach (var point in r)
    {
        if (point.Distance <= 1)
        {
            inside++;
        }
    }

    return 4.0 * inside / (count * size);
}

namespace FeatureExamples
{
    public static class Helper
    {
        public static readonly PhoneNumberUtil Util = PhoneNumberUtil.GetInstance();
    }
}
