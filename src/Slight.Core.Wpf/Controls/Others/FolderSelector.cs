using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
namespace Slight.Core
{


    public delegate void FolderSelectionChanged(object sender, string folderPath);


    public class FolderSelector : ContentControl
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private const FrameworkPropertyMetadataOptions defaultOptions = FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Inherits;


        static FolderSelector()
        {
            PropertyAssist.DefaultStyle<FolderSelector>(DefaultStyleKeyProperty);
        }

        public FolderSelector()
        {
            Content = nameof(FolderSelector);
            bool canPopup = false;
            Cursor = System.Windows.Input.Cursors.Hand;
            MouseLeftButtonDown += (s, e) => canPopup = true;
            MouseLeave += (s, e) => canPopup = false;
            MouseLeftButtonUp += (s, e) =>
            {
                if (canPopup == false)
                {
                    return;
                }

                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string sp = SelectedPath = folderBrowserDialog.SelectedPath;

                    FolderSelectionChanged?.Invoke(this, sp);

                    FolderSelectionChangedCommand?.Execute(sp);
                }
            };
        }


        public event FolderSelectionChanged FolderSelectionChanged;


        public static readonly DependencyProperty FolderSelectionChangedCommandProperty =
                PropertyAssist.PropertyRegister<FolderSelector, ICommand<string>>(i => i.FolderSelectionChangedCommand, null, FrameworkPropertyMetadataOptions.Inherits, (s, e) => { });

        [Bindable(true), Category("ICommand")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public ICommand<string> FolderSelectionChangedCommand
        {
            get => (ICommand<string>)GetValue(FolderSelectionChangedCommandProperty);
            set => SetValue(FolderSelectionChangedCommandProperty, value);
        }

        public static readonly DependencyProperty DescriptionProperty =
          PropertyAssist.PropertyRegister<FolderSelector, string>(i => i.Description, "", defaultOptions, (s, e) =>
          {
              s.folderBrowserDialog.Description = e.NewValue;
          });

        [Bindable(true), Category("Description")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }



        public static readonly DependencyProperty ShowNewFolderButtonProperty =
          PropertyAssist.PropertyRegister<FolderSelector, bool>(i => i.ShowNewFolderButton, false, defaultOptions, (s, e) =>
          {
              s.folderBrowserDialog.ShowNewFolderButton = e.NewValue;
          });

        [Bindable(true), Category("ShowNewFolderButton")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public bool ShowNewFolderButton
        {
            get => (bool)GetValue(ShowNewFolderButtonProperty);
            set => SetValue(ShowNewFolderButtonProperty, value);
        }


        public static readonly DependencyProperty SelectedPathProperty =
       PropertyAssist.PropertyRegister<FolderSelector, string>(i => i.SelectedPath, "", defaultOptions, (s, e) =>
       {
           s.folderBrowserDialog.SelectedPath = e.NewValue;
       });

        [Bindable(true), Category("SelectedPath")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public string SelectedPath
        {
            get => (string)GetValue(SelectedPathProperty);
            set => SetValue(SelectedPathProperty, value);
        }


        public static readonly DependencyProperty RootFolderProperty =
            PropertyAssist.PropertyRegister<FolderSelector, Environment.SpecialFolder>(i => i.RootFolder, Environment.SpecialFolder.Desktop, defaultOptions, (s, e) =>
            {
                s.folderBrowserDialog.RootFolder = e.NewValue;
            });

        [Bindable(true), Category("SelectedPath")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public Environment.SpecialFolder RootFolder
        {
            get => (Environment.SpecialFolder)GetValue(RootFolderProperty);
            set => SetValue(RootFolderProperty, value);
        }



        public static readonly DependencyProperty UseDescriptionForTitleProperty =
          PropertyAssist.PropertyRegister<FolderSelector, bool>(i => i.UseDescriptionForTitle, false, defaultOptions, (s, e) =>
          {
              s.folderBrowserDialog.ShowNewFolderButton = e.NewValue;
          });

        [Bindable(true), Category("ShowNewFolderButton")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public bool UseDescriptionForTitle
        {
            get => (bool)GetValue(UseDescriptionForTitleProperty);
            set => SetValue(UseDescriptionForTitleProperty, value);
        }


        public void Reset()
        {
            folderBrowserDialog.Reset();
        }


        public static readonly DependencyProperty CornerRadiusProperty =
            PropertyAssist.PropertyRegister<FolderSelector, CornerRadius>(i => i.CornerRadius, new CornerRadius(0));

        [Bindable(true), Category("CornerRadius")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}
