using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/// <summary>
/// unity slot machine 
/// Created by Ben D. 100098171
/// Edited from Tom T.'s Starting code
/// 
/// Created Dec 04, 2016
/// 
/// Description, a simple unity slot machie created by ben dunn for 
/// unity, slight edits and script procedural code. This project was
/// created as a general introduction to unity and is designed to be 
/// nothing more then the basics. 
/// 
/// </summary>
public class SlotMachine : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //start thingger 
        changeBet(10);

	}
	
	private int playerMoney = 1000;
	private int winnings = 0;
	private int jackpot = 5000;
	private float turn = 0.0f;
	private int playerBet = 0;
	private float winNumber = 0.0f;
	private float lossNumber = 0.0f;
	private string[] spinResult;
	private string fruits = "";
	private float winRatio = 0.0f;
	private float lossRatio = 0.0f;
	private int grapes = 0;
	private int bananas = 0;
	private int oranges = 0;
	private int cherries = 0;
	private int bars = 0;
	private int bells = 0;
	private int sevens = 0;
	private int blanks = 0;

    //the image and sprite holders

    private bool gameOver = true; //inverted for ease. 
    private bool disableSpinRoll = true; //inverted for ease. 
    public Image[] slots;
    public Sprite[] slotDisplay;
    public Sprite spinDisabled;
    public Sprite spinEnabled;
    public Image spinButton;

    //text information
    public Text creditsTextArea, betTextArea, winnerPaidTextAra, mainUpperTextArea;

    //standard delay for timing
    //WaitForSeconds realDelay = new WaitForSeconds(.25f);

    /* Utility function to show Player Stats */
    private void showPlayerStats()
	{
		winRatio = winNumber / turn;
		lossRatio = lossNumber / turn;
		string stats = "";
		stats += ("Jackpot: " + jackpot + "\n");
		stats += ("Player Money: " + playerMoney + "\n");
		stats += ("Turn: " + turn + "\n");
		stats += ("Wins: " + winNumber + "\n");
		stats += ("Losses: " + lossNumber + "\n");
		stats += ("Win Ratio: " + (winRatio * 100) + "%\n");
		stats += ("Loss Ratio: " + (lossRatio * 100) + "%\n");
		Debug.Log(stats);
	}

	/* Utility function to reset all fruit tallies*/
	private void resetFruitTally()
	{
		grapes = 0;
		bananas = 0;
		oranges = 0;
		cherries = 0;
		bars = 0;
		bells = 0;
		sevens = 0;
		blanks = 0;
	}

    /// <summary>
    /// Causes the display to become disabled if the game credits are
    /// 0. 
    /// </summary>
    public void checkForGameOver()
    {
        if(this.playerMoney<= 0)
        {
            this.mainUpperTextArea.text = "GAME OVER";
            this.betTextArea.text = "GAME OVER";
            this.winnerPaidTextAra.text = "GAME OVER";
            this.creditsTextArea.text = "GAME OVER";
            this.winnerPaidTextAra.text = "GAME OVER";
            this.gameOver = false; //inverted bool, ust for ease and avoiding ! operators. 
            this.spinButton.sprite = this.spinDisabled;
        }
    }

    /// <summary>
    /// General GUI and reset public method for 
    /// clearing the slot machine and getting everthing working
    /// again. 
    /// </summary>
    public void callReset()
    {
        this.resetAll();
        this.resetFruitTally();
        this.creditsTextArea.text = this.playerMoney.ToString();
        this.betTextArea.text = this.playerBet.ToString();
        this.winnerPaidTextAra.text = "";
        this.mainUpperTextArea.text = "Game Reset!";
        this.validateBet(); //incase the button calls and the user bet is still in the old state. 
        this.spinButton.sprite = this.spinEnabled;
    }


	/* Utility function to reset the player stats */
	private void resetAll()
	{
        gameOver = true; //inverted. 
        playerMoney = 1000;
		winnings = 0;
		jackpot = 5000;
		turn = 0;
		playerBet = 0;
		winNumber = 0;
		lossNumber = 0;
		winRatio = 0.0f;

    }

	/* Check to see if the player won the jackpot */
	private void checkJackPot()
	{
		/* compare two random values */
		var jackPotTry = Random.Range (1, 51);
		var jackPotWin = Random.Range (1, 51);
		if (jackPotTry == jackPotWin)
		{
            this.mainUpperTextArea.text = "JACKPOT!!!";
			Debug.Log("You Won the $" + jackpot + " Jackpot!!");
			playerMoney += jackpot;
			jackpot = 1000;
		}
	}

	/* Utility function to show a win message and increase player money */
	private void showWinMessage()
	{
        this.mainUpperTextArea.text = "WIN!"; //comes ahead of jackpot to cause it to be erased if a jackpot happens
        playerMoney += winnings;
		Debug.Log("You Won: $" + winnings);
        this.winnerPaidTextAra.text = ("You Won: $" + winnings.ToString());
        this.creditsTextArea.text = playerMoney.ToString();
        resetFruitTally();
		checkJackPot();

	}

	/* Utility function to show a loss message and reduce player money */
	private void showLossMessage()
	{
        this.mainUpperTextArea.text = "LOSE!"; //comes ahead of jackpot to cause it to be erased if a jackpot happens
        playerMoney -= playerBet;
		Debug.Log("You Lost!");
        this.winnerPaidTextAra.text = "You lost!";
        this.creditsTextArea.text = playerMoney.ToString();
        resetFruitTally();
        this.checkForGameOver();

	}


	/* Utility function to check if a value falls within a range of bounds */
	private bool checkRange(int value, int lowerBounds, int upperBounds)
	{
		return (value >= lowerBounds && value <= upperBounds) ? true : false;

	}


	/* When this function is called it determines the betLine results.
    e.g. Bar - Orange - Banana */
	private string[] Reels()
	{
		string[] betLine = { " ", " ", " " };
		int[] outCome = { 0, 0, 0 };

		for (var spin = 0; spin < 3; spin++)
		{
			outCome[spin] = Random.Range(1,65);

			if (checkRange(outCome[spin], 1, 27)) {  // 41.5% probability
				betLine[spin] = "blank";
                changeSlotImage(spin, 0);
                blanks++;
			}
			else if (checkRange(outCome[spin], 28, 37)){ // 15.4% probability
                changeSlotImage(spin, 1);
                betLine[spin] = "Grapes";
				grapes++;
			}
			else if (checkRange(outCome[spin], 38, 46)){ // 13.8% probability
                changeSlotImage(spin, 2);
                betLine[spin] = "Banana";
				bananas++;
			}
			else if (checkRange(outCome[spin], 47, 54)){ // 12.3% probability
                changeSlotImage(spin, 3);
                betLine[spin] = "Orange";
				oranges++;
			}
			else if (checkRange(outCome[spin], 55, 59)){ //  7.7% probability
                changeSlotImage(spin, 4);
                betLine[spin] = "Cherry";
				cherries++;
			}
			else if (checkRange(outCome[spin], 60, 62)){ //  4.6% probability
                changeSlotImage(spin, 5);
                betLine[spin] = "Bar";
				bars++;
			}
			else if (checkRange(outCome[spin], 63, 64)){ //  3.1% probability
                changeSlotImage(spin, 6);
                betLine[spin] = "Bell";
				bells++;
			}
			else if (checkRange(outCome[spin], 65, 65)){ //  1.5% probability
                changeSlotImage(spin, 7);
                betLine[spin] = "Seven";
				sevens++;
			}

		}
		return betLine;
	}

	/* This function calculates the player's winnings, if any */
	private void determineWinnings()
	{
		if (blanks == 0)
		{
			if (grapes == 3)
			{
				winnings = playerBet * 10;
			}
			else if (bananas == 3)
			{
				winnings = playerBet * 20;
			}
			else if (oranges == 3)
			{
				winnings = playerBet * 30;
			}
			else if (cherries == 3)
			{
				winnings = playerBet * 40;
			}
			else if (bars == 3)
			{
				winnings = playerBet * 50;
			}
			else if (bells == 3)
			{
				winnings = playerBet * 75;
			}
			else if (sevens == 3)
			{
				winnings = playerBet * 100;
			}
			else if (grapes == 2)
			{
				winnings = playerBet * 2;
			}
			else if (bananas == 2)
			{
				winnings = playerBet * 2;
			}
			else if (oranges == 2)
			{
				winnings = playerBet * 3;
			}
			else if (cherries == 2)
			{
				winnings = playerBet * 4;
			}
			else if (bars == 2)
			{
				winnings = playerBet * 5;
			}
			else if (bells == 2)
			{
				winnings = playerBet * 10;
			}
			else if (sevens == 2)
			{
				winnings = playerBet * 20;
			}
			else if (sevens == 1)
			{
				winnings = playerBet * 5;
			}
			else
			{
				winnings = playerBet * 1;
			}
			winNumber++;
			showWinMessage();
		}
		else
		{
			lossNumber++;
			showLossMessage();
		}

	}

    /// <summary>
    /// on click, performs the routines for a spin. 
    /// </summary>
	public void OnSpinButtonClick()
	{
       
        if (validateBet()&& gameOver && disableSpinRoll)
        {

            if (playerBet == 0)
            {
                this.mainUpperTextArea.text = "Please Place a bet! to play";
            }
            else if (playerMoney == 0)
            {

                this.mainUpperTextArea.text = "YOU LOSE";
            }
            else if (playerBet > playerMoney)
            {
                this.mainUpperTextArea.text = "you don't have enough creadits";

            }
            else if (playerBet < 0)
            {
                this.mainUpperTextArea.text = "HOW?!";

            }
            else if (playerBet <= playerMoney)
            {
                spinningTheSlots();

            }
            else
            {
                this.mainUpperTextArea.text = "Please enter a valid amount!";
            }
        }
	}

    /// <summary>
    /// simple method for changing the display fruits. 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    public void changeSlotImage(int index, int value)
    {
        this.slots[index].sprite = this.slotDisplay[value];
    }

    /// <summary>
    /// function, changes the bet amount on action. 
    /// </summary>
    /// <param name="newBet"></param>
    public void changeBet (int newBet)
    {
        this.betTextArea.text = newBet.ToString();
        this.playerBet = newBet;
        validateBet(); //visual update purpose. 
    }

    /// <summary>
    /// multipurpose method, checks if the bet is valid,
    /// second updates the display, is used as both a validaiton
    /// route and a visual update route. 
    /// </summary>
    /// <returns></returns>
    public bool validateBet()
    {
        if ((this.playerBet > this.playerMoney)&& gameOver)
        {
            this.spinButton.sprite = this.spinDisabled;
            return false;
        }
        else
        {
            this.spinButton.sprite = this.spinEnabled;
            return true;
        }
    }

    /// <summary>
    /// spins the slots, and delays the program (no threading) for the slot animation. 
    /// </summary>
    private void spinningTheSlots ()
    {
        Debug.Log("test");
            StartCoroutine(delay());

    }

    /// <summary>
    /// Delay method for visually rolling the slots (through randomization), does not effect the roll
    /// merely effects the visuals. Calls the update methods after the roll to continue the code
    /// (using blocking) else the slots will display the wrong values. 
    /// </summary>
    /// <returns></returns>
    public IEnumerator delay()
    {
        this.disableSpinRoll = false;
        for (int k = 0; k <= 20; k++) {
            yield return new WaitForSeconds(.1f);
            this.slots[0].sprite = this.slotDisplay[Random.Range(0, this.slotDisplay.Length - 1)];
            this.slots[1].sprite = this.slotDisplay[Random.Range(0, this.slotDisplay.Length - 1)];
            this.slots[2].sprite = this.slotDisplay[Random.Range(0, this.slotDisplay.Length - 1)];
            
        }
        this.disableSpinRoll = true;
        this.spinResult = Reels();
        fruits = spinResult[0] + " - " + spinResult[1] + " - " + spinResult[2];
        Debug.Log(fruits);

        this.determineWinnings();
        
        showPlayerStats();
        validateBet(); //for update display here. 
        turn++;

    }

}
