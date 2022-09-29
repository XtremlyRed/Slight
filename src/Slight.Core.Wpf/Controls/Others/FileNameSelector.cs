using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Slight.Core
{
    public delegate void FileNameSelectionChanged(object sender, string fileName);


    public class FileNameSelector : ContentControl
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly System.Windows.Forms.SaveFileDialog saveFileDialog = new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private const FrameworkPropertyMetadataOptions defaultOptions = FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Inherits;

        static FileNameSelector()
        {
            PropertyAssist.DefaultStyle<FileNameSelector>(DefaultStyleKeyProperty);
        }


        public FileNameSelector()
        {
            bool canPopup = false;
            Content = nameof(FileNameSelector);
            Cursor = System.Windows.Input.Cursors.Hand;
            MouseLeftButtonDown += (s, e) =>
            {
                canPopup = true;
            };
            MouseLeave += (s, e) =>
            {
                canPopup = false;
            };
            MouseLeftButtonUp += (s, e) =>
            {
                if (canPopup == false)
                {
                    return;
                }
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileName = FileName = saveFileDialog.FileName;

                    FileNameSelectionChanged?.Invoke(this, fileName);

                    FileNameSelectionChangedCommand?.Execute(fileName);
                }
            };
        }

        public event FileNameSelectionChanged FileNameSelectionChanged;

        public static readonly DependencyProperty FileNameSelectionChangedCommandProperty =
                 PropertyAssist.PropertyRegister<FileNameSelector, ICommand<string>>(i => i.FileNameSelectionChangedCommand, null, FrameworkPropertyMetadataOptions.Inherits, (s, e) => { });

        [Bindable(true), Category("ICommand")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public ICommand<string> FileNameSelectionChangedCommand
        {
            get => (ICommand<string>)GetValue(FileNameSelectionChangedCommandProperty);
            set => SetValue(FileNameSelectionChangedCommandProperty, value);
        }





        public static readonly DependencyProperty TitleProperty =
            PropertyAssist.PropertyRegister<FileNameSelector, string>(i => i.Title, "", defaultOptions, (s, e) =>
            {
                s.saveFileDialog.Title = e.NewValue;
            });

        [Bindable(true), Category("Title")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }


        public static readonly DependencyProperty FileNameProperty =
          PropertyAssist.PropertyRegister<FileNameSelector, string>(i => i.FileName, "", defaultOptions, (s, e) =>
          {
              s.saveFileDialog.FileName = e.NewValue;
          });

        [Bindable(true), Category("FileName")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public string FileName
        {
            get => (string)GetValue(FileNameProperty);
            set => SetValue(FileNameProperty, value);
        }



        public static readonly DependencyProperty FileNamesProperty =
        PropertyAssist.PropertyRegister<FileNameSelector, string[]>(i => i.FileNames, null, defaultOptions, (s, e) => { });

        [Bindable(true), Category("FileNames")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public string[] FileNames
        {
            get => (string[])GetValue(FileNamesProperty);
            set => SetValue(FileNamesProperty, value);
        }



        public static readonly DependencyProperty DefaultExtProperty =
          PropertyAssist.PropertyRegister<FileNameSelector, string>(i => i.DefaultExt, "", defaultOptions, (s, e) =>
          {
              s.saveFileDialog.DefaultExt = e.NewValue;
          });


        [Bindable(true), Category("DefaultExt")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public string DefaultExt
        {
            get => (string)GetValue(DefaultExtProperty);
            set => SetValue(DefaultExtProperty, value);
        }

        public static readonly DependencyProperty FilterProperty =
          PropertyAssist.PropertyRegister<FileNameSelector, string>(i => i.Filter, "", defaultOptions, (s, e) =>
          {
              s.saveFileDialog.Filter = e.NewValue;
          });

        [Bindable(true), Category("Filter")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public string Filter
        {
            get => (string)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }


        public static readonly DependencyProperty InitialDirectoryProperty =
          PropertyAssist.PropertyRegister<FileNameSelector, string>(i => i.InitialDirectory, null, defaultOptions, (s, e) =>
          {
              s.saveFileDialog.InitialDirectory = e.NewValue;
          });

        [Bindable(true), Category("InitialDirectory")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public string InitialDirectory
        {
            get => (string)GetValue(InitialDirectoryProperty);
            set => SetValue(InitialDirectoryProperty, value);
        }


        public static readonly DependencyProperty FilterIndexProperty =
          PropertyAssist.PropertyRegister<FileNameSelector, int>(i => i.FilterIndex, default, defaultOptions, (s, e) =>
          {
              s.saveFileDialog.FilterIndex = e.NewValue;
          });

        [Bindable(true), Category("FilterIndex")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public int FilterIndex
        {
            get => (int)GetValue(FilterIndexProperty);
            set => SetValue(FilterIndexProperty, value);
        }




        public static readonly DependencyProperty CornerRadiusProperty = PropertyAssist.PropertyRegister<FileNameSelector, CornerRadius>(i => i.CornerRadius, new CornerRadius(0));

        [Bindable(true), Category("CornerRadius")]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }




        //public static readonly DependencyProperty PressedBackgroundProperty =
        //    PropertyAssist.PropertyRegister<FileNameSelector, Brush>(i => i.PressedBackground, Brushes.Transparent);

        //[Bindable(true), Category("PressedBackground")]
        //[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        //public Brush PressedBackground
        //{
        //    get => (Brush)GetValue(PressedBackgroundProperty);
        //    set => SetValue(PressedBackgroundProperty, value);
        //}



    }
}
