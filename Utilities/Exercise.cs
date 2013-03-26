using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class Exercise
    {
        string title;
        Func<Drawable[]> function;

        public Exercise(string title, Func<Drawable[]> function)
        {
            this.title = title;
            this.function = function;
        }

        public override string ToString()
        {
            return title;
        }

        public Drawable[] run()
        {
            return function.Invoke();
        }
    }
}
