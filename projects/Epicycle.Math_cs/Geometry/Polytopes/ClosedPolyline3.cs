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

using Epicycle.Commons.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes
{
    // immutable
    public sealed class ClosedPolyline3 : IClosedPolyline3
    {
        public ClosedPolyline3(IEnumerable<Vector3> vertices)
        {
            _vertices = vertices.ToReadOnlyList();
        }

        public ClosedPolyline3(IPolysurfaceFace face)
        {
            _vertices = face.Edges.Select(e => e.Source.Point).ToReadOnlyList();
        }

        public static implicit operator ClosedPolyline3(List<Vector3> vertices)
        {
            return new ClosedPolyline3(vertices);
        }

        public IReadOnlyList<Vector3> Vertices
        {
            get { return _vertices; }
        }

        private readonly IReadOnlyList<Vector3> _vertices;
    }
}
