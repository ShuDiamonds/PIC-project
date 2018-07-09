using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
    public partial class MoveSizeForm : Form
    {
        public MoveSizeForm()
        {
            InitializeComponent();
            moveX = 0;
            moveY = 0;
        }

        private void IDOK_Click(object sender, EventArgs e)
        {
            duplicate = checkBoxDuplicate.Checked;
            if (CheckInputValue()) {
                DialogResult = DialogResult.OK;
            }
        }

        private void MoveSizeForm_Load(object sender, EventArgs e)
        {
            double min;
            double max;
            min = MIN_MOVE / 10000.0F;
            max = MAX_MOVE / 10000.0F;
            colorTextBack = textBoxX.BackColor;

            labelSizeRange.Text = String.Format("({0:##0.0###} - {1:##0.0###}mm)", min, max);

            textBoxX.Text =  String.Format("{0:##0.0###}", moveX / 10000.0);
            textBoxY.Text =  String.Format("{0:##0.0###}", moveY / 10000.0);
            checkBoxDuplicate.Checked = false;
        }

        public const int MIN_MOVE = -2000000;
        public const int MAX_MOVE = 2000000;
        private Color colorTextBack;

        
        public int MoveX
        {
            get { return moveX; }
            set { 
                if (value < MIN_MOVE) {
                    moveX = MIN_MOVE;
                } else if (value > MAX_MOVE) {
                    moveX = MAX_MOVE;
                } else {
                    moveX = value;
                }
            }
        }

        
        public int MoveY
        {
            get { return moveY; }
            set
            {
                if (value < MIN_MOVE) {
                    moveY = MIN_MOVE;
                } else if (value > MAX_MOVE) {
                    moveY = MAX_MOVE;
                } else {
                    moveY = value;
                }
            }
        }

        private bool CheckInputValue()
        {
            double val;
            int nVal;
            bool err = false;

            val = LengthString.ToDouble(textBoxX.Text, -1.0);
            nVal = (int)(val * 10000);
            MoveX = nVal;
            if (MoveX != nVal) {
                textBoxX.BackColor = MbeColors.ColorInputErr;
                err = true;
            }

            val = LengthString.ToDouble(textBoxY.Text, -1.0);
            nVal = (int)(val * 10000);
            MoveY = nVal;
            if (MoveY != nVal) {
                textBoxY.BackColor = MbeColors.ColorInputErr;
                err = true;
            }
            return !err;
        }


        private int moveX;
        private int moveY;
        private bool duplicate;

        public bool Duplicate
        {
            get { return duplicate; }
            //set { duplicate = value; }
        }


    }
}
