using Code.Gameplay.Business.Services;
using UnityEngine;

namespace Code.Gameplay.UI
{
    public class BusinessElementFactory : IBusinessElementFactory
    {
        private const string BusinessElementPath = "Prefabs/BusinessElement";
        
        private readonly IBusinessesService _businesses;

        public BusinessElementFactory(IBusinessesService businesses)
        {
            _businesses = businesses;
        }

        public BusinessElementController Create(BusinessModel model, RectTransform parent)
        {
            GameObject resource = Resources.Load<GameObject>(BusinessElementPath);
            GameObject obj = Object.Instantiate(resource, parent);
            BusinessElementUIView element = obj.GetComponent<BusinessElementUIView>();
            BusinessElementController controller = new (element, _businesses, model);
            controller.Initialize();
            element.gameObject.SetActive(true);
            return controller;
        }
    }
}