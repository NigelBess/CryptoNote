using System;
using Protocol;

namespace Application
{
    abstract class HandlesExceptions
    {
        public Action<Exception> ErrorCaught;
        protected void OnError(string message)
        {
            OnError(new Exception(message));
        }
        protected void OnError(Exception e)
        {
            ErrorCaught?.Invoke(e);
        }

        protected void SafeExecute(Action action, params byte[][] plaintext)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                OnError(e);
            }
            finally
            {
                foreach (var item in plaintext) item?.Wipe();
            }
        }
    }
}
