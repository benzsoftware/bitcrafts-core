using System.ComponentModel.DataAnnotations;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BitCrafts.Application.Abstraction.Events;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Views;
using BitCrafts.Application.Avalonia.Controls.Loading;
using BitCrafts.Infrastructure.Abstraction.Data;
using BitCrafts.Infrastructure.Abstraction.Events;
using BitCrafts.Infrastructure.Data;

namespace BitCrafts.Application.Avalonia.Controls.Views;

public abstract class BaseView : UserControl, IView
{
    private bool _isDisposed;
    private IEventAggregator _eventAggregator;
    private IModel _model;
    private bool _isBusy;
    public bool IsBusy => _isBusy;
    public string Title { get; set; }
    protected IEventAggregator EventAggregator => _eventAggregator;

    public IDataValidator DataValidator { get; } = new DataValidator();

    public void SetModel(IModel model)
    {
        _model = model;
        DisplayModel();
    }

    protected abstract void DisplayModel();

    public IModel GetModel()
    {
        return _model;
    }

    public abstract void UpdateModel();

    public virtual void SetBusy(bool busy, string message = "")
    {
        _isBusy = busy;
        if (LoadingIndicator != null)
        {
            LoadingIndicator.SetLoading(_isBusy, message);
        }
    }

    public bool ValidateModel(out List<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();

        var isValid = DataValidator.TryValidate(_model, false, out validationResults);
        if (!isValid)
        {
            EventAggregator.Publish(ViewEvents.Base.ErrorUpdateModelEventName, validationResults);
        }

        return isValid;
    }


    protected BaseView()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        Title = "UnTitled Control";
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        EventAggregator?.Publish(ViewEvents.Base.DisappearedEventName);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        EventAggregator?.Publish(ViewEvents.Base.AppearedEventName);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Loaded -= OnLoaded;
            Unloaded -= OnUnloaded;
        }
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;
        Dispose(true);
        GC.SuppressFinalize(this);
        _isDisposed = true;
    }

    public void SetEventAggregator(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }

    protected abstract LoadingControl LoadingIndicator { get; }
}