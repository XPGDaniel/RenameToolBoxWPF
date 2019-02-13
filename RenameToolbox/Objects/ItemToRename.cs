using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RenameToolbox.Class
{
    public class ItemToRename : ObservableCollection<ItemToRename>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler ItemPropertyChanged;
        string m_Path;
        TimeSpan? m_Duration;
        string m_Before;
        string m_After;
        string m_Result;
        public string Path
        {
            get { return m_Path; }
            set
            {
                if (m_Path != value)
                {
                    m_Path = value;
                    OnPropertyChanged("Path");
                }
            }
        }
        public TimeSpan? Duration
        {
            get { return m_Duration; }
            set
            {
                if (m_Duration != value)
                {
                    m_Duration = value;
                    //OnPropertyChanged("Duration");
                }
            }
        }
        public string Before
        {
            get { return m_Before; }
            set
            {
                if (m_Before != value)
                {
                    m_Before = value;
                    OnPropertyChanged("Before");
                }
            }
        }
        public string After
        {
            get { return m_After; }
            set
            {
                if (m_After != value)
                {
                    m_After = value;
                    OnPropertyChanged("After");
                }
            }
        }
        public string Result
        {
            get { return m_Result; }
            set
            {
                if (m_Result != value)
                {
                    m_Result = value;
                    OnPropertyChanged("Result");
                }
            }
        }
        void OnPropertyChanged(string propertyName)
        {
            if (ItemPropertyChanged != null)
            {
                ItemPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
