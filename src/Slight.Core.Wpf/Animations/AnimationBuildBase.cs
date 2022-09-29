
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Slight.Core
{
    public abstract class AnimationBuildBase<TOwner, TAnimation>
           where TOwner : AnimationBuildBase<TOwner, TAnimation>, new()
        where TAnimation : Timeline, new()
    {
        public static TOwner Share => new();


        protected internal TAnimation target;
        public AnimationBuildBase()
        {
            target = new TAnimation();
        }

        public TOwner AutoReverse(bool AutoReverse)
        {
            target.AutoReverse = AutoReverse;
            return (TOwner)this;
        }
        public TOwner SpeedRatio(double SpeedRatio)
        {
            target.SpeedRatio = SpeedRatio;
            return (TOwner)this;
        }
        public TOwner RepeatBehavior(RepeatBehavior RepeatBehavior)
        {
            target.RepeatBehavior = RepeatBehavior;
            return (TOwner)this;
        }
        public TOwner Name(string Name)
        {
            target.Name = Name;
            return (TOwner)this;
        }
        public TOwner FillBehavior(FillBehavior FillBehavior)
        {
            target.FillBehavior = FillBehavior;
            return (TOwner)this;
        }
        public TOwner Duration(Duration Duration)
        {
            target.Duration = Duration;
            return (TOwner)this;
        }
        public TOwner DecelerationRatio(double DecelerationRatio)
        {
            target.DecelerationRatio = DecelerationRatio;
            return (TOwner)this;
        }
        public TOwner BeginTime(int milliseconds)
        {
            target.BeginTime = TimeSpan.FromMilliseconds(milliseconds);
            return (TOwner)this;
        }
        public TOwner AccelerationRatio(double AccelerationRatio)
        {
            target.AccelerationRatio = AccelerationRatio;
            return (TOwner)this;
        }


        public TOwner SetTarget(DependencyObject dependencyObject)
        {
            if (dependencyObject is null)
            {
                throw new ArgumentNullException(nameof(dependencyObject));
            }

            Storyboard.SetTarget(target, dependencyObject);
            return (TOwner)this;
        }

        public TOwner SetTargetProperty(PropertyPath propertyPath)
        {
            if (propertyPath is null)
            {
                throw new ArgumentNullException(nameof(propertyPath));
            }
            Storyboard.SetTargetProperty(target, propertyPath);
            return (TOwner)this;
        }

        public TOwner SetTargetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            Storyboard.SetTargetName(target, name);
            return (TOwner)this;
        }

        public TOwner SetOwner(Storyboard owner)
        {
            if (!owner.Children.Contains(target))
            {
                owner.Children.Add(target);
            }
            return (TOwner)this;
        }


        public TAnimation Build()
        {
            return target;
        }
    }
}
