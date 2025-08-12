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
    public bool IsValid;



    public BillNumber(string id)
    {

        this.IsValid = ParseId(id, out this._chamber, out this._type, out this._suffix);

    }

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

    public string BillNumberShort()
    {
        CheckValid();
        string id = $"{_chamber}{_type} {_suffix.ToString()}";
        return id.ToUpper();
    }

    private void CheckValid()
    {
        if (!IsValid)
        {
            throw new InvalidBillNumberException("Bill number is invalid");
        }
    }

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

    private static bool parseSuffix(string id, out int suffix)
    {
        suffix = -1;

        int maxWhiteSpaceCount = 1;
        int maxDigitCount = 5;
        bool foundDigit = false;
        var tempSuffix = new StringBuilder();

        foreach (char letter in id)
        {
            if (Char.IsDigit(letter) && maxDigitCount > 0)
            {
                tempSuffix.Append(letter);
                foundDigit = true;
                maxDigitCount--;
            }
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

        if (suffix <= 0) {
            return false;
        }

        return true;
    }
    

    private static bool parseHead(string id, HashSet<string> set, out string token, out string trimmedId)
    {
        token = null;
        trimmedId = id;

        for (int pointer = 0; pointer < id.Length; pointer++)
        {
            var temp = id.Substring(0, pointer);
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