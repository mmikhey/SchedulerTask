using System;

/// <summary>
/// структура промежутков тактов (целые числа)
/// </summary>
/// 


public struct Interval
{
    public int starttime;
    public bool occflag;//флаг занятости; true - оборудование свободно, false - занято
    public int endtime;
}
