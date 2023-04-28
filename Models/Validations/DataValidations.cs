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
    
    public override bool IsValid(object? value)
    {
        bool result = true;
        List<int> card_valid_length = new List<int>(2) {15, 16};

        try
        {
            
            if (value == null)
            {
                return false;
            }

            string cardNum = (String)value;
        
            if ( card_valid_length.Contains(cardNum.Length) )
            {
                return result;
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

