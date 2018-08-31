﻿// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Cognitive Services (formerly Project Oxford): https://www.microsoft.com/cognitive-services
// 
// Microsoft Cognitive Services (formerly Project Oxford) GitHub:
// https://github.com/Microsoft/Cognitive-Common-Windows
// 
// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ProjectOxford.Common.Contract
{
    public class FaceResult
    {
        /// <summary>
        /// Gets or sets the face rectangle.
        /// </summary>
        /// <value>
        /// The face rectangle.
        /// </value>
        public Rectangle FaceRectangle { get; set; }

        /// <summary>
        /// Gets or sets the emotion scores.
        /// </summary>
        /// <value>
        /// The emotion scores.
        /// </value>
        public FaceAttributes FaceAttributes { get; set; }

        #region overrides
        public override bool Equals(object o)
        {
            if (o == null) return false;

            var other = o as FaceResult;

            if (other == null) return false;

            if (this.FaceRectangle == null)
            {
                if (other.FaceRectangle != null) return false;
            }
            else
            {
                if (!this.FaceRectangle.Equals(other.FaceRectangle)) return false;
            }

            if (this.FaceAttributes == null)
            {
                return other.FaceAttributes == null;
            }
            else
            {
                return this.FaceAttributes.Equals(other.FaceAttributes);
            }
        }

        public override int GetHashCode()
        {
            int r = (FaceRectangle == null) ? 0x33333333 : FaceRectangle.GetHashCode();
            int s = (FaceAttributes == null) ? 0xccccccc : FaceAttributes.GetHashCode();
            return r ^ s;
        }
        #endregion
    }
}
