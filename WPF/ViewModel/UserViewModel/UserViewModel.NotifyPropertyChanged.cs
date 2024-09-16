namespace WPF;

internal partial class UserViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        var _ = this[propertyName];
        SubmitCommand.OnCanExecuteChanged(this);
    }
}