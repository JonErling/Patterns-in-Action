using System.Windows.Threading;
using System.ComponentModel;
using System.Diagnostics;

namespace WpfApp.Models
{
   
    // bbstract base class for business object models

    public abstract class BaseModel : INotifyPropertyChanged
    {

        // dispatcher associated with model

        protected Dispatcher Dispatcher;

        private PropertyChangedEventHandler _propertyChangedEvent;

        public BaseModel()
        {
            // save off dispatcher 

            Dispatcher = Dispatcher.CurrentDispatcher;
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                ConfirmOnUiThread();
                _propertyChangedEvent += value;
            }
            remove
            {
                ConfirmOnUiThread();
                _propertyChangedEvent -= value;
            }
        }

        
        // utility function for use by subclasses to notify that a property value has changed

        protected void Notify(string propertyName)
        {
            ConfirmOnUiThread();
            ConfirmPropertyName(propertyName);

            if (_propertyChangedEvent != null)
            {
                _propertyChangedEvent(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        
        // debugging facility that ensures methods are called on the UI thread
        
        [Conditional("Debug")]
        protected void ConfirmOnUiThread()
        {
            Debug.Assert(Dispatcher.CurrentDispatcher == Dispatcher, "Call must be made on UI thread.");
        }

        
        // debugging facility that ensures the property does exist on the class

        [Conditional("Debug")]
        private void ConfirmPropertyName(string propertyName)
        {
            Debug.Assert(GetType().GetProperty(propertyName) != null, "Property " + propertyName + " is not a valid name.");
        }
    }
}


