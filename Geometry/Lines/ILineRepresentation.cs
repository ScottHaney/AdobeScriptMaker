using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Lines
{
    public interface ILineRepresentation
    {
        ILineIntersectionResult GetIntersectionWith(ILineRepresentation otherLine);
        bool IsInRange(PointD targetPoint, PointD bound1, PointD bound2);
    }

    public interface ICanonicalLineFormCapable
    {
        CanonicalLineForm GetCanonicalLineForm();
    }

    public interface ILineIntersectionResult
    {
        bool HasSinglePointIntersection { get; }
        bool HasNoPointsInCommonWith { get; }
        bool IsTheSameLine { get; }
        PointD GetStart();
        PointD GetEnd();
    }

    public class SinglePointLineIntersectionResult : ILineIntersectionResult
    {
        private readonly PointD _intersectionPoint;

        public bool HasSinglePointIntersection => true;

        public bool HasNoPointsInCommonWith => false;

        public bool IsTheSameLine => false;

        public SinglePointLineIntersectionResult(PointD intersectionPoint)
        {
            _intersectionPoint = intersectionPoint;
        }

        public PointD GetStart()
            => _intersectionPoint;

        public PointD GetEnd()
            => _intersectionPoint;
    }

    public class NoLineIntersectionResult : ILineIntersectionResult
    {
        public bool HasSinglePointIntersection => false;
        public bool HasNoPointsInCommonWith => true;
        public bool IsTheSameLine => false;

        public PointD GetStart()
            => null;

        public PointD GetEnd()
            => null;
    }

    public class IsSameLineIntersectionResult : ILineIntersectionResult
    {
        public bool HasSinglePointIntersection => false;
        public bool HasNoPointsInCommonWith => false;
        public bool IsTheSameLine => true;

        public PointD GetStart()
            => null;

        public PointD GetEnd()
            => null;
    }
}
