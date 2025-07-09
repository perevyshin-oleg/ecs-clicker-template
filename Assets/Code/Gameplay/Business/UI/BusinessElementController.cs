using System;
using Code.Gameplay.Business.Services;

namespace Code.Gameplay.UI
{
    public class BusinessElementController : IDisposable
    {
        private readonly BusinessElementUIView _view;
        private readonly BusinessModel _model;
        private readonly IBusinessesService _businessesService;

        public BusinessElementController(
            BusinessElementUIView view,
            IBusinessesService businessesService,
            BusinessModel businessModel)
        {
            _view = view;
            _businessesService = businessesService;
            _model = businessModel;
        }

        public void Initialize()
        {
            UpdateView();

            //_view.AddUpgradeButton();
            
            _model.Updated += UpdateView;
            _view.OnLevelUpClicked += OnLevelUpClick;
        }

        private void UpdateView()
        {
            _view.SetName(_model.Name);
            _view.SetLevel($"LVL\n{_model.Level}");
            _view.SetIncome($"Доход\n{_model.TotalIncome}");
            _view.SetLevelUpText($"LVL UP\n{_model.TotalCost}");
            _view.SetProgressValue(_model.IncomeProgress);
        }

        private void OnLevelUpClick() =>
            _businessesService.CreateLevelUpRequest(_model.Id);

        private void CreateUpgradeRequest()
        {
            //_businessesService.CreateUpgradeRequest(_);
        }

        public void Dispose()
        {
            _view.OnLevelUpClicked -= OnLevelUpClick;
        }
    }
}