using Code.Progress.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Progress.Systems
{
    public class PeriodicallySendProgressRequestSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _timers;
        private const float SaveIntervalInSeconds = 10f;
        
        public void Init(IEcsSystems systems)
        {
            _timers = systems.GetWorld().Filter<SaveProgressTimer>().End();
            
            int timerEntity = systems.GetWorld().NewEntity();
            ref var timer = ref systems.GetWorld().GetPool<SaveProgressTimer>().Add(timerEntity);
            timer.DurationInSeconds = SaveIntervalInSeconds;
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int timer in _timers)
            {
                ref var timerComponent = ref systems.GetWorld().GetPool<SaveProgressTimer>().Get(timer);
                timerComponent.Progress += 1 / timerComponent.DurationInSeconds * Time.deltaTime;

                if (timerComponent.Progress >= 1f)
                {
                    timerComponent.Progress = 0f;
                    systems.GetWorld().GetPool<SaveProgressRequest>().Add(timer);
                }
            }
        }
    }
}