using CutMkv.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CutMkv.View
{
    /// <summary>
    /// Logique d'interaction pour PopupConfirmation.xaml
    /// </summary>
    public partial class PopupConfirmation : UserControl
    {
        public PopupConfirmation(string message, MessageBoxButton messageBoxButton)
        {
            InitializeComponent();

            textBlockMessage.Text = message;

            if (messageBoxButton == MessageBoxButton.OK)
            {
                buttonValider.Visibility = Visibility.Collapsed;
                buttonAnnuler.Visibility = Visibility.Collapsed;
            }
            else if (messageBoxButton == MessageBoxButton.YesNo)
            {
                buttonOK.Visibility = Visibility.Collapsed;
            }
            else
            {
                throw new Exception("Type de MessageBox non pris en charge");
            }
        }
    }
}
