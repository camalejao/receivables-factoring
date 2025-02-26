namespace ReceivablesFactoring.Domain.Extensions;

public static class CnpjExtensions
{
    public static bool IsValidCnpj(this string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return false;

        if (cnpj.Contains('.') || cnpj.Contains('-') || cnpj.Contains('/'))
            return false;

        if (cnpj.Length != 14)
            return false;

        int[] multiplyer1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplyer2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        
        string tmpCnpj = cnpj.Substring(0, 12);
        
        int sum = 0;
        for (int i = 0; i < 12; i++)
            sum += int.Parse(tmpCnpj[i].ToString()) * multiplyer1[i];

        int remainder = (sum % 11);
        remainder = (remainder < 2) ? 0 : 11 - remainder;

        string digit = remainder.ToString();
        tmpCnpj = tmpCnpj + digit;
        
        sum = 0;
        for (int i = 0; i < 13; i++)
            sum += int.Parse(tmpCnpj[i].ToString()) * multiplyer2[i];

        remainder = (sum % 11);
        remainder = (remainder < 2) ? 0 : 11 - remainder;

        digit = digit + remainder.ToString();

        return cnpj.EndsWith(digit);
    }
}
