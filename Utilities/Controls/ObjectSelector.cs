using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utilities.Controls
{
    public class ObjectSelector : ListView
    {
        List<Drawable3D> objects = new List<Drawable3D>();
        public OpenGLControl open_gl_control;

        public ObjectSelector()
        {
            InitializeComponent();
        }

        public void AddObject(Drawable3D Object)
        {
            objects.Add(Object);
            Items.Add(Object.ToString());
        }

        public void AddGroup(string name)
        {
            Groups.Add(name, name);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ObjectSelector
            // 
            this.CheckBoxes = true;
            this.HoverSelection = true;
            this.View = System.Windows.Forms.View.List;
            this.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.ObjectSelector_ItemChecked);
            this.SelectedIndexChanged += new System.EventHandler(this.ObjectSelector_SelectedIndexChanged);
            this.ResumeLayout(false);

        }

        private void ObjectSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateObjects();
        }

        private void updateObjects()
        {
            if (open_gl_control != null)
            {
                open_gl_control.objects.Clear();
                foreach (int item in SelectedIndices)
                {
                    open_gl_control.objects.Add(objects[item]);
                }
                foreach (int item in this.CheckedIndices)
                {
                    open_gl_control.objects.Add(objects[item]);
                }
                open_gl_control.Invalidate();
            }
        }

        private void ObjectSelector_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            updateObjects();

        }

    }
}
