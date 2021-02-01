using System.Collections.Generic;
using System.ComponentModel;

namespace Application
{
    abstract class ViewModel : Notifier
    {
        public void Bind(INotifyPropertyChanged model, string modelProperty, params string[] ownPropertiesAffected)
        {
            foreach (var ownProperty in ownPropertiesAffected)
            {
                Bind(model, modelProperty, ownProperty);
            }
        }

        private readonly Dictionary<INotifyPropertyChanged, HashSet<PropertyChangedEventHandler>> _modelBindings = new ();

        protected void Unbind(INotifyPropertyChanged model)
        {
            if (!_modelBindings.TryGetValue(model, out var hashSet)) return;
            foreach (var binding in hashSet)
            {
                model.PropertyChanged -= binding;
            }
        }

        public void Bind(INotifyPropertyChanged model, string modelProperty, string ownProperty)
        {
            var binding = new PropertyChangedEventHandler( (o, e) =>
            {
                if (e.PropertyName == modelProperty) OnChange(ownProperty);
            });
            if (!_modelBindings.TryGetValue(model, out var hashSet))
            {
                hashSet = new HashSet<PropertyChangedEventHandler>();
                _modelBindings[model] = hashSet;
            }
            model.PropertyChanged += binding;
            hashSet.Add(binding);
            OnChange(ownProperty);
        }
    }
}
