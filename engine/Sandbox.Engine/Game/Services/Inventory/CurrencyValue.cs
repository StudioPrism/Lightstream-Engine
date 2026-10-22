namespace Sandbox;

/// <summary>
/// Describes money, in a certain currency
/// </summary>
public struct CurrencyValue : IEquatable<CurrencyValue>
{
	/// <summary>
	/// The name of the currency
	/// </summary>
	public readonly string Currency;

	/// <summary>
	/// The value without decimals. This is in the smallest denomination of the currency.
	/// </summary>
	public readonly long Value;

	public CurrencyValue( long price, string currency ) : this()
	{
		Currency = currency;
		Value = price;
	}

	public static CurrencyValue operator +( CurrencyValue m1, CurrencyValue m2 )
	{
		if ( !string.Equals( m1.Currency, m2.Currency, StringComparison.OrdinalIgnoreCase ) )
			throw new InvalidOperationException( "Cannot add amounts with different currencies." );

		return new CurrencyValue( m1.Value + m2.Value, m1.Currency );
	}

	public static CurrencyValue operator -( CurrencyValue m1, CurrencyValue m2 )
	{
		if ( !string.Equals( m1.Currency, m2.Currency, StringComparison.OrdinalIgnoreCase ) )
			throw new InvalidOperationException( "Cannot subtract amounts with different currencies." );

		return new CurrencyValue( m1.Value - m2.Value, m1.Currency );
	}

	public bool Equals( CurrencyValue other )
	{
		return string.Equals( Currency, other.Currency, StringComparison.OrdinalIgnoreCase ) && Value == other.Value;
	}

	public override bool Equals( object obj )
	{
		return obj is CurrencyValue other && Equals( other );
	}

	public override int GetHashCode()
	{
		return HashCode.Combine( Currency, Value );
	}

	public override string ToString()
	{
		return Format();
	}

	public readonly string Format()
	{
		var decimaled = (Value / 100.0f).ToString( "0.00" );

		switch ( Currency.ToUpperInvariant() )
		{
			case "AED": return $"{decimaled}د.إ";
			case "ARS": return $"${decimaled} ARS";
			case "AUD": return $"A${decimaled}";
			case "BRL": return $"R${decimaled}";
			case "CAD": return $"C${decimaled}";
			case "CHF": return $"Fr. {decimaled}";
			case "CLP": return $"${decimaled} CLP";
			case "CNY": return $"{decimaled}元";
			case "COP": return $"COL$ {decimaled}";
			case "CRC": return $"₡{decimaled}";
			case "EUR": return $"€{decimaled}";
			case "SEK": return $"{decimaled}kr";
			case "GBP": return $"£{decimaled}";
			case "HKD": return $"HK${decimaled}";
			case "ILS": return $"₪{decimaled}";
			case "IDR": return $"Rp{decimaled}";
			case "INR": return $"₹{decimaled}";
			case "JPY": return $"¥{decimaled}";
			case "KRW": return $"₩{decimaled}";
			case "KWD": return $"KD {decimaled}";
			case "KZT": return $"{decimaled}₸";
			case "MXN": return $"Mex${decimaled}";
			case "MYR": return $"RM {decimaled}";
			case "NOK": return $"{decimaled} kr";
			case "NZD": return $"${decimaled} NZD";
			case "PEN": return $"S/. {decimaled}";
			case "PHP": return $"₱{decimaled}";
			case "PLN": return $"{decimaled}zł";
			case "QAR": return $"QR {decimaled}";
			case "RUB": return $"{decimaled}₽";
			case "SAR": return $"SR {decimaled}";
			case "SGD": return $"S${decimaled}";
			case "THB": return $"฿{decimaled}";
			case "TRY": return $"₺{decimaled}";
			case "TWD": return $"NT$ {decimaled}";
			case "UAH": return $"₴{decimaled}";
			case "USD": return $"${decimaled}";
			case "UYU": return $"$U {decimaled}"; // yes the U goes after $
			case "VND": return $"₫{decimaled}";
			case "ZAR": return $"R {decimaled}";

			// TODO - check all of them https://partner.steamgames.com/doc/store/pricing/currencies

			default: return $"{decimaled} {Currency}";
		}
	}
}
