using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace CutMkv.Modeles
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> m_execute;
        private readonly Predicate<object> m_canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            m_execute = execute;
            m_canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return m_canExecute == null || m_canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            try
            {
                object targetVm = Convert.ChangeType(m_execute.Target, Type.GetType(m_execute.Method.DeclaringType.AssemblyQualifiedName));
                PropertyInfo propertyInfo = ((IEnumerable<PropertyInfo>)targetVm.GetType().GetProperties()).FirstOrDefault(p => p.GetIndexParameters().Length == 0 && p.GetValue(targetVm, null) == this);
                if (propertyInfo != null)
                {
                    string name = propertyInfo.Name;
                    if (parameter != null)
                    {
                        string str = name + ", paramètre : " + parameter.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            m_execute(parameter ?? "<N/A>");
        }
    }
}
