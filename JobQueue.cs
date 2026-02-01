using StackExchange.Redis;
using System.Text.Json;

namespace Artifacts
{
    internal class JobQueue
    {
        // How many items to collect or monsters to kill per job assignment
        private const int JOB_QUANTITY = 10;

        private static readonly Lazy<JobQueue> _instance = new(() => new JobQueue());

        public static JobQueue Instance => _instance.Value;

        private JobQueue()
        {
        }

        public static async Task SubmitJobAsync(Job job)
        {
            await Instance.ModifyJobs(jobs =>
            {
                jobs.Add(job);
            });
        }

        public static List<Job> GetAllJobs()
        {
            return Instance.LoadQueueSafe("queue.json");
        }

        public async Task StartWork(string workerId, Job job)
        {
            await Instance.ModifyJobs(jobs =>
            {
                var existingJob = jobs.FirstOrDefault(j => j.Id == job.Id);
                if (existingJob != null)
                {
                    var chunk = new Job
                    {
                        Id = existingJob.Id,
                        Owner = workerId,
                        JobType = existingJob.JobType,
                        Code = existingJob.Code,
                        Quantity = Math.Min(existingJob.Quantity, JOB_QUANTITY),
                        Expiry = DateTime.UtcNow.AddHours(1)
                    };

                    existingJob.Quantity -= chunk.Quantity;

                    if (existingJob.Quantity <= 0)
                    {
                        jobs.Remove(existingJob);
                    }

                    jobs.Add(chunk);
                }

                // Clear any expired jobs
                jobs.RemoveAll(j => j.Expiry < DateTime.UtcNow);
            });
        }

        private async Task ModifyJobs(Action<List<Job>> action)
        {
            WithFileLock("queue.json.lock", () =>
            {
                var jobs = LoadQueueSafe("queue.json");
                action(jobs);
                SaveQueueSafe("queue.json", jobs);
            });
        }

        private void SaveQueueSafe(string path, List<Job> jobs)
        {
            var json = JsonSerializer.Serialize(jobs, new JsonSerializerOptions { WriteIndented = true });

            // Try exclusive access
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var sw = new StreamWriter(fs))
            {
                sw.Write(json);
            }
        }

        private List<Job> LoadQueueSafe(string path)
        {
            if (!File.Exists(path))
                return new List<Job>();

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var sr = new StreamReader(fs))
            {
                var json = sr.ReadToEnd();
                return JsonSerializer.Deserialize<List<Job>>(json);
            }
        }

        private static void WithFileLock(string lockPath, Action action)
        {
            while (true)
            {
                try
                {
                    using (var lockFs = new FileStream(lockPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                    {
                        action();
                        return; // done
                    }
                }
                catch (IOException)
                {
                    // Another process holds the lock
                    Thread.Sleep(50); // wait and retry
                }
            }
        }
    }
}
