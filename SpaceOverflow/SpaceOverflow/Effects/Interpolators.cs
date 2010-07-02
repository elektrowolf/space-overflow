﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceOverflow.Effects
{
    public class Interpolators
    {
        public static Interpolator Linear {
            get {
                return new Interpolator((origFrom, origTo, origProgress) =>
                    Interpolators.InterpolateNumeric(origFrom, origTo, origProgress, 
                        new NumericInterpolator((from, to, progress) => from + (to - from) * progress))
                );
            }
        }

        public static Interpolator QuadraticIn {
            get {
                return new Interpolator((origFrom, origTo, origProgress) =>
                    Interpolators.InterpolateNumeric(origFrom, origTo, origProgress, 
                        new NumericInterpolator((from, to, progress) => from + (to - from) * progress * progress))
                );
            }
        }

        public static Interpolator QuadraticOut {
            get {
                return new Interpolator((origFrom, origTo, origProgress) =>
                    Interpolators.InterpolateNumeric(origFrom, origTo, origProgress,
                        new NumericInterpolator((from, to, progress) => from - (to - from) * (progress - 2) * progress))
                );
            }
        }

        public static Interpolator CubicIn {
            get {
                return new Interpolator((origFrom, origTo, origProgress) =>
                    Interpolators.InterpolateNumeric(origFrom, origTo, origProgress,
                        new NumericInterpolator((from, to, progress) => from + (to - from) * progress * progress * progress))
                );
            }
        }

        public static Interpolator CubicOut {
            get {
                return new Interpolator((origFrom, origTo, origProgress) =>
                    Interpolators.InterpolateNumeric(origFrom, origTo, origProgress,
                        new NumericInterpolator((from, to, progress) => from + (to - from) * ((progress - 1) * (progress - 1) * (progress - 1) + 1)))
                );
            }
        }

        protected delegate double NumericInterpolator(double from, double to, float progress);

        protected static object InterpolateNumeric(object fromObj, object toObj, float progress, NumericInterpolator interpolator) {
            if (fromObj.GetType() != toObj.GetType())
                throw new ArgumentException("From and to parameters must be the same type!");

            if (fromObj is Vector3) {
                var from = (Vector3)fromObj;
                var to = (Vector3)toObj;

                return new Vector3(
                    (float)interpolator(from.X, to.X, progress),
                    (float)interpolator(from.Y, to.Y, progress),
                    (float)interpolator(from.Z, to.Z, progress));
            }
            else if (fromObj is double || fromObj is float || fromObj is int) {
                var from = Convert.ToDouble(fromObj);
                var to = Convert.ToDouble(toObj);
                var result = interpolator(from, to, progress);

                if (fromObj is float) return (float)result;
                else if (fromObj is int) return (int)result;
                else return result;
            }
            else
                throw new NotSupportedException("Only int, float, double and Vector3 are supported.");
        }
    }
}
