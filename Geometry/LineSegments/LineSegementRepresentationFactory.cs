using Geometry.Lines;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Geometry.LineSegments
{
    public class LineSegmentRepresentationFactory : ILineSegmentRepresentationFactory
    {
        private readonly ILineRepresentationFactory _factory;

        public LineSegmentRepresentationFactory(ILineRepresentationFactory factory)
        {
            _factory = factory;
        }

        public LineSegment Create(PointD point1, PointD point2)
        {
            return new LineSegment(_factory.CreateLine(point1, point2), point1, point2);
        }

        public LineSegment Create(ILineRepresentation lineRep, PointD point1, PointD point2)
        {
            return new LineSegment(lineRep, point1, point2);
        }

        public LineSegment Create(PointF point1, PointF point2)
        {
            return Create(new PointD(point1), new PointD(point2));
        }
    }
}
