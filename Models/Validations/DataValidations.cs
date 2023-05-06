using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Globalization;


[AttributeUsage(AttributeTargets.Property | 
  AttributeTargets.Field, AllowMultiple = false)]
public class ValidateCardNum : ValidationAttribute
{
    private readonly string _property_name;
    public ValidateCardNum(string property_name)
    {
        _property_name = property_name;
    }

    private static bool IsNumericAndValidLength(object value)
    {
        bool isNumeric = Int64.TryParse(value.ToString(), out Int64 number);

        if (isNumeric)
        {
            return number.ToString().Length  == 15 
                || number.ToString().Length == 16;
        }
        else
        {
            return false;
        }
        
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        
        if (value == null)
        {
            return new ValidationResult($"{_property_name}is required");
        }

        if (IsNumericAndValidLength(value))
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult($"{_property_name} is invalid");
        }

    }


}

[AttributeUsage(AttributeTargets.Property | 
  AttributeTargets.Field, AllowMultiple = false)]
public class ValidateAcquirerAttribute : ValidationAttribute
{

    public override bool IsValid(object? value)
    {
        try
        {

            if (value == null)
            {
                return false;
            }

            if (value.ToString() == "402971"){
            return true;
            } else { return false; }

        } catch (Exception e){
            Console.WriteLine(e.Message);
            return false;
        }
        //Console.WriteLine("Validating AcqID");
        

    }

    public override string FormatErrorMessage(string name)
    {
        return String.Format(CultureInfo.CurrentCulture, 
            ErrorMessageString, name);
    }

}

[AttributeUsage(AttributeTargets.Property | 
  AttributeTargets.Field, AllowMultiple = false)]
public class ValidateOutletAttribute : ValidationAttribute
{

    public override bool IsValid(object? value)
    {
        try
        {

            if (value == null)
            {
                return false;
            }

            string outletId = (String)value;
            //Console.WriteLine("Validating OutletId");
            if (outletId.Length == 10 && outletId.All(char.IsDigit)){
                return true;
            } else { return false; }

        } catch (Exception e){
            Console.WriteLine(e.Message);
            return false;
        }
        

    }

    public override string FormatErrorMessage(string name)
    {
        return String.Format(CultureInfo.CurrentCulture, 
            ErrorMessageString, name);
    }

}

