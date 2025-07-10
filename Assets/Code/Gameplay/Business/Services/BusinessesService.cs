using System;
using System.Collections.Generic;
using Code.Gameplay.LevelUp.Components;
using Leopotam.EcsLite;

namespace Code.Gameplay.Business.Services
{
    public class BusinessesService : IBusinessesService
    {
        private readonly EcsWorld _world;
        
        public Dictionary<int, BusinessModel> CurrentBusinesses { get; private set; }

        public event Action<BusinessModel> OnBusinessAdded;
        
        public BusinessesService(EcsWorld world)
        {
            _world = world;
            CurrentBusinesses = new Dictionary<int, BusinessModel>();
        }

        public void AddBusinessModel(BusinessModel businessModel)
        {
            if (CurrentBusinesses.TryAdd(businessModel.Id, businessModel))
            {
                OnBusinessAdded?.Invoke(businessModel);
            }
        }

        public void CreateLevelUpRequest(int businessId)
        {
            int entity = _world.NewEntity();
            ref var levelUpRequest = ref _world.GetPool<LevelUpRequest>().Add(entity);
            levelUpRequest.BusinessId = businessId;
        }
    }
}