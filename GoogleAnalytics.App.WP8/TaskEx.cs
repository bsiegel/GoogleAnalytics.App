using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleAnalytics.App {
    public static class TaskEx {
        public static Task<TResult[]> WhenAll<TResult>(IEnumerable<Task<TResult>> tasks)
        {
            return Task.WhenAll(tasks);
        }
    }
}
