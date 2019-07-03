using System;
using System.Windows.Forms;
using System.Drawing;

namespace MineSweeper
{
    /// <summary>
    /// Factory used to operate Labels of the Mine Field
    /// </summary>
    class MineFieldLabel : Label, ICloneable
    {
        MouseEventHandler MouseDowned;
        MouseEventHandler MouseUpped;
        EventHandler DoubleClicked;

        public MineFieldLabel(Size size,
                                ImageList imageList,
                                Color backColor,
                                MouseEventHandler mouseDown,
                                MouseEventHandler mouseUp,
                                EventHandler doubleClick)
            : base()
        {
            Size = size;
            ImageList = imageList;
            BackColor = backColor;
            MouseDown += mouseDown;
            MouseUp += mouseUp;
            DoubleClick += doubleClick;

            ImageAlign = ContentAlignment.MiddleCenter;
            TextAlign = ContentAlignment.MiddleCenter;
            ImageIndex = -1;
            BorderStyle = BorderStyle.FixedSingle;
            Font = new Font("Candara", 14, FontStyle.Bold);
            Text = "";

            MouseDowned = mouseDown;
            MouseUpped = mouseUp;
            DoubleClicked = doubleClick;
        }

        /// <summary>
        /// Changes some fields of the MineField label
        /// </summary>
        /// <param name="backColor">New Back Color</param>
        /// <param name="imageIndex">New Image index</param>
        /// <param name="text">New label text</param>
        public void ChangeLabel(Color backColor, int imageIndex, string text)
        {
            BackColor = backColor;
            ImageIndex = imageIndex;
            Text = text;
        }

        /// <summary>
        /// Gets the new label of the Mine field for the specified location
        /// </summary>
        /// <param name="location">Location of the new label</param>
        /// <returns>Instance of the new button</returns>
        public MineFieldLabel GetNew(Point location)
        {
            MineFieldLabel newLabel = (MineFieldLabel)Clone();
            newLabel.Location = location;
            return newLabel;
        }

        /// <summary>
        /// Gets a clone of the current MineField label
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {

            return new MineFieldLabel(Size,
                                        ImageList,
                                        BackColor,
                                        MouseDowned,
                                        MouseUpped,
                                        DoubleClicked);
        }
    }
}
