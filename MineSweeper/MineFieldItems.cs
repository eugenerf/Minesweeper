using System;
using System.Windows.Forms;
using System.Drawing;

namespace MineSweeper
{
    /// <summary>
    /// Factory used to operate Buttons of the Mine Field
    /// </summary>
    class MineFieldButton : Button, ICloneable
    {
        MouseEventHandler MouseDowned;
        MouseEventHandler MouseUpped;

        public MineFieldButton(AnchorStyles anchor,
                                bool autoSize,
                                bool enabled,
                                Size size,
                                ImageList imageList,
                                Color backColor,
                                int imageIndex,
                                bool visible,
                                bool tabStop,
                                FlatStyle flatStyle,
                                MouseEventHandler mouseDown,
                                MouseEventHandler mouseUp)
            : base()
        {
            Anchor = anchor;
            AutoSize = autoSize;
            Enabled = enabled;
            Size = size;
            ImageList = imageList;
            BackColor = backColor;
            ImageIndex = imageIndex;
            Visible = visible;
            TabStop = tabStop;
            FlatStyle = flatStyle;
            MouseDown += mouseDown;
            MouseUp += mouseUp;

            MouseDowned = mouseDown;
            MouseUpped = mouseUp;
        }

        /// <summary>
        /// Changes some fields of the MineField button
        /// </summary>
        /// <param name="backColor">New Back Color</param>
        /// <param name="imageIndex">New Image index</param>
        /// <param name="visible">New Visibility state</param>
        public void ChangeButton(Color backColor, int imageIndex, bool visible, bool enabled)
        {
            BackColor = backColor;
            ImageIndex = imageIndex;
            Visible = visible;
            Enabled = enabled;
        }

        /// <summary>
        /// Gets the new button of the Mine field for the specified location
        /// </summary>
        /// <param name="location">Location of the new button</param>
        /// <returns>Instance of the new button</returns>
        public MineFieldButton GetNew(Point location)
        {
            MineFieldButton newButton = (MineFieldButton)Clone();
            newButton.Location = location;
            return newButton;
        }

        /// <summary>
        /// Gets a clone of the current MineField button
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {

            return new MineFieldButton(Anchor, 
                                        AutoSize, 
                                        Enabled, 
                                        Size, 
                                        ImageList, 
                                        BackColor, 
                                        ImageIndex, 
                                        Visible, 
                                        TabStop, 
                                        FlatStyle, 
                                        MouseDowned, 
                                        MouseUpped);
        }
    }

    /// <summary>
    /// Factory used to operate Labels of the Mine Field
    /// </summary>
    class MineFieldLabel : Label, ICloneable
    {
        MouseEventHandler MouseDowned;
        MouseEventHandler MouseUpped;
        EventHandler DoubleClicked;

        public MineFieldLabel(AnchorStyles anchor,
                                bool autoSize,
                                bool enabled,
                                Size size,
                                ImageList imageList,
                                Color backColor,
                                int imageIndex,
                                bool visible,
                                BorderStyle borderStyle,
                                Font font,
                                ContentAlignment imageAlign,
                                ContentAlignment textAlign,
                                string text,
                                MouseEventHandler mouseDown,
                                MouseEventHandler mouseUp,
                                EventHandler doubleClick)
            : base()
        {
            Anchor = anchor;
            AutoSize = autoSize;
            Enabled = enabled;
            Size = size;
            ImageList = imageList;
            BackColor = backColor;
            ImageIndex = imageIndex;
            Visible = visible;
            BorderStyle = borderStyle;
            Font = font;
            ImageAlign = imageAlign;
            TextAlign = textAlign;
            Text = text;
            MouseDown += mouseDown;
            MouseUp += mouseUp;
            DoubleClick += doubleClick;

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
        /// <param name="visible">New Visibility state</param>
        public void ChangeLabel(Color backColor, int imageIndex, string text, bool visible)
        {
            BackColor = backColor;
            ImageIndex = imageIndex;
            Text = text;
            Visible = visible;
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

            return new MineFieldLabel(Anchor,
                                        AutoSize,
                                        Enabled,
                                        Size,
                                        ImageList,
                                        BackColor,
                                        ImageIndex,
                                        Visible,
                                        BorderStyle,
                                        Font,
                                        ImageAlign,
                                        TextAlign,
                                        Text,
                                        MouseDowned,
                                        MouseUpped,
                                        DoubleClicked);
        }
    }
}
