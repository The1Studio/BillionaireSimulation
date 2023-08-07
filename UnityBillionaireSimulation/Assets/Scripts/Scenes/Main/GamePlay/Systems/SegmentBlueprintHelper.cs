namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems
{
    using System;
    using TheOneStudio.HyperCasual.Blueprints;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines;

    public class SegmentBlueprintHelper
    {
        private readonly SegmentMiniGameBlueprint      segmentMiniGameBlueprint;
        private readonly SegmentPickActionBlueprint    segmentPickActionBlueprint;
        private readonly SegmentDelayIntervalBlueprint segmentDelayIntervalBlueprint;

        public SegmentBlueprintHelper(SegmentMiniGameBlueprint segmentMiniGameBlueprint,
            SegmentPickActionBlueprint segmentPickActionBlueprint, SegmentDelayIntervalBlueprint segmentDelayIntervalBlueprint)
        {
            this.segmentMiniGameBlueprint      = segmentMiniGameBlueprint;
            this.segmentPickActionBlueprint    = segmentPickActionBlueprint;
            this.segmentDelayIntervalBlueprint = segmentDelayIntervalBlueprint;
        }

        public GameStateType GetGameStateTypeBySegmentId(string nextSegmentId)
        {
            var result = nextSegmentId switch
            {
                _ when this.segmentMiniGameBlueprint.ContainsKey(nextSegmentId)      => this.segmentMiniGameBlueprint[nextSegmentId].GameStateType,
                _ when this.segmentPickActionBlueprint.ContainsKey(nextSegmentId)    => this.segmentPickActionBlueprint[nextSegmentId].GameStateType,
                _ when this.segmentDelayIntervalBlueprint.ContainsKey(nextSegmentId) => this.segmentDelayIntervalBlueprint[nextSegmentId].GameStateType,
                _                                                                    => throw new ArgumentOutOfRangeException(nameof(nextSegmentId), nextSegmentId, null)
            };
            return result;
        }
    }
}