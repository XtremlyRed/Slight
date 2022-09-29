using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Slight.Core
{
    public class Messenger : IMessenger
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ConcurrentDictionary<string, Mapper> mappers = new();

        //#region Default

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)] private static IMessenger defaultInstance;

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)] private static readonly object syncRootLock = new();

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]

        //public static IMessenger Default
        //{
        //    get
        //    {
        //        if (defaultInstance == null)
        //        {
        //            lock (syncRootLock)
        //            {
        //                defaultInstance ??= new Messenger();
        //            }
        //        }

        //        return defaultInstance;
        //    }
        //}

        //#endregion



        public virtual void Unregister(string token)
        {
            mappers.TryRemove(token, out _);
        }

        public virtual void UnregisterAll(object subscriber)
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }

            System.Collections.Generic.KeyValuePair<string, Mapper>[] exists = mappers.Where(i => i.Value.Subscriber == subscriber).ToArray();
            foreach (System.Collections.Generic.KeyValuePair<string, Mapper> item in exists)
            {
                mappers.TryRemove(item.Key, out _);
            }
        }


        public virtual void Publish(string publishToken, params object[] messengerParamters)
        {
            if (string.IsNullOrWhiteSpace(publishToken))
            {
                throw new ArgumentNullException(nameof(publishToken));
            }

            if (mappers.TryGetValue(publishToken, out Mapper mapper))
            {
                messengerParamters ??= new object[] { null };

                //Type[] paramTypes = messageParams?.Where(i => i != null).Select(i => i.GetType()).ToArray(); 
                //if (Enumerable.SequenceEqual(mapper.Arguments, paramTypes) == false)
                //{
                //    throw new ArgumentException("Parameters error or abnormal sequence");
                //} 
                //if (string.Compare(mapper.ReturnType.Name, "Void", true) != 0)
                //{
                //    throw new ArgumentException("Return value error or type exception");
                //}

                mapper.Invoke(messengerParamters);

                return;
            }

            throw new ArgumentException("Token does not exist");
        }

        public virtual TResult Publish<TResult>(string publishToken, params object[] messengerParamters)
        {
            if (string.IsNullOrWhiteSpace(publishToken))
            {
                throw new ArgumentNullException(nameof(publishToken));
            }

            if (mappers.TryGetValue(publishToken, out Mapper mapper))
            {
                //Type[] paramTypes = messageParams?.Where(i => i != null).Select(i => i.GetType()).ToArray();

                //if (Enumerable.SequenceEqual(mapper.Arguments, paramTypes) == false)
                //{
                //    throw new ArgumentException("Parameters error or abnormal sequence");
                //}

                if (Equals(mapper.ReturnType, typeof(TResult)) == false)
                {
                    throw new ArgumentException("return type error or type exception");
                }

                messengerParamters ??= new object[] { null };

                object invokerValue = mapper.Invoke(messengerParamters);

                return invokerValue is TResult returnValue ? returnValue : default;
            }

            throw new ArgumentException("Token does not exist");

        }

        public virtual Task PublishAsync(string token, params object[] messageParams)
        {
            return string.IsNullOrWhiteSpace(token)
                ? throw new ArgumentNullException(nameof(token))
                : Task.Factory.StartNew(() =>
            {
                Publish(token, messageParams);
            }, System.Threading.CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }

        public virtual Task<TResult> PublishAsync<TResult>(string token, params object[] messageParams)
        {
            return string.IsNullOrWhiteSpace(token)
                ? throw new ArgumentNullException(nameof(token))
                : Task.Factory.StartNew(() =>
            {
                return Publish<TResult>(token, messageParams);
            }, System.Threading.CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }

        public virtual void Subscribe<TResult>(object subscriber, string token, Func<TResult> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }


            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe(object subscriber, string token, Action subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage, TResult>(object subscriber, string token, Func<TMessage, TResult> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage>(object subscriber, string token, Action<TMessage> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TResult>(object subscriber, string token, Func<TMessage1, TMessage2, TResult> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2>(object subscriber, string token, Action<TMessage1, TMessage2> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TResult>(object subscriber, string token, Func<TMessage1, TMessage2, TMessage3, TResult> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TMessage3>(object subscriber, string token, Action<TMessage1, TMessage2, TMessage3> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TResult>(object subscriber, string token, Func<TMessage1, TMessage2, TMessage3, TMessage4, TResult> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4>(object subscriber, string token, Action<TMessage1, TMessage2, TMessage3, TMessage4> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TResult>(object subscriber, string token, Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TResult> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5>(object subscriber, string token, Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TResult>(object subscriber, string token, Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TResult> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6>(object subscriber, string token, Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TResult>(object subscriber, string token, Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TResult> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7>(object subscriber, string token, Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TResult>(object subscriber, string token, Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TResult> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8>(object subscriber, string token, Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TResult>(object subscriber, string token, Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TResult> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9>(object subscriber, string token, Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TMessage10, TResult>(object subscriber, string token, Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TMessage10, TResult> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TMessage10>(object subscriber, string token, Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TMessage10> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TMessage10, TMessage11, TResult>(object subscriber, string token, Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TMessage10, TMessage11, TResult> subscribeDelegate)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TMessage10, TMessage11>(object subscriber, string token, Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TMessage10, TMessage11> subscribeDelegate)
        {

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (mappers.TryGetValue(token, out _))
            {
                throw new ArgumentException("Token already exists");
            }

            object subscriber1 = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
            Mapper mapper = new(subscriber1, token, method, subscribeDelegate);
            mappers[token] = mapper;
        }

        private class Mapper
        {
            private readonly object Handler;
            private readonly MethodInfo Method;
            public Mapper(object subscriber, string token, MethodInfo method, object handler)
            {
                Token = token;
                Subscriber = subscriber;
                Arguments = method.GetParameters().Select(i => i.ParameterType).ToArray();
                ReturnType = method.ReturnType;
                Handler = handler;
                Method = Handler.GetType().GetMethod(nameof(MethodInfo.Invoke));
            }

            public string Token { get; }

            public object Subscriber { get; }

            public Type[] Arguments { get; }

            public Type ReturnType { get; }


            public object Invoke(params object[] messageParams)
            {
                object invokerValue = Method?.Invoke(Handler, messageParams);
                return invokerValue;
            }
        }
    }
}
