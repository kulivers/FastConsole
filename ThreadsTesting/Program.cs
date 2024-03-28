using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadsTesting
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            LocalConcurrentDictionaryRepliesObjectsFromOtherThread();
        }

        static void LocalConcurrentDictionaryRepliesObjectsFromOtherThread()
        {
            var concurrentDictionary = new ConcurrentDictionary<string, string>();
            Reply(concurrentDictionary);
            var waitForReply = WaitForReply(concurrentDictionary, "1", TimeSpan.FromSeconds(3));
            
        }

        public static TWaitType WaitForReply<TWaitType>(ConcurrentDictionary<string, TWaitType> replies, string replyKey, TimeSpan timeout) where TWaitType : class
        {
            TWaitType reply = null;
            using (var tokenSource = new CancellationTokenSource(timeout))
            {
                var token = tokenSource.Token;
                try
                {
                    var spinner = new SpinWait();
                    var task = Task.Run(async () =>
                    {
                        while (true)
                        {
                            reply = replies[replyKey];

                            if (reply != null)
                            {
                                break;
                            }
                            if (!spinner.NextSpinWillYield)
                            {
                                spinner.SpinOnce();
                            }
                        }
                    }, token);
                    task.Wait(token);
                }
                catch (OperationCanceledException)
                { }
                catch (AggregateException ae)
                {
                    if (!(ae.InnerException is TaskCanceledException))
                    {
                        throw ae.InnerException;
                    }
                }
                finally
                {
                    replies.TryRemove(replyKey, out _);
                }
            }
            return reply;
        }
        
        private static void Reply(ConcurrentDictionary<string, string> replies)
        {
            using (var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(2)))
            {
                var token = tokenSource.Token;
                var task = Task.Run(async () =>
                {
                    Thread.Sleep(1);
                    replies.TryAdd("1", "2");
                }, token);
                task.Wait(token);
            }
        }
    }
}