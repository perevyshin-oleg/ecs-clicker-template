using Code.Gameplay.Business.Services;
using UnityEngine;

namespace Code.Gameplay.UI
{
    public interface IBusinessElementFactory
    {
        BusinessElementController Create(BusinessModel model, RectTransform parent);
    }
}