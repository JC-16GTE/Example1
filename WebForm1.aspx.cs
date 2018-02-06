using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MegaChallengeCasino
{
    /*
     * Fruit Machine Simulation
     * 
     * 1 Cherry   - bet * 2
     * 2 Cherries - bet * 3
     * 3 Cherries - bet * 4
     * 3 Sevens   - bet * 100 (Jackpot)
     * 1 or more BARS.  No win.  
     * 
     * Indexes for the respective images in string array "images"
     * Cherry - 5
     * Seven  - 8
     * Bar    - 2
     * 
     *  Algorithm (Taking 1,2 and 3 as the panels where the Cherry appears).
     * 
     *  1 Cherry   2 Cherries  3 Cherries
     *  
     *  1 !2!3       12 !3       1 2 3
     *  2 !1!3       13 !2
     *  3 !1!2       23 !1
     *
    */
 
    public partial class WebForm1 : System.Web.UI.Page
    {
        Random randomImage = new Random();
        string[] images = new string[] { "Strawberry", "Bar", "Lemon", "Bell", "Clover", "Cherry", "Diamond", "Orange", "Seven", "HorseShoe", "Plum", "Watermelon" };
        int Image1Index, Image2Index, Image3Index;

        private bool ValidBet() // Validate the data entered for the bet : Must be double
        {
            double bet = 0.0;
            if (!Double.TryParse(betTextBox.Text, out bet))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void SetImages()
        {
            Image1Index = randomImage.Next(11);
            panel1Image.ImageUrl = "/Images/" + images[Image1Index] + ".png";
            Image2Index = randomImage.Next(11);
            panel2Image.ImageUrl = "/Images/" + images[Image2Index] + ".png";
            Image3Index = randomImage.Next(11);
            panel3Image.ImageUrl = "/Images/" + images[Image3Index] + ".png";
        }

    private bool OneCherry()
        {
            if (
                (Image1Index == 5 && !(Image2Index == 5 && Image3Index == 5)) ||
                (Image2Index == 5 && !(Image1Index == 5 && Image3Index == 5)) ||
                (Image3Index == 5 && !(Image1Index == 5 && Image2Index == 5))
               )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool TwoCherries()
        {
            if (
                (Image1Index == 5 && Image2Index == 5) && !(Image3Index == 5) ||
                (Image1Index == 5 && Image3Index == 5) && !(Image2Index == 5) ||
                (Image2Index == 5 && Image3Index == 5) && !(Image1Index == 5)
               )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ThreeCherries()
        {
            if (Image1Index == 5 && Image2Index == 5 && Image3Index == 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ThreeSevens()
        {
            if (Image1Index == 8 && Image2Index == 8 && Image3Index == 8)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool OneBar()
        {
            if (Image1Index == 1 || Image2Index == 1 || Image3Index == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

                
        protected void Page_Load(object sender, EventArgs e)
       {
            // Set some default images for each panel
            if (!Page.IsPostBack)
            {
                SetImages();
            }
        }


        // Main routine : Button Click

        protected void spinButton_Click(object sender, EventArgs e)
        {            
            resultLabel.Text = "";  // Clear the result output
            
            // First check the bet box is not empty and then validate the bet
            if (betTextBox.Text.Trim().Length != 0)
            {
                if (ValidBet())
                {
                    // Set the bet value
                    double betValue = double.Parse(betTextBox.Text);
                    double winValue = 0.0;
                    string result = "No win this time";

                    SetImages();

                    // Check for one cherry. Win = bet * 2
                    if (OneCherry())
                    {
                        winValue = betValue * 2.00;
                        result = string.Format("One Cherry. You win {0:C}", winValue);
                                                
                        // Finally check for one bar as this overides all else. Lose. 
                        if (OneBar())
                        {
                            winValue = 0.00;
                            result = string.Format("Sorry You've got one bar. You win {0:C}",winValue);
                        }
                    }
                    
                    // Check for two cherries. Win = bet * 3
                    if (TwoCherries())
                    {
                        winValue = betValue * 3.00;
                        result = string.Format("Two Cherries. You win {0:C}", winValue);
                        // Finally check for one bar as this overides all else. Lose. 
                        if (OneBar())
                        {
                            winValue = 0.00;
                            result = string.Format("Sorry You've got one bar. You win {0:C}", winValue);
                        }
                    }
                    
                    // Check for three cherries. Win = bet * 4
                    if (ThreeCherries())
                    {
                        winValue = betValue * 4.00;
                        result = string.Format("Three Cherries. You win {0:C}", winValue);
                        /*
                         *  Finally check for one bar as this overides all else. Lose. 
                         *  
                         */                        
                        if (OneBar())
                        {
                            winValue = 0.00;
                            result = string.Format("Sorry You've got one bar. You win {0:C}", winValue);
                        }
                    }  
                    
                    // Check for three sevens. win = bet * 100 (Jackpot)
                    if (ThreeSevens())
                    {
                        winValue = betValue * 100.00;
                        result = string.Format("Three Sevens. Congratulations - Jackpot. You've won {0:C}", winValue);
                       
                    }
                    
                    // Output the result
                    resultLabel.Text = result;
                }
            }
        }
    }
}