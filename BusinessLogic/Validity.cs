namespace ALib.BusinessLogic;



using System;
using System.Runtime.InteropServices.Marshalling;

public static class Validity
{
    public static bool IsValidName(string name)
    {
        name = name.Trim();
        bool allLetters = AllLetters(name);
        if(allLetters && name.Length <= 50)
        {
            return true;
        }
        return false;
    }
    private static bool AllLetters(string name)
    {
        name = name.Trim();
        for(int i = 0; i < name.Length; i++)
        {
            if (!char.IsLetter(name[i]))
            {
                return false;
            }
        }
        return true;
    }
    public static string FormatName(string name, out bool isValid)
    {
        isValid = false;
        name = name.Trim();
        string[] splitOnlyFName = name.Split();
        name = splitOnlyFName[0];
        bool isValidToFormat = IsValidName(name);
        if(!isValidToFormat)
        {
            isValid = false;
            return "Invalid format for a name!";
        }

        string formatedName = "";
        formatedName += name.ToUpper()[0];
        for(int i = 1; i < name.Length; i++)
        {
            formatedName += name[i];
        }

        isValid = true;
        return formatedName;
    }
    private static bool LeapYear(int year)
    {
        if(year % 4 == 0)
        {
            if(year % 100 == 0 && year % 400 != 0)
            {
                return false;
            }
            return true;
        }
        return false;
    }
    public static string AgeGroup(DateOnly dob, out string exactAge)
    {
        int year = DateTime.Now.Year - dob.Year;
        int month = DateTime.Now.Month - dob.Month;
        int day = DateTime.Now.Day - dob.Day;

        if (day < 0)
        {
            --month;
            if (month < 0)
            {
                --year;
                month += 12;
            }
            if (month == 1)
            {
                day += 31;
            }
            else if(month == 2)
            {
                if(LeapYear(year))
                {
                    day += 29;
                }
                else
                {
                    day += 28;
                }
            }
            else if (month == 3)
            {
                day += 31;
            }
            else if (month == 4)
            {
                day += 30;
            }
            else if (month == 5)
            {
                day += 31;
            }
            else if (month == 6)
            {
                day += 30;
            }
            else if (month == 7)
            {
                day += 31;
            }
            else if (month == 8)
            {
                day += 31;
            }
            else if (month == 9)
            {
                day += 30;
            }
            else if (month == 10)
            {
                day += 31;
            }
            else if (month == 11)
            {
                day += 30;
            }
            else if (month == 12)
            {
                day += 31;
            }
        }
        else if(month < 0)
        {
            --year;
            month += 12;
        }


        exactAge = "- years, - months, - days.";
        if(year >= 0)
        {
            exactAge = $"{year} years, {month} months, {day} days.";
        }


        if (year < 0)
        {
            return "Invalid Dob";
        }
        else if(year < 18)
        {
            return "Below 18";
        }
        else if(year < 65)
        {
            return "Working age";
        }
        else
        {
            return "Old";
        }
    }
    public static bool IsTypeOfALibPassword(string password)
    {
        bool isUpper = false;
        bool isLower = false;
        bool isDigit = false;
        bool isSpecialchar = false;
        bool containsWhiteSpace = false;

        for (int i = 0; i < password.Length; i++)
        {
            if (char.IsUpper(password[i]))
            {
                isUpper = true;
            }
            else if (char.IsLower(password[i]))
            {
                isLower = true;
            }
            else if (char.IsDigit(password[i]))
            {
                isDigit = true;
            }
            else if (password[i] != ' ')
            {
                isSpecialchar = true;
            }
            else if (password[i] ==  ' ')
            {
                containsWhiteSpace = true;
            }
        }

        if(isUpper && isLower && isDigit && isSpecialchar && 
            !containsWhiteSpace && password.Length >= 8 && password.Length <= 50)
        {
            return true;
        }
        return false;
    }
    public static string GenerateALibPassword()
    {
        string strongPassword = "";
        Random random = new Random();
        int randomNumber;
        char randomULetter;
        char randomLLetter;
        char randomSpecialChar;
        char[] specialCharacters = {
            '!', '\"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', 
            ',', '-', '.', '/', ':', ';', '<', '=', '>', '?', '@', '[', '\\', ']', 
            '^', '_', '`', '{', '|', '}', '~'
        };

        for(byte j = 0; j < 4; j++)
        {
            randomNumber = random.Next(10);
            randomULetter = (char)random.Next('A', 'Z' + 1);
            randomLLetter = (char)random.Next('a', 'z' + 1);
            randomSpecialChar = specialCharacters[random.Next(specialCharacters.Length)];

            strongPassword += randomULetter.ToString() + randomSpecialChar.ToString() + 
                              randomLLetter.ToString() + randomNumber.ToString();
        }

        return strongPassword;
    }
    public static bool IsTypeOfALibUsername(string username)
    {
        if(username.Length < 5 || username.Length > 50)
        {
            return false;
        }
        
        for(int i = 0; i < username.Length; i++)
        {
            if (username[i] == ' ')
            {
                return false; // username can't hold whiteSpace
            }
            else if (username[i] == '\n')
            {
                return false; // username can't hold new line
            }
        }

        return true;
    }
    public static bool IsEthPhoneNumber(string phoneNumber, out string formattedPhoneNum)
    {
        phoneNumber = phoneNumber.Trim();
        bool isDig = false;

        if (phoneNumber[0] == '+' && phoneNumber[1] == '2' && phoneNumber[2] == '5' && phoneNumber[3] == '1'
            && phoneNumber[4] == '9' && phoneNumber.Length == 13)
        {
            isDig = IsOnlyDigit(phoneNumber.Substring(5));
            if(isDig)
            {
                formattedPhoneNum = "09" + phoneNumber[5] + phoneNumber[6] + phoneNumber[7] + phoneNumber[8] + phoneNumber[9] +
                    phoneNumber[10] + phoneNumber[11] + phoneNumber[12];
            }
            else
            {
                formattedPhoneNum = "Not Ethiopian Phone Number";
            }

            return isDig;
        }
        else if (phoneNumber[0] == '2' && phoneNumber[1] == '5' && phoneNumber[2] == '1' && phoneNumber[3] == '9'
            && phoneNumber.Length == 12)
        {
            isDig = IsOnlyDigit(phoneNumber.Substring(4));
            if(isDig)
            {
                formattedPhoneNum = "09" + phoneNumber[4] + phoneNumber[5] + phoneNumber[6] + phoneNumber[7] + phoneNumber[8] +
                    phoneNumber[9] + phoneNumber[10] + phoneNumber[11];
            }
            else
            {
                formattedPhoneNum = "Not Ethiopian Phone Number";
            }

            return isDig;
        }
        else if (phoneNumber[0] == '0' && phoneNumber[1] == '9' && phoneNumber.Length == 10)
        {
            isDig = IsOnlyDigit(phoneNumber.Substring(2));
            if(isDig)
            {
                formattedPhoneNum = phoneNumber.Trim();
            }
            else
            {
                formattedPhoneNum = "Not Ethiopian Phone Number";
            }

            return isDig;
        }
        else
        {
            formattedPhoneNum = "Not Ethiopian Phone Number";
            return false;
        }
    }
    public static bool IsOnlyDigit(string number)
    {
        number = number.Trim();
        foreach(char c in number)
        {
            if(!char.IsDigit(c))
            {
                return false;
            }
        }
        return true;
    }
    public static bool IsGmail(string gmail, out string detail)
    {
        detail = "";
        gmail = gmail.Trim();
        if(gmail.Length <= 10)
        {
            detail = "Invalid Gmail";
            return false;
        }
        string domainName = gmail.Substring(gmail.Length - 10);
        string username = gmail.Substring(0, gmail.Length - 10);

        if(domainName != "@gmail.com")
        {
            detail = "The domain must be \"gmail.com\" since we only allow Gmail, and the '@' " +
                "symbol is must to separate the username from the domain name.";
        }
        bool usernameConsistAt = false;
        foreach(char c in username)
        {
            if(c == '@')
            {
                usernameConsistAt = true;
            }
        }
        if(usernameConsistAt)
        {
            if(detail == "")
            {
                detail = "Username can't consist the '@' symbol.";
            }
            else
            {
                detail += "\nAdditionally username can't consist the '@' symbol.";
            }
        }
        if(detail != "")
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}