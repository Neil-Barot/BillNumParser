namespace BillNumParser;

using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;

public class BillNumber
{
    private static readonly HashSet<string> _validChamber = new(StringComparer.OrdinalIgnoreCase) { "H", "S" };
    private static readonly HashSet<string> _validType = new(StringComparer.OrdinalIgnoreCase) {"B", "R", "CR", "JR"};

    
    private string _chamber;
    private string _type;
    private int _suffix;
    public bool IsValid { get; }



    public BillNumber(string id)
    {

        this.IsValid = ParseId(id, out this._chamber, out this._type, out this._suffix);

    }

    // Description: Formats the bill in long form: ex. hr 0205 -> HR00205
    // Returns: Formatted bill number
    public string BillNumberLong()
    {
        CheckValid();
        string suffixStr = _suffix.ToString();
        var zeros = new StringBuilder();
        int leadingZeros = 5 - suffixStr.Length;
        for (int count = 0; count < leadingZeros; count++)
        {
            zeros.Append('0');
        }

        string id = $"{_chamber}{_type}{zeros.ToString()}{suffixStr}";

        return id.ToUpper();
    }

    // Description: Formats the bill in short form: ex. hr0205 -> HR 205
    // Returns: Formatted bill number
    public string BillNumberShort()
    {
        CheckValid();
        string id = $"{_chamber}{_type} {_suffix.ToString()}";
        return id.ToUpper();
    }

    // Description: Used for exception handling
    private void CheckValid()
    {
        if (!IsValid)
        {
            throw new InvalidBillNumberException("Bill number is invalid");
        }
    }

    // Description: Parsing logic for each section of the id
    // Returns: boolean value based on validity of the id
    private static bool ParseId(string id, out string chamber, out string type, out int suffix)
    {
        chamber = null;
        type = null;
        suffix = -1;

        if (id == null)
        {
            return false;
        }

        string modifiedId = id.Trim();

        // Find and validate the chamber
        if (!parseHead(modifiedId, _validChamber, out chamber, out modifiedId))
        {
            return false;
        }

        // Find and validate the type
        if (!parseHead(modifiedId, _validType, out type, out modifiedId))
        {
            return false;
        }

        // Find and validate the suffix
        if (!parseSuffix(modifiedId, out suffix))
        {
            return false;
        }

        return true;
    }

    // Description: Parsing logic for the tail portion of the id. Finds the Suffix.
    // Returns: boolean value based on validity of the suffix
    private static bool parseSuffix(string id, out int suffix)
    {
        suffix = -1;

        int maxWhiteSpaceCount = 1; // Number of allowed whitespaces before the suffix
        int maxDigitCount = 5; // The number of digits that the tail can contain (including leading 0s)
        bool foundDigit = false;
        var tempSuffix = new StringBuilder();

        foreach (char letter in id)
        {
            // Verifies that the letter is a digit and that the maxDigitCount isn't exceeded
            if (Char.IsDigit(letter) && maxDigitCount > 0)
            {
                tempSuffix.Append(letter);
                foundDigit = true;
                maxDigitCount--;
            }
            // Verifies that whitespace is only found before the digits and that maxWhiteSpaceCount isn't exceeded
            else if (Char.IsWhiteSpace(letter) && !foundDigit && maxWhiteSpaceCount > 0)
            {
                maxWhiteSpaceCount--;
            }
            else
            {
                return false;
            }
        }

        suffix = int.Parse(tempSuffix.ToString());

        // Verifies that the tail doesn't consist of only 0's
        if (suffix <= 0)
        {
            return false;
        }

        return true;
    }

    // Description: Parsing logic for the head portion of the id.
    // Parameters: 
    //      - id: the bill number
    //      - set: set corresponding to the item you're looking for (ex. _validChamber)
    //      - token: returns the match that was found between a substring of the id and the set 
    //               (ex. returns 'h' if set = _validChamber and id = 'hr05')
    //      - trimmedId: returns id with the token removed from the front (ex. returns 'r05' if token = 'h')
    // Returns: boolean value based on validity of the suffix
    private static bool parseHead(string id, HashSet<string> set, out string token, out string trimmedId)
    {
        token = null;
        trimmedId = id;

        for (int pointer = 0; pointer < id.Length; pointer++)
        {
            var temp = id.Substring(0, pointer);
            
            // Checks if the set contains the current substring of id
            if (set.Contains(temp))
            {
                token = temp;
                trimmedId = id.Substring(pointer);
                return true;
            }
        }

        return false;
    }
    
}