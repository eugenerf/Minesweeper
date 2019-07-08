using System;
using System.Windows.Forms;
using System.Drawing;

namespace MineSweeper
{
    /// <summary>
    /// Cell of the Mine Field (represented by Label)
    /// </summary>
    class MineFieldLabel : Label, ICloneable
    {
        #region Fields
        /// <summary>
        /// Mouse button went down event handler for current cell
        /// </summary>
        MouseEventHandler MouseDowned;
        /// <summary>
        /// Mouse button went up event handler for current cell
        /// </summary>
        MouseEventHandler MouseUpped;
        /// <summary>
        /// DoubleClick event handler for current cell
        /// </summary>
        EventHandler DoubleClicked;
        /// <summary>
        /// Mine field column number of the current cell
        /// </summary>
        public int Column;
        /// <summary>
        /// Mine field row number of the current cell
        /// </summary>
        public int Row;
        #endregion

        #region Methods
        /// <summary>
        /// Minefield Cell Label ctor 
        /// (use only to create the first prototype, 
        /// and then use GetNew method to get new cells)
        /// </summary>
        /// <param name="size">Size</param>
        /// <param name="imageList">Image list</param>
        /// <param name="backColor">Background color</param>
        /// <param name="mouseDown">Mouse Down event handler</param>
        /// <param name="mouseUp">Mouse Up event handler</param>
        /// <param name="doubleClick">DoubleClick event handler</param>
        public MineFieldLabel(Size size,
                                ImageList imageList,
                                Color backColor,
                                MouseEventHandler mouseDown,
                                MouseEventHandler mouseUp,
                                EventHandler doubleClick)
            : base()                                            //construct cell using Label ctor
        {
            //set cell properties
            Size = size;                                        //size
            ImageList = imageList;                              //image list
            BackColor = backColor;                              //back color
            MouseDown += mouseDown;                             //add mouse down event handler to the cell
            MouseUp += mouseUp;                                 //add mouse up event handler to the cell
            DoubleClick += doubleClick;                         //add double click event handler to the cell
            ImageAlign = ContentAlignment.MiddleCenter;         //image align: middle-center
            TextAlign = ContentAlignment.MiddleCenter;          //text align: middle-center
            ImageIndex = -1;                                    //imageindex: -1 (no image shown)
            BorderStyle = BorderStyle.FixedSingle;              //border: fixed single
            Font = new Font("Candara", 14, FontStyle.Bold);     //text font
            Text = "";                                          //text: empty
            Column = -1;                                        //mine field column not set
            Row = -1;                                           //mine field row not set
            //save event handlers for further use
            MouseDowned = mouseDown;                            //mouse down event handler
            MouseUpped = mouseUp;                               //mouse up event handler
            DoubleClicked = doubleClick;                        //double click event handler
        }   //END (ctor)

        /// <summary>
        /// Changes some fields of the MineField Cell Label
        /// </summary>
        /// <param name="backColor">New Back Color</param>
        /// <param name="imageIndex">New Image index</param>
        /// <param name="text">New label text</param>
        public void ChangeLabel(Color backColor, int imageIndex, string text)
        {
            BackColor = backColor;      //set back color
            ImageIndex = imageIndex;    //set image index
            Text = text;                //set text
        }   //END (ChangeLabel)

        /// <summary>
        /// Get the new Mine Field Cell Label for the specified mine field location
        /// </summary>
        /// <param name="location">Location of the new Cell</param>
        /// <returns>Instance of the new Cell Label</returns>
        public MineFieldLabel GetNew(int cellSize, int column, int row)
        {
            MineFieldLabel newLabel = (MineFieldLabel)Clone();          //clone new cell label from prototype
            newLabel.Location = 
                new Point(cellSize * column + 2, cellSize * row + 8);   //set label location on the form
            newLabel.Column = column;                                   //set mine field column number
            newLabel.Row = row;                                         //set mine field row number
            return newLabel;                                            //return instance of the new cell label
        }   //END (GetNew)

        /// <summary>
        /// Get a clone of the current MineField Cell Label
        /// (to get the newly cloned Cell Label for specified location use GetNew method)
        /// </summary>
        /// <returns>Instance of the cloned Cell Label</returns>
        public object Clone()
        {
            return new MineFieldLabel(Size,
                                    ImageList,
                                    BackColor,
                                    MouseDowned,
                                    MouseUpped,
                                    DoubleClicked); //just return newly constructed identical cell label
        }   //END (Clone)
        #endregion
    }   //ENDCLASS (MineFieldLabel)
}   //ENDNAMESPACE (MineSweeper)
