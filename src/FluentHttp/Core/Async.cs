// original code from http://tomasp.net/blog/csharp-async.aspx
// converted to C# 2.0 compatible code and modified to support exceptions.

namespace AsyncEnumerator
{
    using System;
    using System.Collections.Generic;

#if PRABIR_ASYNC_INTERNAL
    internal
#else
    public
#endif
 delegate void Action();

#if PRABIR_ASYNC_INTERNAL
    internal
#else
    public
#endif
 delegate void ErrorFunc<T>(T obj, Exception ex);

    #region Helpers

#if PRABIR_ASYNC_INTERNAL
    internal
#else
    public
#endif
 delegate IAsyncResult Func<TArg1, TArg2, TArg3>(TArg1 buffer, TArg2 offset, TArg3 length, AsyncCallback callback, object state);

#if PRABIR_ASYNC_INTERNAL
    internal
#else
    public
#endif
 delegate TResult Func<TArg1, TResult>(TArg1 arg1);

#if PRABIR_ASYNC_INTERNAL
    internal
#else
    public
#endif
 delegate IAsyncResult Func(AsyncCallback callback, object state);


    #endregion

    /// <summary>
    /// Represents a primitive untyped asynchronous operation.
    /// This interface should be used only in asynchronous method declaration.
    /// </summary>
#if PRABIR_ASYNC_INTERNAL
    internal
#else
    public
#endif
 interface IAsync
    {
        void ExecuteStep(Action<Exception> nextAction);

        Exception Exception { get; set; }
    }

    /// <summary>
    /// Represents a type with no value - alternative to C# void in 
    /// situations where void can't be used
    /// </summary>
#if PRABIR_ASYNC_INTERNAL
    internal
#else
    public
#endif
 class Unit
    {
        private Unit() { }

        static Unit()
        {
            Value = new Unit();
        }

        public static Unit Value { get; private set; }
    }

#if PRABIR_ASYNC_INTERNAL
    internal
#else
    public
#endif
 class Async<T> : IAsync
    {
        protected T result;
        protected bool isCompleted;
        protected Exception exception;

        Action<ErrorFunc<T>> func;

        public Async(Action<ErrorFunc<T>> function)
        {
            this.func = function;
        }

        public Async(Func<byte[], int, int> begin, Func<IAsyncResult, T> end, byte[] arg1, int arg2, int arg3, object state)
        {
            this.func = (cont) => begin(arg1, arg2, arg3, ar =>
                                                                   {
                                                                       try
                                                                       {
                                                                           cont(end(ar), null);
                                                                       }
                                                                       catch (Exception ex)
                                                                       {
                                                                           cont(default(T), ex);
                                                                       }
                                                                   }, state);
        }

        public Async(Func<byte[], int, int> begin, Action<IAsyncResult> end, byte[] arg1, int arg2, int arg3, object state)
        {
            this.func = (cont) => begin(arg1, arg2, arg3, ar =>
            {
                try
                {
                    end(ar);
                    cont(default(T), null);
                }
                catch (Exception ex)
                {
                    cont(default(T), ex);
                }
            }, state);
        }

        public Async(Func begin, Func<IAsyncResult, T> end)
        {
            this.func = (cont) => begin(ar =>
                                            {
                                                try
                                                {
                                                    cont(end(ar), null);
                                                }
                                                catch (Exception ex)
                                                {
                                                    cont(default(T), ex);
                                                }
                                            }, null);
        }

        public void ExecuteStep(Action<Exception> nextAction)
        {
            func((res, ex) =>
                     {
                         this.result = res;
                         this.isCompleted = true;
                         this.exception = ex;
                         nextAction(ex);
                     });
        }

        public T Result
        {
            get
            {
                if (!this.isCompleted)
                {
                    throw new Exception("Operation not completed, did you forgot 'yield return'?");
                }
                return result;
            }
        }

        public Exception Exception
        {
            get
            {
                //if (!isCompleted) throw new Exception("Operation not completed, did you forgot 'yield return'?");
                return this.exception;
            }
            set { this.exception = value; }
        }
    }

#if PRABIR_ASYNC_INTERNAL
    internal
#else
    public
#endif
 static class Async
    {
        internal static void Run(IEnumerator<IAsync> en, Action<Exception> cont)
        {
            try
            {
                if (!en.MoveNext())
                {
                    cont(null);
                    return;
                }

                en.Current.ExecuteStep
                (ex =>
                {
                    en.Current.Exception = ex;
                    Run(en, cont);
                });
            }
            catch (Exception e)
            {
                cont(e);
                return;
            }
        }

        public static void ExecuteAndWait(IEnumerable<IAsync> en)
        {
            var wh = new System.Threading.ManualResetEvent(false);
            Run(en.GetEnumerator(),
                e => { wh.Set(); });
            wh.WaitOne();
        }

        public static Async<T> FromAsync<T>(Func<byte[], int, int> beginMethod, Func<IAsyncResult, T> endMethod, byte[] arg1, int args2, int args3, object state)
        {
            return new Async<T>(beginMethod, endMethod, arg1, args2, args3, state);
        }

        public static Async<Unit> FromAsync(Func<byte[], int, int> beginMethod, Action<IAsyncResult> endMethod, byte[] args1, int args2, int args3, object state)
        {
            return new Async<Unit>(beginMethod, endMethod, args1, args2, args3, state);
        }

        public static Async<T> FromAsync<T>(Func begin, Func<IAsyncResult, T> end)
        {
            return new Async<T>(begin, end);
        }
    }
}