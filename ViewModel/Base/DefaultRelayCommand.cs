using System;

namespace KeyboardMainBrowserWindowVideoPlaybackControl.ViewModel.Base
{
    public class DefaultRelayCommand : RelayCommand<object>
    {
        public DefaultRelayCommand(Action<object> execute, Func<object, bool> canExecute = null) : base(execute, canExecute)
        {

        }
    }
}
