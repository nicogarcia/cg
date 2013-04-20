using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class Exercise
    {
        string title;
        Func<Drawable2D[]> function;

        public Exercise(string title, Func<Drawable2D[]> function)
        {
            this.title = title;
            this.function = function;
        }

        public override string ToString()
        {
            return title;
        }

        public Drawable2D[] run()
        {
            return function.Invoke();
        }
    }
}
