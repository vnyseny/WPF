namespace WPF;

internal partial class UserViewModel
{
    private string _pdfPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDetails.pdf");
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private string _address = string.Empty;
    private string _mobileNumber = string.Empty;
    private string _email = string.Empty;
    private DateTime? _dateOfBirth;
    private bool _IsFormSubmited = false;

    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            OnPropertyChanged(nameof(FirstName));
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            OnPropertyChanged(nameof(LastName));
        }
    }

    public string Address
    {
        get => _address;
        set
        {
            _address = value;
            OnPropertyChanged(nameof(Address));
        }
    }

    public string MobileNumber
    {
        get => _mobileNumber;
        set
        {
            _mobileNumber = value;
            OnPropertyChanged(nameof(MobileNumber));
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }

    public DateTime? DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            _dateOfBirth = value;
            OnPropertyChanged(nameof(DateOfBirth));
        }
    }

    public ActionCommand SubmitCommand { get; }
    public ActionCommand SavePdfCommand { get; }

    public UserViewModel()
    {
        SubmitCommand = new ActionCommand(Submit, CanSubmit);
        SavePdfCommand = new ActionCommand(SavePdf, CanSavePdf);
    }

    private bool CanSubmit() => !HasErrors;

    private void ValidateProperites()
    {
        var properties = this.GetType().GetProperties();
        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(string) || property.PropertyType == typeof(DateTime?))
            {
                var _ = this[property.Name];
                OnErrorsChanged(property.Name);
            }
        }
    }

    private void Submit()
    {
        ValidateProperites();
        if (!HasErrors)
        {
            GeneratePdf();
            DisplayPdf();
            _IsFormSubmited = true;
        }
        else
        {
            SubmitCommand.OnCanExecuteChanged(this);
        }
        SavePdfCommand.OnCanExecuteChanged(this);
    }

    private void GeneratePdf()
    {
        using (FileStream fs = new FileStream(_pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, fs);

            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA, 20, Font.BOLD);
            Font contentFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.NORMAL);

            document.Open();
            document.Add(new Paragraph("User Details", titleFont));
            document.Add(new Paragraph($"Name: {FirstName} {LastName}", contentFont));
            document.Add(new Paragraph($"Address: {Address}", contentFont));
            document.Add(new Paragraph($"Mobile: {MobileNumber}", contentFont));
            document.Add(new Paragraph($"Email: {Email}", contentFont));
            document.Add(new Paragraph($"Date of Birth: {DateOfBirth?.ToShortDateString()}", contentFont));

            document.Close();
            writer.Close();
        }
    }

    private void DisplayPdf()
    {
        if (Application.Current.MainWindow is MainWindow mainWindow)
        {
            mainWindow.PdfViewer.Height = 400;
            mainWindow.PdfViewer.Source = new Uri(_pdfPath);
        }
    }

    private void SavePdf()
    {
        if (!_IsFormSubmited)
        {
            MessageBox.Show("Please submit your details.", "No Pdf Generated", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (File.Exists(_pdfPath))
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "UserDetails",
                DefaultExt = ".pdf",
                Filter = "PDF documents (.pdf)|*.pdf"
            };

            if (dlg.ShowDialog() ?? false)
            {
                string filename = dlg.FileName;
                System.IO.File.Copy(_pdfPath, filename, true);
                MessageBox.Show("PDF saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        else
        {
            MessageBox.Show("The file has been deleted. Please submit your details again.", "File Deleted", MessageBoxButton.OK, MessageBoxImage.Error);
            _IsFormSubmited = false;
            SavePdfCommand.OnCanExecuteChanged(this);
        }
    }

    private bool CanSavePdf() => !string.IsNullOrEmpty(_pdfPath) && _IsFormSubmited;
}