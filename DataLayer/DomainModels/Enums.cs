using System.ComponentModel.DataAnnotations;

namespace DataLayer.DomainModels
{
    public enum Suffixes
    {
        Invalid = 0,
        II,
        Jr
    }

    public enum Issues
    {
        [Display(Name = "--select issue--")]
        Invalid = 0,
        [Display(Name = "Early-Fall")]
        Early_Fall,
        Fall,
        [Display(Name = "Fall-Winter")]
        Fall_Winter,
        [Display(Name = "Jan-Feb")]
        Jan_Feb,
        [Display(Name = "Jul-Aug")]
        Jul_Aug,
        [Display(Name = "Mar-Apr")]
        Mar_Apr,
        [Display(Name = "May-Jun")]
        May_Jun,
        [Display(Name = "Nov-Dec")]
        Nov_Dec,
        [Display(Name = "Sep-Oct")]
        Sep_Oct,
        Spring,
        Summer,
        Winter
    }

    public enum PublicationYears
    {
        [Display(Name = "--select year--")]
        Invalid = 0,
        [Display(Name = "1990")]
        Y1990,
        [Display(Name = "1991")]
        Y1991,
        [Display(Name = "1992")]
        Y1992,
        [Display(Name = "1993")]
        Y1993,
        [Display(Name = "1994")]
        Y1994,
        [Display(Name = "1995")]
        Y1995,
        [Display(Name = "1996")]
        Y1996,
        [Display(Name = "1997")]
        Y1997,
        [Display(Name = "1998")]
        Y1998,
        [Display(Name = "1999")]
        Y1999,
        [Display(Name = "2000")]
        Y2000,
        [Display(Name = "2001")]
        Y2001,
        [Display(Name = "2002")]
        Y2002,
        [Display(Name = "2003")]
        Y2003,
        [Display(Name = "2004")]
        Y2004,
        [Display(Name = "2005")]
        Y2005,
        [Display(Name = "2006")]
        Y2006,
        [Display(Name = "2007")]
        Y2007,
        [Display(Name = "2008")]
        Y2008,
        [Display(Name = "2009")]
        Y2009,
        [Display(Name = "2010")]
        Y2010,
        [Display(Name = "2011")]
        Y2011,
        [Display(Name = "2012")]
        Y2012,
        [Display(Name = "2013")]
        Y2013,
        [Display(Name = "2014")]
        Y2014,
        [Display(Name = "2015")]
        Y2015,
        [Display(Name = "2016")]
        Y2016,
        [Display(Name = "2017")]
        Y2017,
        [Display(Name = "2018")]
        Y2018,
        [Display(Name = "2019")]
        Y2019,
        [Display(Name = "2020")]
        Y2020,
        [Display(Name = "2021")]
        Y2021,
        [Display(Name = "2022")]
        Y2022,
        [Display(Name = "2023")]
        Y2023,
        [Display(Name = "2024")]
        Y2024,
        [Display(Name = "2025")]
        Y2025,
        [Display(Name = "2026")]
        Y2026,
        [Display(Name = "2027")]
        Y2027,
        [Display(Name = "2028")]
        Y2028,
        [Display(Name = "2029")]
        Y2029,
        [Display(Name = "2030")]
        Y2030,
        [Display(Name = "2031")]
        Y2031,
        [Display(Name = "2032")]
        Y2032,
        [Display(Name = "2033")]
        Y2033,
        [Display(Name = "2034")]
        Y2034,
        [Display(Name = "2035")]
        Y2035,
        [Display(Name = "2036")]
        Y2036,
        [Display(Name = "2037")]
        Y2037,
        [Display(Name = "2038")]
        Y2038,
        [Display(Name = "2039")]
        Y2039,
        [Display(Name = "2040")]
        Y2040,
        [Display(Name = "2041")]
        Y2041,
        [Display(Name = "2042")]
        Y2042,
        [Display(Name = "2043")]
        Y2043,
        [Display(Name = "2044")]
        Y2044,
        [Display(Name = "2045")]
        Y2045,
        [Display(Name = "2046")]
        Y2046,
        [Display(Name = "2047")]
        Y2047,
        [Display(Name = "2048")]
        Y2048,
        [Display(Name = "2049")]
        Y2049,
        [Display(Name = "2050")]
        Y2050
    }
}
