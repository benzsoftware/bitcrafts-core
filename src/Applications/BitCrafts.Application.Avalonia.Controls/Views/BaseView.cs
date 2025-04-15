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
    protected IModel Model { get; private set; }
    private bool _isBusy;
    public bool IsBusy => _isBusy;

    public string Title { get; set; }

    protected IEventAggregator EventAggregator => _eventAggregator;

    public IDataValidator DataValidator { get; } = new DataValidator();


    public void SetModel(IModel model)
    {
        Model = model;
    }

    public (bool isValid, IModel model, IReadOnlyList<ValidationResult> validationResults) GetModel()
    {
        List<ValidationResult> validationResults;
        bool isValid = ValidateModel(out validationResults);
        if (!isValid)
        {
            EventAggregator.Publish(ViewEvents.Base.ErrorUpdateModelEventName, validationResults);
        }

        SetModel(UpdateModelFromInputsCore());

        return (isValid, Model, validationResults);
    }

    protected virtual IModel UpdateModelFromInputsCore()
    {
        return Model as IModel;
    }

    public void Clear()
    {
        ClearCore();
    }

    protected virtual void ClearCore()
    {
    }

    public virtual void SetVisible(bool visible)
    {
        IsVisible = true;
    }

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

        var isValid = DataValidator.TryValidate(Model, false, out validationResults);

        return isValid;
    }


    protected BaseView()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        Title = "UnTitled Control";
    }

    protected virtual void OnAppeared()
    {
    }

    protected virtual void OnDisappeared()
    {
    }


    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        EventAggregator?.Publish(ViewEvents.Base.DisappearedEventName);
        OnDisappeared();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        EventAggregator?.Publish(ViewEvents.Base.AppearedEventName);
        OnAppeared();
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