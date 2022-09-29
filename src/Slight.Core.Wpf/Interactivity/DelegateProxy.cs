using System;
using System.Diagnostics;
using System.Linq;
namespace Slight.Core.Interactivity
{
    internal class DelegateProxy : IDisposable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private EventRaiser eventRaiser;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private object attachObject;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Delegate @delegate;

        public DelegateProxy(object attachObject, EventRaiser eventRaiser)
        {
            this.attachObject = attachObject;
            this.eventRaiser = eventRaiser;
        }

        public void AttacheHandler()
        {

            if (@delegate != null)
            {
                RemoveHandler();
            }

            System.Reflection.EventInfo eventInfo = attachObject.GetType().GetEvent(eventRaiser.EventMame);

            System.Reflection.MethodInfo invokerMethod = eventInfo.EventHandlerType.GetMethod("Invoke");
            if (invokerMethod is null)
            {
                return;
            }

            System.Reflection.ParameterInfo[] pars = invokerMethod.GetParameters();
            System.Reflection.MethodInfo method = GetType().GetMethods()
                .FirstOrDefault(i => i.GetParameters().Length == pars.Length);

            if (method is null)
            {
                return;
            }

            if (method.IsGenericMethod)
            {
                method = method.MakeGenericMethod(pars.Select(i => i.ParameterType).ToArray());
            }

            @delegate = method.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(attachObject, @delegate);

        }

        public void RemoveHandler()
        {
            if (@delegate is null || attachObject is null || eventRaiser is null)
            {
                return;
            }

            System.Reflection.EventInfo eventInfo = attachObject.GetType().GetEvent(eventRaiser.EventMame);
            eventInfo.RemoveEventHandler(attachObject, @delegate);
        }


        public void Dispose()
        {
            RemoveHandler();
            attachObject = null;
            @delegate = null;
            eventRaiser = null;
        }

        private void ComandInvoker(params object[] objects)
        {

            if (eventRaiser.PushEventArgsToCommandParameter)
            {
                EventArgs eventArg = objects?.OfType<EventArgs>().FirstOrDefault();

                eventRaiser.Command.Execute(eventArg);
                return;
            }
            eventRaiser.Command.Execute(eventRaiser.CommandParameter);

        }

        #region function

        public void ComandEventArgsInvoker()
        {
            ComandInvoker();
        }

        public void ComandEventArgsInvoker<T1>(T1 t1)
        {
            ComandInvoker(t1);
        }

        public void ComandEventArgsInvoker<T1, T2>(T1 t1, T2 t2)
        {
            ComandInvoker(t1, t2);
        }

        public void ComandEventArgsInvoker<T1, T2, T3>(T1 t1, T2 t2, T3 t3)
        {
            ComandInvoker(t1, t2, t3);
        }

        public void ComandEventArgsInvoker<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4)
        {
            ComandInvoker(t1, t2, t3, t4);
        }

        public void ComandEventArgsInvoker<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            ComandInvoker(t1, t2, t3, t4, t5);
        }

        public void ComandEventArgsInvoker<T1, T2, T3, T4, T5, T6>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            ComandInvoker(t1, t2, t3, t4, t5, t6);
        }

        public void ComandEventArgsInvoker<T1, T2, T3, T4, T5, T6, T7>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        {
            ComandInvoker(t1, t2, t3, t4, t5, t6, t7);
        }

        public void ComandEventArgsInvoker<T1, T2, T3, T4, T5, T6, T7, T8>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
        {
            ComandInvoker(t1, t2, t3, t4, t5, t6, t7, t8);
        }

        public void ComandEventArgsInvoker<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
        {
            ComandInvoker(t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }

        public void ComandEventArgsInvoker<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
        {
            ComandInvoker(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }

        public void ComandEventArgsInvoker<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
        {
            ComandInvoker(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }

        #endregion
    }
}
