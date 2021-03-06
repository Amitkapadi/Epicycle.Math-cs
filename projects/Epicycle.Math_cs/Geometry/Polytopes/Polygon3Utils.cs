﻿// [[[[INFO>
// Copyright 2015 Epicycle (http://epicycle.org, https://github.com/open-epicycle)
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// For more information check https://github.com/open-epicycle/Epicycle.Math-cs
// ]]]]

using Epicycle.Commons;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes
{

    public static class Polygon3Utils
    {
        public static Plane Plane(this IPolygon3 @this)
        {
            return @this.CoordinateSystem.ApplyToPlaneXY();
        }

        public static Vector3 Normal(this IPolygon3 @this)
        {
            return @this.CoordinateSystem.Rotation.ColZ();
        }

        public static Vector3 PlaneToSpace(this IPolygon3 @this, Vector2 point)
        {
            return @this.CoordinateSystem.Apply((Vector3)point);
        }

        public static IEnumerable<Vector3> Vertices(this IPolygon3 @this)
        {
            return @this.InPlane.Vertices().Select(v => @this.PlaneToSpace(v));
        }

        public static IEnumerable<IClosedPolyline3> Contours(this IPolygon3 @this)
        {
            return @this.InPlane.Contours.Select(c => (IClosedPolyline3)new ClosedPolyline3
                (c.Vertices.Select(v => @this.PlaneToSpace(v))));
        }

        public static double Area(this IPolygon3 @this)
        {
            return @this.InPlane.Area();
        }

        public static double Distance2(Vector3 point, IPolygon3 polygon)
        {
            var localPoint = polygon.CoordinateSystem.ApplyInv(point);

            return polygon.InPlane.Distance2(localPoint.XY) + localPoint.Z * localPoint.Z;
        }

        public static double DistanceTo(this Vector3 @this, IPolygon3 polygon)
        {
            return BasicMath.Sqrt(Distance2(@this, polygon));
        }

        public static bool IsOnPositiveSide(this IPolygon3 @this, Plane plane)
        {
            return @this.Vertices().All(v => v.IsOnPositiveSide(plane));
        }

        public static bool IsOnNegativeSide(this IPolygon3 @this, Plane plane)
        {
            return @this.Vertices().All(v => !v.IsOnPositiveSide(plane));
        }

        // intersection of polygon with half space lying on negative side of line: this is the natural convention since we use external normals
        public static IPolygon3 CropBy(this IPolygon3 @this, Plane plane)
        {
            var allPositive = true;
            var allNegative = true;

            foreach (var vertex in @this.InPlane.BoundingBox().Vertices)
            {
                if (@this.PlaneToSpace(vertex).IsOnPositiveSide(plane))
                {
                    allNegative = false;
                }
                else
                {
                    allPositive = false;
                }
            }

            if (allPositive)
            {
                return new Polygon3(@this.CoordinateSystem, Polygon.Empty);                
            }
            else if (allNegative)
            {
                return @this;
            }
            else
            {
                var croppingLine = @this.CoordinateSystem.ApplyInv(plane).IntersectWithPlaneXY();
                return new Polygon3(@this.CoordinateSystem, @this.InPlane.CropBy(croppingLine));
            }            
        }

        public static IPolygon3 CreateTriangle(Vector3 a, Vector3 b, Vector3 c)
        {
            var deltaB = b - a;
            var deltaC = c - a;

            var rotation = Rotation3Utils.GramSchmidt(deltaB, deltaC);

            var bNorm = Rotation3Utils.InvProductX(rotation, deltaB);
            var localC = Rotation3Utils.InvProductXY(rotation, deltaC);

            var polygon = new Polygon(new List<Vector2> 
            {
                Vector2.Zero,
                new Vector2(bNorm, 0),
                localC
            });

            return new Polygon3(new RotoTranslation3(rotation, a), polygon);
        }
    }
}
