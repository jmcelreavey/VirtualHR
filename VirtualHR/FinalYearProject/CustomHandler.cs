using Leap;
using System;

namespace FinalYearProject
{
    public class CustomHandler : IDisposable
    {

        public CustomListener Listener { get; set; }
        private readonly Controller _myController;

        public CustomHandler()
        {
            try
            {
                Listener = new CustomListener();
                _myController = new Controller();
                _myController.AddListener(Listener);
                // Now we are safe from exceptions
            }
            catch(Exception)
            {
                if (Listener != null) Listener.Dispose();
                if (_myController != null) _myController.Dispose();
            }
        }

        ~CustomHandler()
        {
            Dispose(false);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _myController.RemoveListener(Listener);
                _myController.Dispose();
                Listener.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
