using Code.Gameplay.Business.Services;
using Code.Gameplay.BusinessUpgrades.Services;
using UnityEngine;

namespace Code.Gameplay.UI
{
    public class BusinessElementFactory : IBusinessElementFactory
    {
        private const string BusinessElementPath = "Prefabs/BusinessElement";
        
        private readonly IBusinessesService _businesses;
        private readonly IUpgradesService _upgrades;

        public BusinessElementFactory(IBusinessesService businesses, IUpgradesService upgrades)
        {
            _businesses = businesses;
            _upgrades = upgrades;
        }

        public BusinessElementController Create(BusinessModel model, RectTransform parent)
        {
            GameObject resource = Resources.Load<GameObject>(BusinessElementPath);
            GameObject obj = Object.Instantiate(resource, parent);
            BusinessElementUIView element = obj.GetComponent<BusinessElementUIView>();
            BusinessElementController controller = new (element, _businesses, model, _upgrades);
            controller.Initialize();
            element.gameObject.SetActive(true);
            return controller;
        }
    }
}