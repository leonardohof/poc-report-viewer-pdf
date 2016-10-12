using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Web;

namespace PocReportViewer.Repository.Base
{
    public static class UnitOfWork
    {
        private const string UNIT_OF_WORK = "POCCONTEXT_UNIT_OF_WORK";
        private static readonly ConcurrentDictionary<string, PocContext> _threads = new ConcurrentDictionary<string, PocContext>();
        private static Guid UnitTestContext = Guid.Empty;

        private static bool ContextExists()
        {
            return HttpContext.Current.Items.Contains(UNIT_OF_WORK);
        }


        public static PocContext Context
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (!ContextExists())
                    {
                        PocContext context = new PocContext();
                        HttpContext.Current.Items.Add(UNIT_OF_WORK, context);
                    }
                    return (PocContext)HttpContext.Current.Items[UNIT_OF_WORK];
                }
                else
                {
                    Thread thread = Thread.CurrentThread;
                    if (string.IsNullOrEmpty(thread.Name))
                    {
                        thread.Name = Guid.NewGuid().ToString();
                    }
                    if (!_threads.ContainsKey(Thread.CurrentThread.Name))
                    {
                        var context = new PocContext();
                        context.Configuration.AutoDetectChangesEnabled = false;
                        context.Configuration.ValidateOnSaveEnabled = false;
                        _threads[Thread.CurrentThread.Name] = context;
                    }
                    return (PocContext)_threads[Thread.CurrentThread.Name];


                }
            }
        }

        /// <summary>
        /// Save the changes made on a repository, this method must be called after the operations
        /// </summary>
        public static void Save()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Throw is commented because exceptions are being treated on the service.
        /// </summary>
        public static void Close()
        {
            try
            {
                if (ContextExists())
                {
                    Context.SaveChanges();
                    Context.Dispose();
                }
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Items.Contains(UNIT_OF_WORK))
                    {
                        HttpContext.Current.Items.Remove(UNIT_OF_WORK);
                    }
                }
                else
                {
                    PocContext v;
                    _threads.TryRemove(Thread.CurrentThread.Name, out v);
                }
            }
            catch (Exception ex)
            {
                //Logger.Error(ex);
            }
        }

        public static void SetContext(Guid guid)
        {
            UnitTestContext = guid;
        }
    }
}
