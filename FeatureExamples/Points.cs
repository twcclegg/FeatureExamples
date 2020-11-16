using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FeatureExamples
{
    public class Points
    {
        private static readonly HttpClient HttpClient = new();

        private static readonly JsonSerializerOptions SerializerOptions = new()
            { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        private static Task<Task<T>> [] Interleaved<T>(IReadOnlyCollection<Task<T>> tasks)
        {

            var buckets = new TaskCompletionSource<Task<T>>[tasks.Count];
            var results = new Task<Task<T>>[buckets.Length];
            for (var i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new TaskCompletionSource<Task<T>>();
                results[i] = buckets[i].Task;
            }

            var nextTaskIndex = -1;

            void Continuation(Task<T> completed)
            {
                var bucket = buckets[Interlocked.Increment(ref nextTaskIndex)];
                bucket.TrySetResult(completed);
            }

            foreach (var task in tasks)
                task.ContinueWith(Continuation, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return results;
        }

        internal static async IAsyncEnumerable<Point> GetRandomPoints(int count, int size)
        {
            var batches = new Task<IEnumerable<Point>>[count];
            Array.Fill(batches, GetRandomPoints(size));

            foreach (var batch in Interleaved(batches))
            {
                foreach (var item in await await batch)
                {
                    yield return item;
                }
            }
        }

        private static async Task<IEnumerable<Point>> GetRandomPoints(int count)
        {
            var config = new
            {
                Jsonrpc = "2.0", Method = "generateDecimalFractions", Id = 0,
                Params = new { ApiKey = ApiKey.Value.ToString(), DecimalPlaces = 14, N = count * 2 }
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.random.org/json-rpc/2/invoke")
            {
                Content = new StringContent(JsonSerializer.Serialize(config, SerializerOptions))
                    {Headers = {ContentType = new MediaTypeHeaderValue("application/json")}}
            };
            using var response = await HttpClient.SendAsync(request);
            return Points((await JsonSerializer.DeserializeAsync<RandomData>(await response.Content.ReadAsStreamAsync(),
                SerializerOptions))!.Result!.Random!.Data!);

            static IEnumerable<Point> Points(IList<double> data)
            {
                for (var i = 0; i < data.Count; i += 2)
                {
                    yield return new Point(data[i], data[i + 1]);
                }
            }
        }
    }
}
