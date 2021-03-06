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

using Epicycle.Math.Geometry.Differential;

namespace Epicycle.Math.Geometry
{
    using System;

    public struct Rotation2 : IManifoldPoint
    {
        public Rotation2(Vector2 complexNumber)
        {
            var rotationNumber = complexNumber.Normalized;

            _cos = rotationNumber.X;
            _sin = rotationNumber.Y;
        }

        public Rotation2(double angle)
        {
            _cos = Math.Cos(angle);
            _sin = Math.Sin(angle);
        }

        private Rotation2(double cos, double sin)
        {
            _cos = cos;
            _sin = sin;
        }

        private readonly double _cos;
        private readonly double _sin;

        public static readonly Rotation2 Id = new Rotation2(cos: 1, sin: 0);

        #region properties

        public double Cos
        {
            get { return _cos; }
        }

        public double Sin
        {
            get { return _sin; }
        }

        public Vector2 ColX
        {
            get { return new Vector2(_cos, _sin); }
        }

        public Vector2 ColY
        {
            get { return new Vector2(-_sin, _cos); }
        }

        public double Angle
        {
            get { return Math.Atan2(_sin, _cos); }
        }

        public Rotation2 Inv
        {
            get { return new Rotation2(_cos, -_sin); }
        }

        #endregion

        public Vector2 Apply(Vector2 v)
        {
            return new Vector2(_cos * v.X - _sin * v.Y, _sin * v.X + _cos * v.Y);
        }

        public Vector2 ApplyInv(Vector2 v)
        {
            return new Vector2(_cos * v.X + _sin * v.Y, -_sin * v.X + _cos * v.Y);
        }

        public static Rotation2 operator *(Rotation2 r, Rotation2 q)
        {
            return new Rotation2(r.Cos * q.Cos - r.Sin * q.Sin, r.Cos * q.Sin + r.Sin * q.Cos);
        }

        public static Vector2 operator *(Rotation2 r, Vector2 v)
        {
            return r.Apply(v);
        }

        int IManifoldPoint.Dimension
        {
            get { return 1; }
        }

        public override string ToString()
        {
            return Angle.ToString();
        }
    }
}
