namespace WPF;

internal partial class UserViewModel : INotifyDataErrorInfo
{
    public bool HasErrors => _Errors.Any();

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName)
    {
        var errors = new List<string>();

        if (!string.IsNullOrEmpty(propertyName) && _Errors.TryGetValue(propertyName, out string? error))
        {
            errors.Add(error);
        }

        return errors;
    }

    private void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
}