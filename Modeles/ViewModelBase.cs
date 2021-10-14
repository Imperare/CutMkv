using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace CutMkv.Modeles
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] != null)
                return;
            Debug.Fail("Invalid property name: " + propertyName);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;
            propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public KeyValuePair<string, List<string>> BuildPropertyForRaise(string property,
                                                                        params string[] propertiesRaise)
        {
            return new KeyValuePair<string, List<string>>(property, ((IEnumerable<string>)propertiesRaise).ToList());
        }

        private void DoPropertyChangedForRaise<T>(T item,
                                                  params KeyValuePair<string,
                                                  List<string>>[] propertiesDictionnary)
                                                  where T : INotifyPropertyChanged
        {
            item.PropertyChanged += (sender, e) =>
           {
               foreach (KeyValuePair<string, List<string>> keyValuePair in propertiesDictionnary)
               {
                   if (e.PropertyName == keyValuePair.Key)
                   {
                       foreach (string propertyName in keyValuePair.Value)
                           RaisePropertyChanged(propertyName);
                   }
               }
           };
        }

        public void RaiseCollectionItemsChanged<T>(ObservableCollection<T> collection,
                                                   params string[] propertiesRaise)
        {
            if (collection == null)
                return;
            collection.CollectionChanged += (sender, e) =>
                {
                    foreach (string propertyName in ((IEnumerable<string>)propertiesRaise).ToList())
                        RaisePropertyChanged(propertyName);
                };
        }
    }
}
