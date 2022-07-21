// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

class Test
{
    static string currentDir = AppDomain.CurrentDomain.BaseDirectory; //Current directory
    static string oldJsonDir = @"oldJson\";
    static string newJsonDir = @"newJson\";
    static string jsonpath = currentDir + oldJsonDir; //Old metadata (*.json) directory
    static string jsonsavePath = currentDir + newJsonDir; //New metadata (*.json) directory
    static int realfiles = jsonpath.Length; //Number of files in directory

    public static void Computing( double t, int S, int i )
    {
        int JN = 0; //Number of digits in the input variable

        //Calculate Number of digits
        do
        {
            t /= 10;
            JN += 1;
        } while (t > 1);

        int[] number = new int[JN]; //Array type of input variable

        //Build new array
        for (int j = JN; j > 0; j--)
        {
            number[j - 1] = S % 10;
            S /= 10;
        }

        string path = jsonpath + i + ".json"; //Path of old metadata (*.json)

        string data = File.ReadAllText(path); //Get all content of metadata and store in variable "data"

        System.Console.WriteLine("Get information of NFT metadata successfully!"); //Check get progress in console

        char[] fulldata = data.ToCharArray(); //Declarating array variable of metadata

        int len = fulldata.Length; //Length of metadata

        //Spilts for data control
        int sharpIndex = data.IndexOf("#") + 1; //Target for first spilts
        char[] head = new char[sharpIndex]; //Declarating first piece of metadata

        //Get first piece of metadata
        for (int j = 0; j < sharpIndex; j++)
        {
            head[j] = fulldata[j];
        }

        int tailIndex = data.IndexOf("description") - 3;  //Target for third spilts
        char[] tail = new char[len - tailIndex]; //Declarating third piece of metadata

        //Get third piece of metadata
        for (int j = tailIndex, p = 0; j < len; j++, p++)
        {
            tail[p] = fulldata[j];
        }

        int totalLength = JN + sharpIndex + (len - tailIndex); //Length of completed data

        char[] finaldata = new char[totalLength]; //Declarating array variable that will contain completed data
        int BN = 0; //Variable for store data in completed data

        //Store first piece of old metadata to new array variable
        for (int j = 0; j < sharpIndex; j++, BN++)
        {
            finaldata[BN] = head[j];
        }
        //Store changed data to new array variable
        for (int j = 0; j < JN; j++, BN++)
        {
            finaldata[BN] = (char)(number[j] + 48); // * (+48) is need to convert 'int' to 'char' *
        }
        //Store third piece of old metadata to new array variable
        for (int j = 0; j < (len - tailIndex); j++, BN++)
        {
            finaldata[BN] = tail[j];
        }

        string InputData = new string(finaldata); //Convert array of char to string

        string savePath = jsonsavePath + i + ".json"; //Path of new metadata (*.json)

        File.WriteAllText(savePath, InputData, Encoding.UTF8); //Store completed data in new file and save as new metadata

        System.Console.WriteLine("Save information of NFT metadata successfully!"); //Check save progress in console
    }

    public static void Progress(int totallength, int mintlength, int[] mintnumber)
    {

        //------------- Calculating for minted NFTs -------------//
        for (int i = 0; i < mintlength; i++)
        {
            double t = mintnumber[i];
            int S = mintnumber[i];

            Computing( t, S, i);
        }


        //------------ Calculating for unminted NFTs ------------//
        for (int i = mintlength; i < totallength; i++)
        {
            double t = 5000 + i;
            int S = 5000 + i;

            Computing(t, S, i);
        }
    }
    public static void Main()
    {
        // ------- Declare basic variables for a project ------- //
        
        int totallength, mintlength;
        int[] mintnumber;

        System.Console.WriteLine("Please choose input method");
        System.Console.WriteLine("If you want to Manual Input Method, please input 'm'. Or If you want to File input Method, please input other key.");

        var inputmethod = Console.ReadLine();

        if(inputmethod == "m")
        {
            //------------------ Manual input method------------------//
            System.Console.WriteLine("Please input count of total NFTs");
            totallength = Convert.ToInt32(Console.ReadLine()); //Count of minted NFTs

            while (totallength > realfiles || totallength < 0)
            {
                Console.WriteLine("Your input count is greater than count of files in directory or smaller than zero. Please input correct count.");
                totallength = Convert.ToInt32(Console.ReadLine()); //Count of minted NFTs
            }

            System.Console.WriteLine("Please input count of minted NFTs");
            mintlength = Convert.ToInt32(Console.ReadLine()); //Count of minted NFTs

            mintnumber = new int[mintlength]; //Declarating array variable of minted NFT ids

            System.Console.WriteLine("Please input IDs of NFTs");
            for (int j = 0; j < mintlength; j++)
            {
                mintnumber[j] = Convert.ToInt32(Console.ReadLine());
                for (int p = 0; p < j; p++)
                {
                    while (mintnumber[p] == mintnumber[j])
                    {
                        System.Console.WriteLine("The minted NFT number is a duplicate. Please input correct ID");
                        mintnumber[j] = Convert.ToInt32(Console.ReadLine());
                    }
                    while (mintnumber[j] < 0 || mintnumber[j] >= totallength)
                    {
                        System.Console.WriteLine("The ID of minted NFTs is greater than the maximum number. Please input correct ID");
                        mintnumber[j] = Convert.ToInt32(Console.ReadLine());
                    }
                }
            }

            Progress(totallength, mintlength, mintnumber);

        }
        else
        {
            System.Console.WriteLine("Please input count of total NFTs");
            totallength = Convert.ToInt32(Console.ReadLine()); //Count of minted NFTs

            while (totallength > realfiles || totallength < 0)
            {
                Console.WriteLine("Your input count is greater than count of files in directory or smaller than zero. Please input correct count.");
                totallength = Convert.ToInt32(Console.ReadLine()); //Count of minted NFTs
            }

            Console.WriteLine("");
            string IDS = File.ReadAllText(currentDir + "IDS.txt");
            string[] source = IDS.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
            mintlength = source.Length;

            System.Console.WriteLine("Do you want to exchange all minted NFTs? Yes: y No: n");

            var choosecount = Console.ReadLine();

            if(choosecount != "y")
            {
                System.Console.WriteLine("Please input count of exchange NFTs");
                int currentmintlength = Convert.ToInt32(Console.ReadLine()); //Count of minted NFTs
                while(currentmintlength > mintlength || currentmintlength < 0)
                {
                    Console.WriteLine("Your input count is greater than count of file's data or smaller than zero. Please input correct count.");
                    currentmintlength = Convert.ToInt32(Console.ReadLine()); //Count of minted NFTs
                }
                mintlength = currentmintlength;
            }
            else
            {
                System.Console.WriteLine("I will exchange all minted NFTs");
            }

            mintnumber = new int[mintlength]; //Declarating array variable of minted NFT ids

            for (int j = 0; j < mintlength; j++)
            {
                mintnumber[j] = Convert.ToInt32(source[j]);
            }

            Progress(totallength, mintlength, mintnumber);
        }

        Console.WriteLine("Press any key to exit.");
        System.Console.ReadKey();
    }
}
