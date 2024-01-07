using System;
public interface IEndTurn
{
    public (Player prev, Player current) EndTurn();
}

public interface IProvinceDisplayer
{
    public void DisplayProvince(Province province);
}