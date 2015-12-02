using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using iChat.Client.Models;
using iChat.Client.ServiceReference;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;

namespace iChat.Client.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase, IChatContractsCallback
    {
        private ChatContractsClient _chatSvc;

        #region Observable Properties

        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();

        public ObservableCollection<Message> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                RaisePropertyChanged("Messages");
            }
        }

        private string _text;

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                RaisePropertyChanged("Text");
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        private bool _cnxBtnEnabled = true;

        public bool CnxBtnEnabled
        {
            get { return _cnxBtnEnabled; }
            private set
            {
                _cnxBtnEnabled = value;
                RaisePropertyChanged("CnxBtnEnabled");
            }
        }

        private bool _connected = false;

        public bool Connected
        {
            get { return _connected; }
            set
            {
                _connected = value;
                if (_connected)
                    CnxBtnEnabled = false;
                else
                    CnxBtnEnabled = true;
                RaisePropertyChanged("Connected");
            }
        }

        #endregion Observable Properties

        #region Commands

        private ICommand _disconnectCmd;

        public ICommand DisconnectCmd
        {
            get
            {
                if (_disconnectCmd == null)
                    _disconnectCmd = new RelayCommand(
                        () => _Disconnect(),
                        () => { return Connected; });
                return _disconnectCmd;
            }
        }

        private ICommand _appExitCmd;

        public ICommand ApplicationExitCmd
        {
            get
            {
                if (_appExitCmd == null)
                    _appExitCmd = new RelayCommand(() => Application.Current.Shutdown());
                return _appExitCmd;
            }
        }

        private ICommand _connectCmd;

        public ICommand ConnectCmd
        {
            get
            {
                if (_connectCmd == null)
                    _connectCmd = new RelayCommand(
                        () => _Connect(),
                        () => !Connected && !String.IsNullOrWhiteSpace(Name));
                return _connectCmd;
            }
        }

        private ICommand _sendMsgCmd;

        public ICommand SendMsgCmd
        {
            get
            {
                if (_sendMsgCmd == null)
                    _sendMsgCmd = new RelayCommand(
                        () => _SendMessage(Text),
                        () => { return Connected; });
                return _sendMsgCmd;
            }
        }

        private ICommand _resetTxtCmd;

        public ICommand ResetTxtCmd
        {
            get
            {
                if (_resetTxtCmd == null)
                    _resetTxtCmd = new RelayCommand(
                        () => Text = String.Empty,
                        () => { return !String.IsNullOrWhiteSpace(Text); });
                return _resetTxtCmd;
            }
        }

        #endregion Commands

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        { }

        #region Private Methods

        private void _Connect()
        {
            _chatSvc = new ChatContractsClient(new InstanceContext(this));
            _chatSvc.Open();
            _chatSvc.StartSession(Name);
            Connected = true;
        }

        private void _Disconnect()
        {
            _chatSvc.StopSession();
            _chatSvc.Close();
            Connected = false;
        }

        private void _SendMessage(string msg)
        {
            _chatSvc.SendMessage(msg);
        }

        #endregion Private Methods

        #region IChatContractsCallback members

        public void UserConnected(string name)
        {
            var msg = new Message
            {
                Content = "User : " + name + " connected",
                Timestamp = DateTime.Now
            };
            Messages.Add(msg);
            var debug = true;
        }

        public void UserDisconnected(string name)
        {
            var msg = new Message
            {
                Content = "User : " + name + " disconnected",
                Timestamp = DateTime.Now
            };
            _messages.Add(msg);
        }

        public void MessageReceived(string pseudo, string message)
        {
            var msg = new Message
            {
                Author = pseudo,
                Content = message,
                Timestamp = DateTime.Now
            };
            _messages.Add(msg);
        }

        #endregion IChatContractsCallback members
    }
}