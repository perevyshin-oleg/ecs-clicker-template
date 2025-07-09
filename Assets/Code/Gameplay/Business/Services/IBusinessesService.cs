using System;
using System.Collections.Generic;

namespace Code.Gameplay.Business.Services
{
    public interface IBusinessesService
    {
        Dictionary<int, BusinessModel> CurrentBusinesses { get; }
        void CreateUpgradeRequest(int businessId, int upgradeId);
        void CreateLevelUpRequest(int businessId);
        event Action<BusinessModel> OnBusinessAdded;
        void AddBusinessModel(BusinessModel businessModel);
    }
}