using System.Collections.Generic;

namespace Events
{
    public record struct CameraSequenceEvent(CameraSequence CameraSequence) : IEvent;
    public record struct CameraSequencesEvent(List<CameraSequence> CameraSequences) : IEvent;
}