public class TRRModel
{
    [FixedLengthField(9, 0)]
    public string SMLP { get; set; }

    [FixedLengthField(40, 12)]
    public string SMLP_PL { get; set; }

    [FixedLengthField(8, 62)]
    public string DATE { get; set; }

    [FixedLengthField(21, 71)]
    public string TEXT1 { get; set; }

    [FixedLengthField(11, 109)]
    public string TEXT2 { get; set; }

    [FixedLengthField(2, 131)]
    public string TEXT3 { get; set; }

    [FixedLengthField(15, 162)]
    public string TEXT4 { get; set; }
}