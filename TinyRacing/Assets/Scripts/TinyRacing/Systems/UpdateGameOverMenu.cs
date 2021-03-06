using Unity.Entities;
using Unity.Transforms;
#if UNITY_DOTSPLAYER
using Unity.Tiny.Audio;

#endif

namespace TinyRacing.Systems
{
    /// <summary>
    ///     Update the race ending UI menu
    /// </summary>
    [UpdateAfter(typeof(ResetRace))]
    [UpdateAfter(typeof(TransformSystemGroup))]
    public class UpdateGameOverMenu : SystemBase
    {
        protected override void OnUpdate()
        {
            var race = GetSingleton<Race>();
            SetMenuVisibility(race.IsRaceFinished);
        }

        private void SetMenuVisibility(bool isVisible)
        {
            if (isVisible)
            {
#if UNITY_DOTSPLAYER
                Entities.WithAll<GameOverMenuTag, AudioSource, Disabled>().ForEach((Entity entity) =>
                {
                    EntityManager.AddComponent<AudioSourceStart>(entity);
                }).WithStructuralChanges().Run();
#endif
                Entities.WithAll<GameOverMenuTag, Disabled>().ForEach((Entity entity) =>
                {
                    EntityManager.RemoveComponent<Disabled>(entity);
                }).WithStructuralChanges().Run();
            }
            else
            {
                Entities.WithAll<GameOverMenuTag>().ForEach((Entity entity) =>
                {
                    EntityManager.AddComponent<Disabled>(entity);
                }).WithStructuralChanges().Run();
            }
        }
    }
}
