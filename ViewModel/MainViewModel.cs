using MaterialDesignThemes.Wpf;
using Microsoft.WindowsAPICodePack.Dialogs;
using CutMkv.Helpers;
using CutMkv.Modeles;
using CutMkv.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CutMkv.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Singleton

        private static readonly Lazy<MainViewModel> m_instance = new Lazy<MainViewModel>(() => new MainViewModel());
        public static MainViewModel Instance => m_instance.Value;

        #endregion

        #region Propriétés private

        private string m_logs;
        private string m_emplacementVideo;
        private string m_emplacementSortie;
        private string m_instructionsCut;

        #endregion

        #region Propriétés public

        public ICommand OuvrirRepertoireCommand => new RelayCommand(x => OuvrirRepertoire(x.ToString()));
        public ICommand SelectionVideoCommand => new RelayCommand(x => SelectionVideo());
        public ICommand SelectionSortieCommand => new RelayCommand(x => SelectionSortie());
        public ICommand CouperMkvCommand => new RelayCommand(async x => await CouperMkv(), x => !string.IsNullOrEmpty(InstructionsCut));

        public string EmplacementVideo
        {
            get => m_emplacementVideo;
            set
            {
                m_emplacementVideo = value;
                RaisePropertyChanged(nameof(EmplacementVideo));
                SauveConfig();
            }
        }

        public string EmplacementSortie
        {
            get => m_emplacementSortie;
            set
            {
                m_emplacementSortie = value;
                RaisePropertyChanged(nameof(EmplacementSortie));
                SauveConfig();
            }
        }

        public string InstructionsCut
        {
            get => m_instructionsCut;
            set
            {
                m_instructionsCut = value;
                RaisePropertyChanged(nameof(InstructionsCut));
            }
        }

        public string Logs
        {
            get => m_logs;
            set
            {
                m_logs = value;
                RaisePropertyChanged(nameof(Logs));
            }
        }

        #endregion

        #region Constructeur

        public MainViewModel()
        {
            m_emplacementVideo = Properties.Settings.Default.EmplacementVideo;
            m_emplacementSortie = Properties.Settings.Default.EmplacementSortie;
        }

        #endregion

        #region Fonctions

        private void OuvrirRepertoire(string repertoire)
        {
            switch (repertoire)
            {
                case "Video":
                    Tools.OuvrirExplorerRepertoire(EmplacementVideo);
                    break;
                case "Sortie":
                    Tools.OuvrirExplorerRepertoire(EmplacementSortie);
                    break;
            }
        }

        private void SelectionVideo()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = !string.IsNullOrEmpty(EmplacementVideo) ? EmplacementVideo : Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dialog.Filters.Add(new CommonFileDialogFilter("Fichiers mkv", "*.mkv"));
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;

            EmplacementVideo = dialog.FileName;
        }

        private void SelectionSortie()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = !string.IsNullOrEmpty(EmplacementVideo) ? EmplacementVideo : Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;

            EmplacementSortie = dialog.FileName;
        }

        public void SauveConfig()
        {
            try
            {
                Properties.Settings.Default.EmplacementVideo = m_emplacementVideo;
                Properties.Settings.Default.EmplacementSortie = m_emplacementSortie;
                Properties.Settings.Default.Save();
            }
            catch { }
        }

        private async Task CouperMkv()
        {
            if (!File.Exists("ffmpeg.exe"))
            {
                Log($"Le fichier ffmpeg.exe n'existe pas");
                PopupConfirmation popup = new PopupConfirmation($"Le fichier ffmpg.exe n'existe pas", MessageBoxButton.OK);
                await DialogHost.Show(popup, "EcranPrincipalDialog");
                return;
            }

            if (!File.Exists(m_emplacementVideo))
            {
                Log($"Le fichier vidéo n'existe pas : {EmplacementVideo}");
                PopupConfirmation popup = new PopupConfirmation($"Le fichier vidéo n'existe pas : {EmplacementVideo}", MessageBoxButton.OK);
                await DialogHost.Show(popup, "EcranPrincipalDialog");
                return;
            }

            if (!Directory.Exists(EmplacementSortie))
            {
                Log($"Le répertoire sortie n'existe pas : {EmplacementSortie}");
                PopupConfirmation popup = new PopupConfirmation($"Le répertoire sortie n'existe pas : {EmplacementSortie}", MessageBoxButton.OK);
                await DialogHost.Show(popup, "EcranPrincipalDialog");
                return;
            }

            int index = 0;
            IEnumerable<string> listeTimestamps = InstructionsCut.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (string timestamp in listeTimestamps)
            {
                string[] debutFin = timestamp.Split(' ');
                string nomFichier = $"{EmplacementSortie}\\cut-{DateTime.Now.ToString("yyyyMMdd-HHmmss")}-{index++}.mkv";
                string arguments = $@"-ss {debutFin[0]} -i {EmplacementVideo} -to {debutFin[1]} -c copy {nomFichier}";
                Log($"ffmpeg.exe {arguments}");

                ProcessStartInfo startInfo = new ProcessStartInfo("ffmpeg.exe");
                startInfo.Arguments = arguments;
                Process.Start(startInfo);
            }
        }

        public void Log(string log)
        {
            Logs += $"{Environment.NewLine}{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff")} - {log}";
        }

        #endregion
    }
}
