using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberConvertion.Components.Pages;
using System.Reflection;

namespace NumberConversion.Tests
{
    [TestClass]
    public class ValidationTests
    {
        private Home homeComponent;
        
        [TestInitialize]
        public void Setup()
        {
            homeComponent = new Home();
        }
        
        #region Basic Setup Tests
        
        [TestMethod]
        [TestCategory("Setup")]
        public void HomeClass_ShouldInstantiate()
        {
            Assert.IsNotNull(homeComponent, "Home component should instantiate successfully");
        }
        
        [TestMethod]
        [TestCategory("Setup")]
        public void InitialState_ShouldBeValid()
        {
            bool isValid = GetIsValid();
            string errorMessage = GetErrorMessage();
            
            Assert.IsTrue(isValid, "Initial state should be valid");
            Assert.AreEqual("", errorMessage, "Initial error message should be empty");
        }
        
        #endregion
        
        #region Input Format Validation Tests
        
        [TestMethod]
        [TestCategory("Format")]
        [Priority(1)]
        public void ValidFormat_SingleDigit_ShouldPass()
        {
            TestValidInput("1");
            TestValidInput("9");
        }
        
        [TestMethod]
        [TestCategory("Format")]
        [Priority(1)]
        public void ValidFormat_MultipleDigits_ShouldPass()
        {
            TestValidInput("12");
            TestValidInput("123");
            TestValidInput("1234");
            TestValidInput("12345");
        }
        
        [TestMethod]
        [TestCategory("Format")]
        [Priority(1)]
        public void ValidFormat_WithTwoDecimals_ShouldPass()
        {
            TestValidInput("1.00");
            TestValidInput("12.34");
            TestValidInput("123.45");
            TestValidInput("1234.56");
        }
        
        [TestMethod]
        [TestCategory("Format")]
        [Priority(1)]
        public void ValidFormat_MaximumDigits_ShouldPass()
        {
            TestValidInput("123456789012345"); // 15 digits
            TestValidInput("999999999999999"); // 15 nines
            TestValidInput("100000000000000"); // 15 digits starting with 1
        }
        
        [TestMethod]
        [TestCategory("Format")]
        [Priority(1)]
        public void ValidFormat_MaximumWithDecimals_ShouldPass()
        {
            TestValidInput("123456789012345.99"); // 15 digits + decimals
            TestValidInput("999999999999999.01"); // Max + min decimals
        }
        
        [TestMethod]
        [TestCategory("Format")]
        [Priority(1)]
        public void InvalidFormat_EmptyString_ShouldFail()
        {
            TestInvalidInput("", "Please enter a numeric dollar amount");
        }
        
        [TestMethod]
        [TestCategory("Format")]
        [Priority(1)]
        public void InvalidFormat_WhitespaceOnly_ShouldFail()
        {
            TestInvalidInput(" ", "Please enter a numeric dollar amount");
            TestInvalidInput("   ", "Please enter a numeric dollar amount");
            TestInvalidInput("\t", "Please enter a numeric dollar amount");
        }
        
        [TestMethod]
        [TestCategory("Format")]
        [Priority(1)]
        public void InvalidFormat_NonNumeric_ShouldFail()
        {
            TestInvalidInput("abc", "Format: Maximum 15 digits before decimal");
            TestInvalidInput("12a", "Format: Maximum 15 digits before decimal");
            TestInvalidInput("a12", "Format: Maximum 15 digits before decimal");
            TestInvalidInput("1.2a", "Format: Maximum 15 digits before decimal");
        }
        
        [TestMethod]
        [TestCategory("Format")]
        [Priority(1)]
        public void InvalidFormat_TooManyDigits_ShouldFail()
        {
            TestInvalidInput("1234567890123456", "Format: Maximum 15 digits before decimal"); // 16 digits
            TestInvalidInput("12345678901234567", "Format: Maximum 15 digits before decimal"); // 17 digits
        }
        
        [TestMethod]
        [TestCategory("Format")]
        [Priority(1)]
        public void InvalidFormat_WrongDecimalPlaces_ShouldFail()
        {
            TestInvalidInput("12.3", "Format: Maximum 15 digits before decimal"); // 1 decimal place
            TestInvalidInput("12.345", "Format: Maximum 15 digits before decimal"); // 3 decimal places
            TestInvalidInput("12.3456", "Format: Maximum 15 digits before decimal"); // 4 decimal places
        }
        
        [TestMethod]
        [TestCategory("Format")]
        [Priority(1)]
        public void InvalidFormat_MultipleDecimals_ShouldFail()
        {
            TestInvalidInput("12.34.56", "Format: Maximum 15 digits before decimal");
            TestInvalidInput("1.2.3", "Format: Maximum 15 digits before decimal");
            TestInvalidInput("..12", "Format: Maximum 15 digits before decimal");
        }
        
        [TestMethod]
        [TestCategory("Format")]
        [Priority(1)]
        public void InvalidFormat_SpecialCharacters_ShouldFail()
        {
            TestInvalidInput("$123", "Format: Maximum 15 digits before decimal");
            TestInvalidInput("123,456", "Format: Maximum 15 digits before decimal");
            TestInvalidInput("123.45$", "Format: Maximum 15 digits before decimal");
            TestInvalidInput("-123", "Format: Maximum 15 digits before decimal");
            TestInvalidInput("+123", "Format: Maximum 15 digits before decimal");
        }

        #endregion

        #region Value Range Validation Tests
        
        [TestMethod]
        [TestCategory("Range")]
        [Priority(1)]
        public void ValidRange_Zero_ShouldPass()
        {
            TestValidInput("0");
            TestValidInput("0.00");
        }
        
        [TestMethod]
        [TestCategory("Range")]
        [Priority(1)]
        public void ValidRange_MinimumValue_ShouldPass()
        {
            TestValidInput("0.01"); // Minimum valid value
        }
        
        [TestMethod]
        [TestCategory("Range")]
        [Priority(1)]
        public void ValidRange_SmallValues_ShouldPass()
        {
            TestValidInput("0.02");
            TestValidInput("0.99");
            TestValidInput("1.00");
            TestValidInput("1.01");
        }
        
        [TestMethod]
        [TestCategory("Range")]
        [Priority(1)]
        public void ValidRange_LargeValues_ShouldPass()
        {
            TestValidInput("999999999999999"); // Maximum possible: 15 digits (999 trillion)
            TestValidInput("999999999999998"); // Just below maximum
            TestValidInput("100000000000000"); // 100 trillion
        }
        
        [TestMethod]
        [TestCategory("Range")]
        [Priority(1)]
        public void ValidRange_MaximumPossible_ShouldPass()
        {
            // The actual maximum is 999,999,999,999,999 (15 digits = 999 trillion)
            // This is less than 1 quadrillion and passes both regex and value checks
            TestValidInput("999999999999999");
            TestValidInput("999999999999999.99"); // With decimals
        }
        
        [TestMethod]
        [TestCategory("Range")]
        [Priority(1)]
        public void InvalidRange_SixteenDigits_FailsOnFormat()
        {
            // 1 quadrillion has 16 digits, so it fails regex format check first
            // The quadrillion value check never gets reached
            TestInvalidInput("1000000000000000", "Format: Maximum 15 digits before decimal");
        }
        
        [TestMethod]
        [TestCategory("Range")]
        [Priority(1)]
        public void InvalidRange_MoreThanSixteenDigits_FailsOnFormat()
        {
            // Any number with 16+ digits fails format check, never reaches value check
            TestInvalidInput("10000000000000000", "Format: Maximum 15 digits before decimal");
            TestInvalidInput("100000000000000000", "Format: Maximum 15 digits before decimal");
        }
        
        #endregion
        
        #region Edge Cases and Boundary Tests
        
        [TestMethod]
        [TestCategory("EdgeCase")]
        [Priority(2)]
        public void EdgeCase_LeadingZeros_ShouldPass()
        {
            // Leading zeros should be handled properly in conversion
            TestValidInput("0001");
            TestValidInput("00123");
            TestValidInput("001.23");
        }
        
        [TestMethod]
        [TestCategory("EdgeCase")]
        [Priority(2)]
        public void EdgeCase_ActualMaximumValue_ShouldPass()
        {
            // The actual maximum is 999,999,999,999,999 (15 digits)
            // This is approximately 999.999 trillion, well below 1 quadrillion
            TestValidInput("999999999999999"); // 999 trillion, 999 billion, 999 million, 999 thousand, 999
            TestValidInput("999999999999999.99"); // Maximum with decimals
        }
        
        [TestMethod]
        [TestCategory("EdgeCase")]
        [Priority(2)]
        public void EdgeCase_OneQuadrillionFailsOnFormat()
        {
            // 1 quadrillion = 1,000,000,000,000,000 (16 digits)
            // This fails regex format check first, never reaches value check
            TestInvalidInput("1000000000000000", "Format: Maximum 15 digits before decimal");
        }
        
        [TestMethod]
        [TestCategory("EdgeCase")]
        [Priority(2)]
        public void EdgeCase_QuadrillionCheckNeverTriggered()
        {
            // Document that the quadrillion check is defensive code
            // Any 16+ digit number fails format first
            // The largest 15-digit number (999,999,999,999,999) is only ~1000 trillion
            // So the quadrillion check (1,000,000,000,000,000) never executes
            TestInvalidInput("1000000000000000", "Format: Maximum 15 digits before decimal");
            TestInvalidInput("9999999999999999", "Format: Maximum 15 digits before decimal"); // 16 nines
        }
        
        [TestMethod]
        [TestCategory("EdgeCase")]
        [Priority(2)]
        public void EdgeCase_DecimalBoundaries_ShouldWork()
        {
            TestValidInput("1.99"); // High decimal
            TestValidInput("1.01"); // Low decimal  
            TestValidInput("1.00"); // Zero decimal
        }
        
        #endregion
        
        #region State Management Tests
        
        [TestMethod]
        [TestCategory("State")]
        [Priority(2)]
        public void StateManagement_ErrorMessageClears_OnValidInput()
        {
            // First set invalid input
            SetInputValue("invalid");
            string errorAfterInvalid = GetErrorMessage();
            Assert.IsTrue(!string.IsNullOrEmpty(errorAfterInvalid), "Should have error message after invalid input");
            
            // Then set valid input
            SetInputValue("123.45");
            string errorAfterValid = GetErrorMessage();
            bool isValid = GetIsValid();
            
            Assert.IsTrue(isValid, "Should be valid after setting valid input");
            Assert.AreEqual("", errorAfterValid, "Error message should clear after valid input");
        }
        
        [TestMethod]
        [TestCategory("State")]
        [Priority(2)]
        public void StateManagement_ValidStateChanges_OnInvalidInput()
        {
            // Start with valid input
            SetInputValue("123.45");
            Assert.IsTrue(GetIsValid(), "Should start valid");
            
            // Change to invalid input
            SetInputValue("invalid");
            bool isValid = GetIsValid();
            string errorMessage = GetErrorMessage();
            
            Assert.IsFalse(isValid, "Should become invalid");
            Assert.IsTrue(!string.IsNullOrEmpty(errorMessage), "Should have error message");
        }
        
        [TestMethod]
        [TestCategory("State")]
        [Priority(2)]
        public void StateManagement_ConvertedStringClears_OnValidation()
        {
            // Set a valid value and convert (simulate)
            SetInputValue("123.45");
            
            // Change to invalid value
            SetInputValue("invalid");
            string convertedString = GetConvertedString();
            
            Assert.AreEqual("", convertedString, "Converted string should clear on validation failure");
        }
        
        #endregion
        
        #region Comprehensive Test Cases
        
        [TestMethod]
        [TestCategory("Comprehensive")]
        [Priority(3)]
        public void Comprehensive_AllValidFormats_ShouldPass()
        {
            string[] validInputs = {
                "1", "12", "123", "1234", "12345",
                "123456", "1234567", "12345678", "123456789",
                "1234567890", "12345678901", "123456789012",
                "1234567890123", "12345678901234", "123456789012345",
                "1.01", "12.34", "123.45", "1234.56", "12345.67",
                "123456.78", "1234567.89", "12345678.90", "123456789.01",
                "1234567890.12", "12345678901.23", "123456789012.34",
                "1234567890123.45", "12345678901234.56", "123456789012345.67"
            };
            
            foreach (string input in validInputs)
            {
                TestValidInput(input);
            }
        }
        
        [TestMethod]
        [TestCategory("Comprehensive")]
        [Priority(3)]
        public void Comprehensive_AllInvalidFormats_ShouldFail()
        {
            var invalidInputs = new Dictionary<string, string>
            {
                // Empty/whitespace
                {"", "Please enter a numeric dollar amount"},
                {" ", "Please enter a numeric dollar amount"},
                {"   ", "Please enter a numeric dollar amount"},
                
                // Non-numeric
                {"abc", "Format: Maximum 15 digits before decimal"},
                {"12a", "Format: Maximum 15 digits before decimal"},
                {"a12", "Format: Maximum 15 digits before decimal"},
                
                // Wrong decimal places
                {"12.3", "Format: Maximum 15 digits before decimal"},
                {"12.345", "Format: Maximum 15 digits before decimal"},
                
                // Too many digits
                {"1234567890123456", "Format: Maximum 15 digits before decimal"},
                
                // 16+ digits (fail on format, never reach value check)
                {"1000000000000000", "Format: Maximum 15 digits before decimal"} // 1 quadrillion (16 digits)
            };
            
            foreach (var testCase in invalidInputs)
            {
                TestInvalidInput(testCase.Key, testCase.Value);
            }
        }
        
        #endregion
        
        #region Regex Pattern Tests
        
        [TestMethod]
        [TestCategory("Regex")]
        [Priority(2)]
        public void RegexPattern_ValidFormats_ShouldMatch()
        {
            string pattern = @"^\d{1,15}(\.\d{2})?$";
            
            string[] validInputs = {
                "1", "12", "123", 
                "123456789012345", // 15 digits (maximum allowed)
                "1.00", "12.34", "123456789012345.99" // With valid decimals
            };
            
            foreach (string input in validInputs)
            {
                bool matches = System.Text.RegularExpressions.Regex.IsMatch(input, pattern);
                Assert.IsTrue(matches, $"Pattern should match valid input: {input}");
            }
        }
        
        [TestMethod]
        [TestCategory("Regex")]
        [Priority(2)]
        public void RegexPattern_InvalidFormats_ShouldNotMatch()
        {
            string pattern = @"^\d{1,15}(\.\d{2})?$";
            
            string[] invalidInputs = {
                "", "abc", "12a", "a12", "12.3", "12.345",
                "1234567890123456", // 16 digits (exceeds 15-digit limit)
                "1000000000000000", // 1 quadrillion (16 digits)
                "12.34.56", "-12", "+12", "12,34"
            };
            
            foreach (string input in invalidInputs)
            {
                bool matches = System.Text.RegularExpressions.Regex.IsMatch(input, pattern);
                Assert.IsFalse(matches, $"Pattern should not match invalid input: {input}");
            }
        }
        
        [TestMethod]
        [TestCategory("Regex")]
        [Priority(2)]
        public void RegexPattern_BoundaryTests_ShouldWork()
        {
            string pattern = @"^\d{1,15}(\.\d{2})?$";
            
            // Test 15-digit boundary (should pass)
            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch("999999999999999", pattern), 
                "15 digits should pass regex");
            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch("100000000000000", pattern), 
                "15 digits should pass regex");
            
            // Test 16-digit boundary (should fail)
            Assert.IsFalse(System.Text.RegularExpressions.Regex.IsMatch("1000000000000000", pattern), 
                "16 digits should fail regex");
            Assert.IsFalse(System.Text.RegularExpressions.Regex.IsMatch("9999999999999999", pattern), 
                "16 digits should fail regex");
        }
        
        #endregion
        
        #region Helper Methods
        
        private void TestValidInput(string input)
        {
            SetInputValue(input);
            bool isValid = GetIsValid();
            string errorMessage = GetErrorMessage();
            
            Assert.IsTrue(isValid, $"Input '{input}' should be valid. Error: {errorMessage}");
            Assert.AreEqual("", errorMessage, $"Valid input '{input}' should have no error message");
        }
        
        private void TestInvalidInput(string input, string expectedErrorPart)
        {
            SetInputValue(input);
            bool isValid = GetIsValid();
            string errorMessage = GetErrorMessage();
            
            Assert.IsFalse(isValid, $"Input '{input}' should be invalid");
            Assert.IsTrue(errorMessage.Contains(expectedErrorPart), 
                $"Error message '{errorMessage}' should contain '{expectedErrorPart}' for input '{input}'");
        }
        
        private void SetInputValue(string value)
        {
            var property = typeof(Home).GetProperty("inputValue", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            property?.SetValue(homeComponent, value);
        }
        
        private bool GetIsValid()
        {
            var field = typeof(Home).GetField("isValid", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            return (bool)(field?.GetValue(homeComponent) ?? false);
        }
        
        private string GetErrorMessage()
        {
            var field = typeof(Home).GetField("errorMessage", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            return (string)(field?.GetValue(homeComponent) ?? "");
        }
        
        private string GetConvertedString()
        {
            var field = typeof(Home).GetField("convertedString", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            return (string)(field?.GetValue(homeComponent) ?? "");
        }
        
        #endregion
    }
}