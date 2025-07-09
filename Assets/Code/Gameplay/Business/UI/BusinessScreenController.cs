using System;
using System.Collections.Generic;
using Code.Gameplay.Balance.Services;
using Code.Gameplay.Business.Services;

namespace Code.Gameplay.UI
{
    public class BusinessScreenController : IDisposable
    {
        private readonly BusinessScreenUIView _screenView;
        private readonly IBusinessesService _businessesService;
        private readonly IUserBalanceService _userBalanceService;
        private readonly IBusinessElementFactory _businessElementFactory;

        private List<BusinessElementController> _subControllers = new();

        public BusinessScreenController(
            BusinessScreenUIView businessScreenUIView,
            IBusinessesService businessesService, 
            IUserBalanceService userBalanceService, 
            IBusinessElementFactory businessElementFactory)
        {
            _screenView = businessScreenUIView;
            _businessesService = businessesService;
            _userBalanceService = userBalanceService;
            _businessElementFactory = businessElementFactory;
            _businessesService.OnBusinessAdded += OnBusinessAdded;
            _userBalanceService.OnBalanceChanged += OnBalanceChanged;
            SetBalance(_userBalanceService.CurrentBalance);
        }
        
        public void Dispose()
        {
            _businessesService.OnBusinessAdded -= OnBusinessAdded;

            foreach (BusinessElementController subController in _subControllers)
            {
                if (subController != null)
                {
                    subController.Dispose();
                }
            }
        }

        private void OnBalanceChanged(int newBalance) =>
            SetBalance(newBalance);

        private void OnBusinessAdded(BusinessModel businessModel)
        {
            BusinessElementController controller = 
                _businessElementFactory.Create(businessModel, _screenView.BusinessElementContainer);
            
            _subControllers.Add(controller);
        }

        private void SetBalance(int newBalance) =>
            _screenView.SetBalanceText($"Баланс: {newBalance}$");
    }
}