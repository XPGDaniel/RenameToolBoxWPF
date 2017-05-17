using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RenameToolbox.Class
{
    public class Rule : ObservableCollection<Rule> , INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler RulePropertyChanged;
        bool m_Enable;
        string m_Target;
        string m_Mode;
        string m_p1;
        string m_p2;
        string m_Sub;
        public bool Enable
        {
            get { return m_Enable; }
            set
            {
                if (m_Enable != value)
                {
                    m_Enable = value;
                    OnPropertyChanged("Enable");
                }
            }
        }
        public string Target
        {
            get { return m_Target; }
            set
            {
                if (m_Target != value)
                {
                    m_Target = value;
                    OnPropertyChanged("Target");
                }
            }
        }
        public string Mode
        {
            get { return m_Mode; }
            set
            {
                if (m_Mode != value)
                {
                    m_Mode = value;
                    OnPropertyChanged("Mode");
                }
            }
        }
        public string p1
        {
            get { return m_p1; }
            set
            {
                if (m_p1 != value)
                {
                    m_p1 = value;
                    OnPropertyChanged("p1");
                }
            }
        }
        public string p2
        {
            get { return m_p2; }
            set
            {
                if (m_p2 != value)
                {
                    m_p2 = value;
                    OnPropertyChanged("p2");
                }
            }
        }
        public string Sub
        {
            get { return m_Sub; }
            set
            {
                if (m_Sub != value)
                {
                    m_Sub = value;
                    OnPropertyChanged("Sub");
                }
            }
        }

        void OnPropertyChanged(string propertyName)
        {
            if (RulePropertyChanged != null)
            {
                RulePropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }        
    }
}
