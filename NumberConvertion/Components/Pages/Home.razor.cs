namespace NumberConvertion.Components.Pages
{
    public partial class Home
    {
        private string convertedString = "";
        private string _inputValue = "";
        private string inputValue
        {
            get => _inputValue;
            set
            {
                _inputValue = value;
                ValidateInput();
            }
        }
        private bool isValid = true;
        private string errorMessage = "";
        private List<string> numbers = new List<string>
        {
            "", // Zero
            "ONE",
            "TWO",
            "THREE",
            "FOUR",
            "FIVE",
            "SIX",
            "SEVEN",
            "EIGHT",
            "NINE"
        };
        private List<string> teens = new List<string>
        {
            "TEN",
            "ELEVEN",
            "TWELVE",
            "THIRTEEN",
            "FOURTEEN",
            "FIFTEEN",
            "SIXTEEN",
            "SEVENTEEN",
            "EIGHTEEN",
            "NINETEEN"
        };
        private List<string> tens = new List<string>
        {
            "", // Add to ensure correct index alignment
            "", // Add to ensure correct index alignment
            "TWENTY",
            "THIRTY",
            "FORTY",
            "FIFTY",
            "SIXTY",
            "SEVENTY",
            "EIGHTY",
            "NINETY"
        };
        private List<string> groups = new List<string>
        {
            "", // Add to ensure correct index alignment
            "THOUSAND",
            "MILLION",
            "BILLION",
            "TRILLION"
        };

        // Function to clear the page
        private void ClearPage()
        {
            inputValue = "";
            convertedString = "";
            isValid = true;
            errorMessage = "";
        }

        // Function to convert the valid input to a string
        private void ConvertValue()
        {
            if (ValidateInput())
            {
                // Split the input into a whole and decimal part to manipulate. Trim leading 0's, ensure still >= '0'
                string[] splitInput = inputValue.Split('.');
                string wholeNumber = splitInput[0].TrimStart('0');
                if (string.IsNullOrEmpty(wholeNumber)) wholeNumber = "0";
                string decimalNumber = splitInput.Length > 1 ? splitInput[1] : "";

                // Split the whole number into groups of 3 to manipulate as hundred, tens, ones
                List<string> numberGroups = SplitIntoThrees(wholeNumber);

                // Process the whole number groups and build the string according to tril, bil, mil, thousands
                int tempCount = numberGroups.Count - 1;
                for (int i = 0; i < numberGroups.Count; i++)
                {
                    int groupValue = int.Parse(numberGroups[i]);
                    if (groupValue != 0)
                    {
                        convertedString += ConvertGroup(numberGroups[i]) + " " + groups[tempCount] + " ";
                    }
                    tempCount--;
                }

                // Assign the correct dollar version
                convertedString += long.Parse(wholeNumber) == 1 ? " DOLLAR" : " DOLLARS";

                // Process decimal number. Assign correct cent version
                if (!string.IsNullOrEmpty(decimalNumber))
                {
                    convertedString += " AND " + ConvertGroup(decimalNumber) + (decimalNumber == "01" ? " CENT" : " CENTS");
                }
            }
        }

        // Function to convert the smaller number to a string
        private string ConvertGroup(string number)
        {
            string numberString = "";
            int length = number.Length;
            int position = 0;
            int hundredsColumn = 0;
            int tensColumn = 0;
            int onesColumn = 0;

            // Process the hundreds column
            if (length == 3)
            {
                hundredsColumn = number[position] - '0';
                tensColumn = number[position + 1] - '0';
                onesColumn = number[position + 2] - '0';

                if (hundredsColumn != 0)
                {
                    numberString = numbers[hundredsColumn] + " HUNDRED";
                    if (tensColumn == 0 && onesColumn == 0)
                    {
                        return numberString;
                    }
                }

                numberString += " AND ";

                length--;
                position++;
            }

            // Process the tens column
            if (length == 2)
            {
                tensColumn = number[position] - '0';
                onesColumn = number[position + 1] - '0';

                if (tensColumn > 1)
                {
                    numberString += tens[tensColumn];
                    if (onesColumn == 0)
                    {
                        return numberString;
                    }
                    numberString += "-";
                }
                else if (tensColumn == 1)
                {
                    return numberString += teens[onesColumn];
                }
                else if (tensColumn == 0 && onesColumn == 0)
                {
                    return numberString;
                }

                length--;
                position++;
            }

            // Process the ones column
            if (length == 1)
            {
                onesColumn = number[position] - '0';

                if (tensColumn == 0 && onesColumn == 0)
                {
                    return numberString;
                }

                numberString += numbers[onesColumn];
            }

            return numberString;
        }

        // Function to split the whole number into groups of 3
        private List<string> SplitIntoThrees(string number)
        {
            List<string> groups = new List<string>();

            for (int i = number.Length; i > 0; i -= 3)
            {
                int start = Math.Max(0, i - 3);
                int length = i - start;
                groups.Insert(0, number.Substring(start, length));
            }

            return groups;
        }

        // Function to validate the input
        private bool ValidateInput()
        {
            // Reset validation state
            isValid = true;
            errorMessage = "";
            convertedString = "";

            // Check if input is empty
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                isValid = false;
                errorMessage = "Please enter a numeric dollar amount.";
                return false;
            }

            // Check if input matches the pattern (up to 15 digits before decimal, exactly 2 after) OR integer < 1 Quadrillion
            if (!System.Text.RegularExpressions.Regex.IsMatch(inputValue, @"^\d{1,15}(\.\d{2})?$"))
            {
                isValid = false;
                errorMessage = "Format: Maximum 15 digits before decimal, exactly 2 digits after, or no decimal (integer).";
                return false;
            }

            // Parse and check if value is positive and less than 1 quadrillion
            decimal result = decimal.Parse(inputValue);
            if (result <= 0)
            {
                isValid = false;
                errorMessage = "Value must be positive.";
                return false;
            }
            if (result >= 1000000000000000m)
            {
                isValid = false;
                errorMessage = "Value must be less than 1 quadrillion.";
                return false;
            }

            return true;
        }
    }
}